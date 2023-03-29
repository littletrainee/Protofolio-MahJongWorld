using System;
using System.Linq;
using System.Threading.Tasks;

using MahJongWorld.Abstract;
using MahJongWorld.Shared;

namespace MahJongWorld.ChineseChessMahJong
{
	namespace _32Tile
	{
		public class Handler : AbstractHandler<Player>
		{
			// Property
			public Wall Wall { get; set; }

			private State State { get; set; }




			public override void Start()
			{
				SetEachListAndGameState();

				SetPlayerName();

				SetWall();

				Draw();

				SortHand();

				// set delegate to handle "clear console", wall and Each Player,
				// print to console
				Print = new PrintToConsole(Console.Clear);
				Print += GameState.PrintToConsole;
				Print += Wall.PrintToConsole;
				AddPrintToDelegate();
			}


			protected override void SetEachListAndGameState()
			{
				// Ask how many player in game
				Console.Write($"How many player you want to join?(2-4): ");
				int key;
				while (true)
				{
					bool ok = int.TryParse(Console.ReadLine(),out key);
					if (!ok || key < 2 || key > 4)
					{
						Console.Write("Wrong enter, Please renter: ");
					}
					else
					{
						Console.Clear();
						break;
					}
				}

				GameState = new();
				GameState.Initialization(key);
				Players = new();
				foreach (int i in Enumerable.Range(0, GameState.MaxPlayer))
				{
					Players.Add(new() { Code = i });
				}
				Wall = new();
			}


			protected override void SetPlayerName()
			{
				string keyin;
				// Set Player Name
				for (int i = 0; i < Players.Count; i++)
				{

					Console.Write($"Please Enter {Enum.GetName(typeof(Ordinal), i + 1)} Player Name:");
					while (true)
					{
						keyin = Console.ReadLine();
						if (keyin == "")
						{
							Console.Write("The Name should not be blank, please renter: ");
						}
						else
						{
							break;
						}
					}
					Players[i].Initialization(keyin);
					if (i == 0)
					{
						GameState.SetFirstPlayerName(Players[i].Name);
					}
				}
				Wall.Name = "Wall";
			}


			/// <summary>
			/// Set Append Chess To Wall and Shuffle it
			/// </summary>
			private void SetWall()
			{
				Wall.AppendToHand();
				Wall.Shuffle();
			}


			protected override void Draw()
			{
				Wall tempwall = Wall;
				foreach (var i in Enumerable.Range(0, 2))
				{
					foreach (Player player in Players)
					{
						foreach (var _ in Enumerable.Range(0, 2))
						{
							player.Draw(ref tempwall);
						}
					}
				}
				Players[0].Draw(ref tempwall);
			}


			protected override void SortHand()
			{
				foreach (Player player in Players)
				{
					player.SortHand();
				}
			}


			protected override void AddPrintToDelegate()
			{
				foreach (Player player in Players)
				{
					Print += new PrintToConsole(player.PrintToConsole);
				}
			}


			public override void Update()
			{
				// declare current State
				int choice = 0;
				int previousPlayerCode = 0;
				int nextPlayerCode = 0;
				State = State.CheckTsumo;
				Player tempPlayer ;

				while (GameState.GameOn)
				{
					// print to console
					Print();
					switch (State)
					{
						case State.CheckTsumo:
						{
							ResetOrder();
							// check Order[0] is Tsumo
							Order[0].TsumoCheck();
							if (Order[0].IsWin)
							{
								State = State.IsTsumo;
								GameState.GameOn = false;
								Console.WriteLine("Tsumo");
								Console.ReadLine();
								break;
							}
							// if not tsumo then discard
							GameState.NextRound(Order[0].Name);
							State = State.AskDeclareTenPai;
							break;
						}
						case State.AskDeclareTenPai:
						{
							tempPlayer = Order[0];
							DeclareTenPai(ref tempPlayer);
							State = State.ManualDiscard;
							break;
						}
						case State.ManualDiscard:
						{
							tempPlayer = Order[0];
							Discard(ref tempPlayer);
							State = State.CheckRon;
							break;
						}
						case State.AutoDiscard:
							// TODO
							break;
						case State.CheckRon:
						{
							// each player check is ron or not
							CheckRon();
							(Player,Player) pair = WhoRon();
							if (pair.Item1 != null)
							{
								//TODO
								State = State.IsRon;
								GameState.GameOn = false;
								Console.WriteLine("Ron");
								Console.ReadLine();
								continue;
							}
							State = State.CheckMeld;
							break;
						}
						case State.CheckMeld:
						{
							previousPlayerCode = Order[0].Code;
							// set nextPlayer code
							if (previousPlayerCode + 1 == GameState.MaxPlayer)
							{
								nextPlayerCode = 0;
							}
							else
							{
								nextPlayerCode = previousPlayerCode + 1;
							}

							Players[nextPlayerCode].CheckMeld(Players[previousPlayerCode].River.Last());
							if (Players[nextPlayerCode].HasMeld.Any())
							{
								choice = AskMakeMeldAndChoose(Players[nextPlayerCode]);
								// player is select
								if (choice != 0)
								{
									State = State.MakeMeld;
									break;
								}
							}
							// player no select
							State = State.DrawFromWall;
							break;
						}
						case State.MakeMeld:
						{
							tempPlayer = Players[previousPlayerCode];
							Players[nextPlayerCode].MakeChiMeld(choice, ref tempPlayer);
							GameState.TurnNext();
							State = State.ManualDiscard;
							ResetOrder();
							break;
						}
						case State.DrawFromWall:
						{
							Wall tempWall = Wall;
							Players[nextPlayerCode].Draw(ref tempWall);
							GameState.TurnNext();
							ResetOrder();
							State = State.CheckTsumo;
							break;
						}
					}
				}
			}


