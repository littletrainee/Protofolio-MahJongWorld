using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MahJongWorld.Abstract;
using MahJongWorld.Shared;

namespace MahJongWorld.DiceMahJong
{
	public class Handler : AbstractHandler<Player>
	{

		public override void Start()
		{
			SetEachListAndGameState();

			SetPlayerName();

			Draw();

			RollDice();

			SortHand();

			// set delegate to print to console word
			Print += new PrintToConsole(Console.Clear);
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
		}


		protected override void Draw()
		{
			// each player get 4 dice
			for (int i = 0; i < 4; i++)
			{
				foreach (Player player in Players)
				{
					player.Hand.Add(new());
				}
			}
			// fisrt player get 5th dice
			Players[0].Hand.Add(new());
		}


		/// <summary>
		/// Each Player Roll Dice.
		/// </summary>
		private void RollDice()
		{
			// Each Player Roll Dice
			foreach (Player player in Players)
			{
				foreach (Dice d in player.Hand)
				{
					d.Roll(new());
				}
			}
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
			while (GameState.GameOn)
			{
				// print to console
				Print();
				ResetOrder();

				// check Order[0] is Tsumo

				if (IsTsumo(Order[0]))
				{
					GameState.GameOn = false;
					continue;
				}
				GameState.TurnNext();
				Print();

				// if not tsumo then discard
				Player tempPlayer = Order[0];
				Discard(ref tempPlayer);
				Print();

				// each playercheck is ron or not
				List<Player> tempOrder = Order;
				CheckRon(tempOrder);
				if (IsRon(WhoRon(tempOrder)))
				{
					GameState.GameOn = false;
					continue;
				}

				// Order[1] IsWin set to false
				Dice tempdice = Order[0].River;
				Order[1].GetDice(ref tempdice);

				Order[1].Hand.ForEach(x => { x.Roll(new()); });// Roll
				Order[1].SortHand();
				GameState.TurnNext();
			}
		}


		protected override void ResetOrder()
		{
			Order = new()
				{
					Players[GameState.GameTurn]
				};
			for (int i = 0; i < Players.Count; i++)
			{
				if (i != GameState.GameTurn)
				{
					Order.Add(Players[i]);
				}
			}
		}


		/// <summary>
		/// Print Tsumo Text To Console
		/// </summary>
		/// <param name="player"></param>
		private static bool IsTsumo(Player player)
		{
			player.TsumoCheck();
			if (player.IsWin)
			{
				// print tsumo to console
				Console.Write($"{player.Name} is Tsumo.\nThe Hand is ");
				for (int i = 0; i < player.Hand.Count; i++)
				{
					if (player.Hand[i].Number is 1 or 4)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write($"{player.Hand[i].Number} ");
						Console.ResetColor();
					}
					else
					{
						Console.Write($"{player.Hand[i].Number} ");
					}
				}
				Console.Write("\tWin By ");
				// ^1 == Count -1
				if (player.Hand[^1].Number is 1 or 4)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(player.Hand[^1].Number);
					Console.ResetColor();
				}
				else
				{
					Console.WriteLine(player.Hand[^1].Number);
				}
				Console.ReadKey();
			}
			return player.IsWin;

		}


		protected override void Discard(ref Player player)
		{

			// Discard
			player.Discard();

			// SortHand
			player.SortHand();
		}


		/// <summary>
		/// Check Ron
		/// </summary>
		/// <returns></returns>
		protected override void CheckRon(List<Player> order)
		{
			// each player check Ron
			for (int i = 1; i < order.Count; i++)
			{
				order[i].RonCheck(order[0].River);
			}
			Task.WaitAll();
		}



		private static (Player, Dice) WhoRon(List<Player> order)
		{
			for (int i = 1; i < order.Count; i++)
			{
				if (order[i].IsWin)
				{
					return (order[i], order[0].River);
				}
			}
			return (null, order[0].River);
		}


		/// <summary>
		///  Print Ron Text To Console
		/// </summary>
		/// <param name="thisPlayer"></param>
		/// <param name="otherPlayerRiver"></param>
		private static bool IsRon((Player, Dice) player)
		{
			if (player.Item1 != null)
			{
				Console.WriteLine($"{player.Item1.Name} is Ron.");
				Console.Write("The Hand is ");
				for (int i = 0; i < player.Item1.Hand.Count; i++)
				{
					if (player.Item1.Hand[i].Number is 1 or 4)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write($"{player.Item1.Hand[i].Number} ");
						Console.ResetColor();
					}
					else
					{
						Console.Write($"{player.Item1.Hand[i].Number} ");
					}
				}
				Console.Write("\tWin By ");
				if (player.Item2.Number is 1 or 4)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(player.Item2.Number);
					Console.ResetColor();
				}
				else
				{
					Console.WriteLine(player.Item2.Number);
				}
				Console.ReadLine();
				return player.Item1.IsWin;
			}
			return false;
		}
	}


}
