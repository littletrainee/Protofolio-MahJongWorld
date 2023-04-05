using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MahJongWorld.Abstract;
using MahJongWorld.Shared;

namespace MahJongWorld.ChineseChessMahJong._56Chess
{
	public class Handler : AbstractHandler<Player>
	{
		// Property
		public Wall Wall { get; set; }

		private State State { get; set; }

		private Score Score { get; set; }

		private BookMaker BookMaker { get; set; }




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
				bool ok = int.TryParse(Console.ReadLine(), out key);
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
			foreach (int i in Enumerable.Range(0, BookMaker.MaxPlayer))
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
			List<Chess> tempwall = Wall.Hand;
			foreach (var i in Enumerable.Range(0, 3))
			{
				foreach (Player player in Players)
				{
					foreach (var _ in Enumerable.Range(0, 2))
					{
						player.Draw(ref tempwall);
					}
				}
			}
			foreach (Player player in Players)
			{
				player.Draw(ref tempwall);
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
		ReUpdate:
			bool delaySetAutoDiscard = false;
			// declare current State
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
						delaySetAutoDiscard = AskDeclareTenPai();
						break;
					#endregion

					#region Discard
					case State.Discard:
						Discard();
						if (delaySetAutoDiscard)
						{
							Players[0].TenPai = delaySetAutoDiscard;
							delaySetAutoDiscard = false;
						}
						break;
					#endregion

					#region CheckRon
					case State.CheckRon:
						CheckRon();
						break;
					#endregion

					#region CheckMeld
					case State.CheckMeld:
						CheckMeld();
						break;
					#endregion

					#region MakeSequence
					case State.MakeSequence:
						MakeSequence();
						break;
					#endregion

					#region MakeTriple
					case State.MakeTriple:
						MakeTriple();
						break;
					#endregion

					#region MakeQuadruple
					case State.MakeBigQuadruple:
					case State.MakeSmallQuadruple:
					case State.MakeConcealedQuadruple:
						MakeQuadruple();
						break;
					#endregion

					#region CheckConcealedQuadruple
					case State.CheckConcealedQuadruple:
						CheckConcealedOrSmallQuadruple();
						break;
					#endregion

					#region DrawFromWall
					case State.DrawFromWall:
						DrawFromWall();
						break;
						#endregion
				}
				Thread.Sleep(100);
			}

