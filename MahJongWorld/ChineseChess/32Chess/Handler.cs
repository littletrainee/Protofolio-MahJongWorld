using System;
using System.Collections.Generic;
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
				// declare current state
				int choice = 0;
				int previousPlayerCode = 0;
				int nextPlayerCode = 0;
				State state = State.CheckTsumo;
				Player tempPlayer ;

				while (GameState.GameOn)
				{
					// print to console
					Print();
					switch (state)
					{
						case State.CheckTsumo:
						{
							ResetOrder();
							// check Order[0] is Tsumo
							Order[0].TsumoCheck();
							if (Order[0].IsWin)
							{
								state = State.IsTsumo;
								GameState.GameOn = false;
								break;
							}
							// if not tsumo then discard
							state = State.Discard;
							GameState.NextRound(Order[0].Name);
							break;
						}
						case State.Discard:
						{
							tempPlayer = Order[0];
							//tempGameState = GameState;
							Discard(ref tempPlayer);
							state = State.CheckRon;
							break;
						}
						case State.CheckRon:
						{
							// each player check is ron or not
							List<Player> tempOrder = Order;
							CheckRon(tempOrder);
							(Player,Player) pair = WhoRon(tempOrder);
							if (pair.Item1 != null)
							{
								//TODO pair.item1 is winner
								state = State.IsRon;
								GameState.GameOn = false;
								continue;
							}
							state = State.CheckMeld;
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
									state = State.MakeMeld;
									break;
								}
							}
							// player no select
							state = State.DrawFromWall;
							break;
						}
						case State.MakeMeld:
						{
							tempPlayer = Players[previousPlayerCode];
							Players[nextPlayerCode].MakeChiMeld(choice, ref tempPlayer);
							GameState.TurnNext();
							state = State.Discard;
							ResetOrder();
							break;
						}
						case State.DrawFromWall:
						{
							Wall tempWall = Wall;
							Players[nextPlayerCode].Draw(ref tempWall);
							GameState.TurnNext();
							ResetOrder();
							state = State.CheckTsumo;
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

				// Discard
				player.Discard();

				// SortHand
				player.SortHand();
			}


			protected override void CheckRon(List<Player> order)
			{
				// each player check Ron
				for (int i = 1; i < order.Count; i++)
				{
					order[i].RonCheck(order[0].River.Last());
				}
				Task.WaitAll();
			}


			private static (Player, Player) WhoRon(List<Player> order)
			{
				for (int i = 1; i < order.Count; i++)
				{
					if (order[i].IsWin)
					{
						return (order[i], order[0]);
					}
				}
				return (null, null);
			}


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
				while (true)
				{
					choice = Console.ReadLine();
					if (choice != "1" && choice != "2" && choice != "3")
					{
						Console.Write("Wrong Enter Please Renter: ");
					}
					else
					{
						break;
					}
				}
				int rt = int.Parse(choice);
				return rt;
			}
		}

	}
}
