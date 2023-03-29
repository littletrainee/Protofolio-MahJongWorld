﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MahJongWorld.Abstract;
using MahJongWorld.Shared;

namespace MahJongWorld.ChineseChess
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
				foreach (int _ in Enumerable.Range(0, GameState.MaxPlayer))
				{
					Players.Add(new());
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
				State state = State.Draw;
				while (GameState.GameOn)
				{
					// print to console
					Print();
					ResetOrder();

					// check Order[0] is Tsumo
					Order[0].TsumoCheck();
					if (Order[0].IsWin)
					{
						state = State.Tsumo;
						GameState.GameOn = false;
						continue;
					}

					// if not tsumo then discard
					GameState.NextRound(Players[0].Name);
					Player tempPlayer = Order[0];
					GameState tempGameState = GameState;
					Discard(ref tempPlayer, ref tempGameState);
					Print();

					// each player check is ron or not
					List<Player> tempOrder = Order;
					CheckRon(tempOrder);
					(Player,Player) pair = WhoRon(tempOrder);
					if (pair.Item1 != null)
					{
						//TODO pair.item1 is winner
						state = State.Ron;
						GameState.GameOn = false;
						continue;
					}

					// check has meld
					(Ordinal,Ordinal) whichOneAndLast =CheckMeld() ;
					// if whichOneAndLast.Item1 != None meaning is Hasmeld
					if (whichOneAndLast.Item1 != Ordinal.None)
					{
						int choice = whichOneAndLast.Item1 switch
						{
							Ordinal.First =>AskMakeMeldAndChoose(Players[0]),
							Ordinal.Second => AskMakeMeldAndChoose(Players[1]),
							Ordinal.Third => AskMakeMeldAndChoose(Players[2]),
							Ordinal.Fourth => AskMakeMeldAndChoose(Players[3]),
							_ => throw new NotImplementedException(),
						};
						// if choice != 0
						if (choice != 0)
						{
							tempPlayer = Players[(int)whichOneAndLast.Item2];
							switch (whichOneAndLast.Item1)
							{
								case Ordinal.First:
									Players[0].MakeChiMeld(choice, ref tempPlayer);
									GameState.GameTurn = 0;
									break;
								case Ordinal.Second:
									Players[1].MakeChiMeld(choice, ref tempPlayer);
									GameState.GameTurn = 1;
									break;
								case Ordinal.Third:
									Players[2].MakeChiMeld(choice, ref tempPlayer);
									GameState.GameTurn = 2;
									break;
								case Ordinal.Fourth:
									Players[3].MakeChiMeld(choice, ref tempPlayer);
									GameState.GameTurn = 3;
									break;
							}
							goto NextPlayer;
						}
					}
				// whichOneAndLast.Item1 == None
				NextPlayer:
					GameState.TurnNext();
				}


				// whitch state
				switch (state)
				{
					case State.Tsumo:
						break;
					case State.Ron:
						break;
					default:
						break;
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


			protected override void Discard(ref Player player, ref GameState gameState)
			{
				// Turn to Next Round
				gameState.NextRound(player.Name);

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


			/// <summary>
			/// Accroding to the number of playrs to choose to judge the meld ,
			/// of current player. Prompt In 32-Chess only Chi can meld.
			/// </summary>
			/// <returns>which player has meld, or <see cref="Ordinal.None"/> 
			/// if no one has it.</returns>
			private (Ordinal, Ordinal) CheckMeld()
			{
				// how many player in this game
				switch (Order.Count)
				{
					// if 2 player
					case 2:
						if (Order[0].Name == Players[0].Name) // Order[0] == Players[0]
						{
							if (Players[1].CheckMeld(Players[0].River.Last()))
							{
								return (Ordinal.Second, Ordinal.First);
							}
						}


						else if (Order[0].Name == Players[1].Name) // Order[0] == Players[1]
						{
							if (Players[0].CheckMeld(Players[1].River.Last()))
							{
								return (Ordinal.First, Ordinal.Second);
							}
						}
						break;

					// if 3 player
					case 3:

						if (Order[0].Name == Players[0].Name) // Order[0] == Players[0]
						{
							if (Players[1].CheckMeld(Players[0].River.Last()))
							{
								return (Ordinal.Second, Ordinal.First);
							}
						}


						else if (Order[0].Name == Players[1].Name) // Order[0] == Players[1]
						{
							if (Players[2].CheckMeld(Players[1].River.Last()))
							{
								return (Ordinal.Third, Ordinal.Second);
							}
						}


						else if (Order[0].Name == Players[2].Name) // Order[0] == Players[2]
						{
							if (Players[0].CheckMeld(Players[2].River.Last()))
							{
								return (Ordinal.First, Ordinal.Third);
							}
						}
						break;

					// if 4 player 
					case 4:

						if (Order[0].Name == Players[0].Name) // Order[0] == Players[0]
						{
							if (Players[1].CheckMeld(Players[0].River.Last()))
							{
								return (Ordinal.Second, Ordinal.First);
							}
						}


						else if (Order[0].Name == Players[1].Name) // Order[0] == Playes[1]
						{
							if (Players[2].CheckMeld(Players[1].River.Last()))
							{
								return (Ordinal.Third, Ordinal.Second);
							}
						}


						else if (Order[0].Name == Players[2].Name) // Order[0] == Players[2]
						{
							if (Players[3].CheckMeld(Players[2].River.Last()))
							{
								return (Ordinal.Fourth, Ordinal.Third);
							}
						}


						else if (Order[0].Name == Players[3].Name) // Order[0] == Players[3]
						{
							if (Players[0].CheckMeld(Players[3].River.Last()))
							{
								return (Ordinal.First, Ordinal.Fourth);
							}
						}
						break;
				}
				return (Ordinal.None, Ordinal.None);
			}


			private int AskMakeMeldAndChoose(Player player)
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