			// caculator score
			if (State == State.IsRon || State == State.IsTsumo)
			{
				Print();
				Score.PrintPatterns();
				Console.ReadLine();
				ReStart();
				if (GameState.GameOn)
				{
					goto ReUpdate;
				}
			}
		}


		protected override void CheckTsumo()
		{
			// check Player[0] is Tsumo
			Players[0].TsumoCheck();
			if (Players[0].IsWin)
			{
				State = State.IsTsumo;
				GameState.GameOn = false;
				BookMaker.Winner = Players[0].Code;
				BookMaker.WinbyWho = Players[0].Code;
				Score.Initilization(Players[0], GameState, State, BookMaker);
				Console.ReadKey();
				return;
			}
			GameState.LastOne = false;
			// if not tsumo then discard
			GameState.NextRound(Players[0].Name);
			State = State.CheckTenPai;
		}


		/// <summary>
		/// Check Player[0] can declare tenpai
		/// </summary>
		private void CheckTenPai()
		{
			if (Players[0].TenPaiCheck())
			{
				State = State.AskDeclareTenPai;
				return;
			}
			State = State.Discard;
		}


		/// <summary>
		/// Player[0] declare tenpai
		/// </summary>
		private bool AskDeclareTenPai()
		{
			State = State.Discard;
			if (!Players[0].TenPai)
			{
				return DeclareTenPai();
			}
			return false;
		}


		/// <summary>
		/// Player[0] Manual Or Auto Discard And Sort 
		/// </summary>
		protected override void Discard()
		{
			Player tempPlayer = Players[0];
			// Discard
			tempPlayer.Discard();

			// SortHand
			tempPlayer.SortHand();
			State = State.CheckRon;
		}


		public override void CheckRon()
		{
			// each player check Ron
			for (int i = 1; i < Players.Count; i++)
			{
				Players[i].RonCheck(Players[0].River.Last());
			}
			Task.WaitAll();

			(Player, Player) pair = WhoRon();
			if (pair.Item1 != null)
			{
				State = State.IsRon;
				GameState.GameOn = false;
				BookMaker.Winner = pair.Item1.Code;
				BookMaker.WinbyWho = pair.Item2.Code;
				Score.Initilization(pair.Item1, GameState, State, BookMaker);
				Console.ReadLine();
				return;
			}
			State = State.CheckMeld;
		}


		/// <summary>
		/// Return who is Ron 
		/// </summary>
		/// <returns>item1 is who Ron and item2 is ron by who</returns>
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
		/// Check Which player can make meld
		/// </summary>
		/// <param name="previousPlayerCode"> the one who was originally Player[0] </param>
		/// <param name="nextPlayerCode"> the latter one who was after Player[0] </param>
		/// <param name="choice"> the latter one who was after Player[0]'s choice</param>
		private void CheckMeld()
		{
			List<Player> tempPlayers;
			if (Players.Count > 2)
			{
				// not next player check Tripe or BigQuadruple first
				for (int i = 2; i < Players.Count; i++)
				{
					Players[i].CheckTripleAndBigQuadruple(Players[0].River.Last());
				}
				Task.WaitAll();
				(Player, Player) pair = WhoHasTripleOrQuadruple();
				if (pair.Item1 != null)
				{
					MeldState choice = AskMakeMeld(pair.Item1);
					if (choice != MeldState.None)
					{
						State = choice switch
						{
							MeldState.Triple => State.MakeTriple,
							MeldState.BigQuadruple => State.MakeBigQuadruple,
							_ => State.DrawFromWall
						};
						// turn to target player	
						do
						{
							tempPlayers = Players;
							GameState.TurnNext(ref tempPlayers, BookMaker.MaxPlayer);
							GameState.NextRound(tempPlayers[0].Name);
						} while (Players[0].Code != pair.Item1.Code);
						return;
					}
				}
			}

			// not next player no Triple or BigQuadruple then next player check Triple, BigQuadruple and sequence
			if (!Players[1].TenPai)
			{
				Players[1].CheckTripleAndBigQuadruple(Players[0].River.Last());
				Task.WaitAll();
				Players[1].CheckSequence(Players[0].River.Last());
				if (Players[1].HasMeld.Any() || Players[1].MeldState != MeldState.None)
				{
					State = AskMakeMeld(Players[1]) switch
					{
						MeldState.Sequence => State.MakeSequence,
						MeldState.Triple => State.MakeTriple,
						MeldState.BigQuadruple => State.MakeBigQuadruple,
						_ => State.DrawFromWall
					};
					// turn to target player
					tempPlayers = Players;
					GameState.TurnNext(ref tempPlayers, BookMaker.MaxPlayer);
					GameState.NextRound(tempPlayers[0].Name);
					return;
				}
			}
			State = State.DrawFromWall;
			// turn to target player
			tempPlayers = Players;
			GameState.TurnNext(ref tempPlayers, BookMaker.MaxPlayer);
			GameState.NextRound(tempPlayers[0].Name);
		}


		private (Player, Player) WhoHasTripleOrQuadruple()
		{
			for (int i = 1; i < Players.Count; i++)
			{
				if (Players[i].MeldState is MeldState.Triple or MeldState.BigQuadruple)
				{
					return (Players[i], Players[0]);
				}
			}
			return (null, null);
		}


		/// <summary>
		/// the player make meld
		/// </summary>
		/// <param name="previousPlayerCode"> the one who was originally Player[0] </param>
		/// <param name="nextPlayerCode"> the latter one who was after Player[0] </param>
		/// <param name="choice"> the latter one who was after Player[0]'s choice</param>
		private void MakeSequence()
		{
			int select = 1;
			if (Players[0].HasMeld.Count != 1)
			{
				select = ChooseOne(Players[0]);
			}
			Player tempPlayer = Players[^1];
			Players[0].MakeSequence(select, ref tempPlayer);
			State = State.CheckTenPai;
		}

		/// <summary>
		/// Ask Player want make meld or not, if confirm then Print and choose which one
		/// </summary>
		/// <param name="player"></param>
		/// <returns>Not 0 meaning is choose,otherwise no </returns>
		public static MeldState AskMakeMeld(Player player)
		{
			string probablyselect = "";
			// sequence, triple and quadruple
			if (player.HasMeld.Any() && player.MeldState == MeldState.BigQuadruple)
			{
				Console.WriteLine("Want to Chi, Pong Or Kang?(c/p/k/s)");
				probablyselect = "cpks";
			}

			// sequence and triple
			if (player.HasMeld.Any() && player.MeldState == MeldState.Triple)
			{
				Console.WriteLine("Want to Chi Or Pong?(c/p/s)");
				probablyselect = "cps";
			}

			// sequence
			if (player.HasMeld.Any() && player.MeldState == MeldState.None)
			{
				Console.WriteLine("Want to Chi?(c/s)");
				probablyselect = "cs";
			}

			// triple and quadruple
			if (!player.HasMeld.Any() && player.MeldState == MeldState.BigQuadruple)
			{
				Console.WriteLine("Want to Pong Or Kang?(p/k/s)");
				probablyselect = "pks";
			}

			// triple
			if (!player.HasMeld.Any() && player.MeldState == MeldState.Triple)
			{
				Console.WriteLine("Want to Pong?(p/s)");
				probablyselect = "ps";
			}

			string key;
			while (true)
			{
				key = Console.ReadLine();
				if (!probablyselect.Contains(key))
				{
					Console.Write("Wrong Enter Please Renter: ");
				}
				else
				{
					break;
				}
			}
			return key switch
			{
				"k" => MeldState.BigQuadruple,
				"p" => MeldState.Triple,
				"c" => MeldState.Sequence,
				_ => MeldState.None
			};

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


		private void MakeTriple()
		{
			Player previousPlayer = Players[^1];
			Players[0].MakeTriple(ref previousPlayer);
			State = State.CheckTenPai;
		}


		private void MakeQuadruple()
		{
			Wall tempWall = Wall;
			switch (State)
			{
				case State.MakeBigQuadruple:
					Player tempPlayer = Players[^1];
					Players[0].MakeBigQuadruple(ref tempPlayer, ref tempWall);
					break;
				case State.MakeSmallQuadruple:
					Players[0].MakeSmallQuadruple(ref tempWall);
					break;
				case State.MakeConcealedQuadruple:
					Players[0].MakeConcealedQuadruple(ref tempWall);
					break;
			}
			State = State.CheckConcealedQuadruple;
		}


		/// <summary>
		/// Ask Player to Declare TenPai
		/// </summary>
		/// <param name="player"> the player who is being asked</param>
		public static bool DeclareTenPai()
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
			return key == "y";
		}


		/// <summary>
		/// Next Player Draw Chess From Wall
		/// </summary>
		/// <param name="nextPlayerCode"> the latter one who was after Player[0]</param>
		protected override void DrawFromWall()
		{
			if (Wall.Hand.Count > 0)
			{
				List<Chess> tempWall = Wall.Hand;
				GameState.LastOne = Wall.Hand.Count == 1;
				Players[0].Draw(ref tempWall);
				State = State.CheckConcealedQuadruple;
			}
			else
			{
				State = State.Draw;
			}
		}


		private void CheckConcealedOrSmallQuadruple()
		{
			if (!Players[0].TenPai && Players[0].Meld.Count < 2)
			{
				Players[0].CheckSmallOrConcealedQuadruple();
				State = Players[0].MeldState switch
				{
					MeldState.SmallQuadruple => State.MakeSmallQuadruple,
					MeldState.ConcealedQuadruple => State.MakeConcealedQuadruple,
					_ => State.CheckTsumo
				};
			}
			State = State.CheckTsumo;
		}

		private void ReStart()
		{
			Console.Write("Continue This Game?(y/n)");
			string key;
			while (true)
			{
				key = Console.ReadLine();
				if (key is not "y" or "n")
				{
					Console.Write("Wrong enter please reneter:");
				}
				else
				{
					break;
				}
			}
			if (key is "y")
			{
				BookMaker.ContinueOrNext();
				SetWall();
				GameState.GameOn = true;
				GameState.GameRound = 0;
				GameState.LastOne = false;
				// reset each player
				foreach (Player player in Players)
				{
					player.Hand = new();
					player.Meld = new();
					player.River = new();
					player.TenPai = false;
					player.HasMeld = new();
					player.MeldState = MeldState.None;
					player.Concealed = true;
					player.Eye = new();
					player.TwoKang = 0;
				}
				Draw();
				SortHand();
			}
		}
	}
}
