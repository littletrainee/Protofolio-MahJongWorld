﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MahJongWorld.Abstract;
using MahJongWorld.Shared;

namespace MahJongWorld.ChineseChessMahJong._32Chess
{
	public class Handler : AbstractHandler<Player>
	{
		// Property
		public ChineseChessMahJong._32Chess.Wall Wall { get; set; }

		private State State { get; set; }

		private Score Score { get; set; }




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
			ChineseChessMahJong.Wall tempwall = Wall;
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
			Score = new();

			while (GameState.GameOn)
			{
				if (State == State.Draw)
				{
					GameState.GameOn = false;
					continue;
				}
				// print to console
				Print();
				switch (State)
				{
					#region CheckTsumo:
					case State.CheckTsumo:
						CheckTsumo();
						break;
					#endregion

					#region CheckTenPai
					case State.CheckTenPai:
						CheckTenPai();
						break;
					#endregion

					#region AskDeclareTenPai
					case State.AskDeclareTenPai:
						AskDeclareTenPai();
						break;
					#endregion

					#region ManualDiscard
					case State.ManualDiscard:
						ManualDiscard();
						break;
					#endregion

					#region AutoDiscard
					case State.AutoDiscard:
						AutoDiscard();
						break;
					#endregion

					#region CheckRon
					case State.CheckRon:
						CheckRon();
						break;
					#endregion

					#region CheckMeld
					case State.CheckMeld:
						CheckMeld(ref previousPlayerCode, ref nextPlayerCode, ref choice);
						break;
					#endregion

					#region MakeMeld
					case State.MakeMeld:
						MakeMeld(ref previousPlayerCode, ref nextPlayerCode, ref choice);
						break;
					#endregion

					#region DrawFromWall
					case State.DrawFromWall:
						DrowFromWall(ref nextPlayerCode);
						break;
						#endregion
				}
				Thread.Sleep(500);
			}

			// caculator score
			if (State == State.IsRon || State == State.IsTsumo)
			{
				Print();
				Score.PrintPatterns();
				Console.ReadLine();
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


		/// <summary>
		/// Check Order[0] is Tsumo
		/// </summary>
		private void CheckTsumo()
		{
			ResetOrder();
			// check Order[0] is Tsumo
			Order[0].TsumoCheck();
			if (Order[0].IsWin)
			{
				State = State.IsTsumo;
				GameState.GameOn = false;
				Score.Initilization(Order[0], GameState, State);
				return;
			}
			GameState.LastOne = false;
			// if not tsumo then discard
			GameState.NextRound(Order[0].Name);
			State = State.CheckTenPai;
		}


		/// <summary>
		/// Check Order[0] can declare tenpai
		/// </summary>
		private void CheckTenPai()
		{
			if (Order[0].TenPai)
			{
				State = State.AutoDiscard;
				return;
			}
			if (Order[0].TenPaiCheck())
			{
				State = State.AskDeclareTenPai;
				return;
			}
			State = State.ManualDiscard;
		}


		/// <summary>
		/// Order[0] declare tenpai
		/// </summary>
		private void AskDeclareTenPai()
		{
			Player tempPlayer = Order[0];
			DeclareTenPai(ref tempPlayer);
			State = State.ManualDiscard;
		}


		/// <summary>
		/// Order[0] Manual Discard And Sort 
		/// </summary>
		protected override void ManualDiscard()
		{
			Player tempPlayer = Order[0];
			// ManualDiscard
			tempPlayer.ManualDiscard();

			// SortHand
			tempPlayer.SortHand();
			State = State.CheckRon;
		}


		/// <summary>
		/// Order[0] Auto Discard AndSort
		/// </summary>
		private void AutoDiscard()
		{
			Player tempPlayer = Order[0];
			// AutoDiscard
			tempPlayer.AutoDiscard();

			// SortHand
			tempPlayer.SortHand();
			State = State.CheckRon;
		}


		public override void CheckRon()
		{
			// each player check Ron
			for (int i = 1; i < Order.Count; i++)
			{
				Order[i].RonCheck(Order[0].River.Last());
			}
			Task.WaitAll();

			(Player,Player) pair = WhoRon();
			if (pair.Item1 != null)
			{
				//TODO
				State = State.IsRon;
				GameState.GameOn = false;
				Score.Initilization(pair.Item1, GameState, State);
				Console.ReadLine();
				return;
			}
			State = State.CheckMeld;
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
		/// Check Which player can make meld
		/// </summary>
		/// <param name="previousPlayerCode"> the one who was originally Order[0] </param>
		/// <param name="nextPlayerCode"> the latter one who was after Order[0] </param>
		/// <param name="choice"> the latter one who was after Order[0]'s choice</param>
		private void CheckMeld(ref int previousPlayerCode, ref int nextPlayerCode, ref int choice)
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
			// if next player not tenpai
			if (!Players[nextPlayerCode].TenPai)
			{
				Players[nextPlayerCode].CheckMeld(Players[previousPlayerCode].River.Last());
				if (Players[nextPlayerCode].HasMeld.Any())
				{
					choice = AskMakeMeldAndChoose(Players[nextPlayerCode]);
					// player is select
					if (choice != 0)
					{
						State = State.MakeMeld;
						return;
					}
				}
			}
			if (Wall.Hand.Count > 0)
			{
				// player no select or player is tenpai
				State = State.DrawFromWall;
			}
			else
			{
				State = State.Draw;
			}
		}


		/// <summary>
		/// the player make meld
		/// </summary>
		/// <param name="previousPlayerCode"> the one who was originally Order[0] </param>
		/// <param name="nextPlayerCode"> the latter one who was after Order[0] </param>
		/// <param name="choice"> the latter one who was after Order[0]'s choice</param>
		private void MakeMeld(ref int previousPlayerCode, ref int nextPlayerCode, ref int choice)
		{
			Player tempPlayer = Players[previousPlayerCode];
			Players[nextPlayerCode].MakeChiMeld(choice, ref tempPlayer);
			GameState.TurnNext();
			State = State.CheckTenPai;
			ResetOrder();
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


		/// <summary>
		/// Ask Player to Declare TenPai
		/// </summary>
		/// <param name="player"> the player who is being asked</param>
		public static void DeclareTenPai(ref Player player)
		{
			string key;
			Console.Write("Do You Want Declare TenPai?(y/n)");
			while (true)
			{
				key = Console.ReadLine();
				if (key != "y" && key != "n")
				{
					Console.Write("Wrong Enter Please Renter:");
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


		/// <summary>
		/// Next Player Draw Chess From Wall
		/// </summary>
		/// <param name="nextPlayerCode"> the latter one who was after Order[0]</param>
		private void DrowFromWall(ref int nextPlayerCode)
		{
			ChineseChessMahJong.Wall tempWall = Wall;
			GameState.LastOne = Wall.Hand.Count == 1;
			Players[nextPlayerCode].Draw(ref tempWall);
			GameState.TurnNext();
			ResetOrder();
			State = State.CheckTsumo;
		}
	}

}
