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
		public State State { get; set; }

		private BookMaker BookMaker { get; set; }

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
			GameState.Initialization();
			Players = new();
			BookMaker = new();
			BookMaker.Initialization(key);
			foreach (int _ in Enumerable.Range(0, BookMaker.MaxPlayer))
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
				player.Roll();
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
			State = State.CheckTsumo;
			while (GameState.GameOn)
			{
				// print to console
				Print();
				switch (State)
				{
					case State.CheckTsumo:
						CheckTsumo();
						break;
					case State.Discard:
						Discard();
						break;
					case State.CheckRon:
						CheckRon();
						break;
					case State.DrawFromWall:
						DrawFromWall();
						break;
				}
			}
		}


		/// <summary>
		/// Print Tsumo Text To Console
		/// </summary>
		/// <param name="player"></param>
		private void PrintTsumo()
		{
			// print tsumo to console
			Console.Write($"{Players[0].Name} is Tsumo.\nThe Hand is ");
			for (int i = 0; i < Players[0].Hand.Count; i++)
			{
				if (Players[0].Hand[i].Number is 1 or 4)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write($"{Players[0].Hand[i].Number} ");
					Console.ResetColor();
				}
				else
				{
					Console.Write($"{Players[0].Hand[i].Number} ");
				}
			}
			Console.Write("\tWin By ");
			// ^1 == Count -1
			if (Players[0].Hand[^1].Number is 1 or 4)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(Players[0].Hand[^1].Number);
				Console.ResetColor();
			}
			else
			{
				Console.WriteLine(Players[0].Hand[^1].Number);
			}
			Console.ReadLine();
		}


		protected override void CheckTsumo()
		{

			Players[0].TsumoCheck();
			if (Players[0].IsWin)
			{
				State = State.IsTsumo;
				PrintTsumo();
				GameState.GameOn = false;
				return;
			}
			GameState.NextRound(Players[0].Name);
			State = State.Discard;
		}


		protected override void Discard()
		{
			// Discard
			Players[0].Discard();

			// SortHand
			Players[0].SortHand();
			State = State.CheckRon;
		}


		/// <summary>
		/// Check Ron
		/// </summary>
		/// <returns></returns>
		public override void CheckRon()
		{
			// each player check Ron
			for (int i = 1; i < Players.Count; i++)
			{
				Players[i].RonCheck(Players[0].River);
			}
			Task.WaitAll();

			(Player,Player) pair = WhoRon();
			if (pair.Item1 != null)
			{
				State = State.IsRon;
				GameState.GameOn = false;
				PrintRon(pair);
				return;
			}
			State = State.DrawFromWall;
		}


		protected override (Player, Player) WhoRon()
		{
			for (int i = 1; i < Players.Count; i++)
			{
				if (Players[i].IsWin)
				{
					return (Players[i], Players[0]);
				}
			}
			return (null, null);
		}


		/// <summary>
		///  Print Ron Text To Console
		/// </summary>
		/// <param name="thisPlayer"></param>
		/// <param name="otherPlayerRiver"></param>
		private static void PrintRon((Player, Player) pair)
		{
			if (pair.Item1 != null)
			{
				Console.WriteLine($"{pair.Item1.Name} is Ron.");
				Console.Write("The Hand is ");
				for (int i = 0; i < pair.Item1.Hand.Count; i++)
				{
					if (pair.Item1.Hand[i].Number is 1 or 4)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write($"{pair.Item1.Hand[i].Number} ");
						Console.ResetColor();
					}
					else
					{
						Console.Write($"{pair.Item1.Hand[i].Number} ");
					}
				}
				Console.Write("\tWin By ");
				if (pair.Item2.River.Number is 1 or 4)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(pair.Item2.River.Number);
					Console.ResetColor();
				}
				else
				{
					Console.WriteLine(pair.Item2.River.Number);
				}
				Console.ReadLine();
			}
		}


		protected override void DrawFromWall()
		{
			Player tempPlayer = Players[0];
			Players[1].GetDice(ref tempPlayer);
			Players[1].Roll();
			Players[1].SortHand();
			List<Player> tempPlayers = Players;
			GameState.TurnNext(ref tempPlayers, BookMaker.MaxPlayer);
		}
	}
}