			protected override void ResetOrder()
			{
				Order = new() { Players[GameState.GameTurn] };
				for (int i = 0; i < Players.Count; i++)
				{
					if (i != GameState.GameTurn)
					{
						Order.Add(Players[i]);
					}
				}
			}


			protected override void Discard(ref Player player)
			{

				// ManualDiscard
				player.Discard();

				// SortHand
				player.SortHand();
			}


			public override void CheckRon()
			{
				// each player check Ron
				for (int i = 1; i < Order.Count; i++)
				{
					Order[i].RonCheck(Order[0].River.Last());
				}
				Task.WaitAll();
			}


			/// <summary>
			/// Return who is Ron 
			/// </summary>
			/// <returns>item1 is who Ron and item2 is ron by who</returns>
			private (Player, Player) WhoRon()
			{
				for (int i = 1; i < Order.Count; i++)
				{
					if (Order[i].IsWin)
					{
						return (Order[i], Order[0]);
					}
				}
				return (null, null);
			}


			/// <summary>
			/// Ask Player want make meld or not, if confirm then Print and choose which one
			/// </summary>
			/// <param name="player"></param>
			/// <returns>Not 0 meaning is choose,otherwise no </returns>
			private static int AskMakeMeldAndChoose(Player player)
			{
				Console.Write("Want to Make Meld?(y/n)");
				string key;
				while (true)
				{
					key = Console.ReadLine();
					if (key != "y" && key != "n")
					{
						Console.Write("Wrong Enter Please Renter: ");
					}
					else
					{
						break;
					}
				}
				if (key == "y")
				{
					return player.HasMeld.Count switch
					{
						int count when count > 1 => ChooseOne(player),
						_ => 1
					};
				}
				return 0;

			}


			/// <summary>
			/// if option more than one ask player which one make meld
			/// </summary>
			/// <param name="player"></param>
			/// <returns></returns>
			private static int ChooseOne(Player player)
			{
				Console.WriteLine();
				for (int i = 0; i < player.HasMeld.Count; i++)
				{
					Console.Write($"{i + 1}.");
					if (player.HasMeld[i].Item1.Color == "r")
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write($"{player.HasMeld[i].Item1.Surface}{player.HasMeld[i].Item2.Surface}");
						Console.ResetColor();
					}
					else
					{
						Console.Write($"{player.HasMeld[i].Item1.Surface}{player.HasMeld[i].Item2.Surface}");
					}
					if (i != player.HasMeld.Count - 1)
					{
						Console.Write(", ");
					}
					else
					{
						Console.WriteLine();
					}

				}
				Console.Write("Please Select Which One Want Make Meld:");
				string choice;
				int rt;
				while (true)
				{
					choice = Console.ReadLine();
					bool ok = int.TryParse(choice, out rt);
					if (choice != "1" && choice != "2" && choice != "3" || !ok || rt > player.HasMeld.Count)
					{
						Console.Write("Wrong Enter Please Renter: ");
					}
					else
					{
						break;
					}
				}

				return rt;
			}


			public void DeclareTenPai(ref Player player)
			{
				string key;
				Console.Write("Do You Want Declare TenPai?(y/n)");
				while (true)
				{
					key = Console.ReadLine();
					if (key != "y" && key != "n")
					{
						Console.Write("Wrong Enter Please Renter:");
						key = "";
					}
					else
					{
						break;
					}
				}
				if (key == "y")
				{
					player.TenPai = true;
				}
			}
		}

	}
}
