using System.Collections.Generic;
using System.Linq;

namespace MahJongWorld.ChineseChessMahJong._32Chess
{
	public class Player : ChineseChessMahJong.Player
	{
		public int Code { get; set; }
		public List<(Chess, Chess)> HasMeld { get; set; }
		public bool TenPai { get; set; }




		public void Initialization(string s)
		{
			Name = s;
			Hand = new();
			Meld = new();
			River = new();
			HasMeld = new();
		}


		protected override bool CheckContains(List<Chess> targetlist, Chess target)
		{
			foreach (Chess c in targetlist)
			{
				if (c.Number == target.Number && c.Color == target.Color)
				{
					return true;
				}
			}
			return false;
		}


		protected override bool Establish(List<Chess> probablyEye, List<Chess> hand)
		{
			foreach (Chess eye in probablyEye)
			{
				List<Chess> temp = hand.ToList();
				RemoveEye(eye, ref temp);

				// temp is empty
				while (temp.Any())
				{
					Chess first = temp[0];
					Chess second= new(){Number= temp[0].Number+1, Color= temp[0].Color,};
					Chess third = new(){Number = temp[0].Number +2, Color = temp[0].Color,};
					if (CheckContains(temp, second) && CheckContains(temp, third))
					{
						temp.Remove(first);
						temp.RemoveAt(FindIndex(temp, second));
						temp.RemoveAt(FindIndex(temp, third));
						continue;
					}

					if (ChessIsEqual(temp[0], temp[1]) &&
						ChessIsEqual(temp[1], temp[2]) &&
						ChessIsEqual(temp[0], temp[2]))
					{
						foreach (int _ in Enumerable.Range(0, 3))
						{
							temp.RemoveAt(FindIndex(temp, first));
						}
						continue;
					}
					break;

				}
				if (!temp.Any())
				{
					return true;
				}
			}
			return false;
		}


		protected override void RemoveEye(Chess targetEye, ref List<Chess> orginal)
		{
			List<Chess> temp = orginal.ToList();
			for (int i = 0; i < 2; i++)
			{
				temp.RemoveAt(FindIndex(orginal, targetEye));
			}
			orginal = temp;
		}


		public void AutoDiscard()
		{
			River.Add(Hand[Hand.Count - 1]);
			Hand.RemoveAt(Hand.Count - 1);
		}


		/// <summary>
		/// Check Meld is in hand
		/// </summary>
		/// <param name="chess"> the previous one is discard to river </param>
		/// <returns> true if the HasMeld contains any probably meld; otherwise, false.</returns>
		public void CheckMeld(Chess chess)
		{
			Chess M2 = new(){Number = chess.Number-2,Color = chess.Color};
			Chess M1 = new(){Number = chess.Number-1,Color = chess.Color};
			Chess P1 = new(){Number = chess.Number+1,Color = chess.Color};
			Chess P2 = new(){Number = chess.Number+2,Color = chess.Color};

			// m2m1
			if (CheckContains(Hand, M2) && CheckContains(Hand, M1))
			{
				HasMeld.Add((Hand[FindIndex(Hand, M2)], Hand[FindIndex(Hand, M1)]));
			}

			// m1p1
			if (CheckContains(Hand, M1) && CheckContains(Hand, P1))
			{
				HasMeld.Add((Hand[FindIndex(Hand, M1)], Hand[FindIndex(Hand, P1)]));
			}

			// p1p2
			if (CheckContains(Hand, P1) && CheckContains(Hand, P2))
			{
				HasMeld.Add((Hand[FindIndex(Hand, P1)], Hand[FindIndex(Hand, P2)]));
			}
		}


		/// <summary>
		/// Make Chi Meld to Meld List
		/// </summary>
		/// <param name="choice"> which one player select.</param>
		/// <param name="previousPlayer">the previous player.</param>
		public void MakeChiMeld(int choice, ref Player previousPlayer)
		{

			int first = FindIndex(Hand,HasMeld[choice -1].Item1);
			int second =FindIndex(Hand, HasMeld[choice - 1].Item2) ;
			Meld.Add(new()
			{
				Hand[first],
				previousPlayer.River.Last(),
				Hand[second],
			});

			// remove pair from Hand
			Hand.RemoveAt(second);
			Hand.RemoveAt(first);
			// previousPlayer.River Remove Last one
			previousPlayer.River.RemoveAt(previousPlayer.River.Count - 1);

			// clear HasMeld
			HasMeld = new();
		}


		/// <summary>
		/// Rewrite FindIndex for Searches for target is matches the 
		/// source list
		/// </summary>
		/// <param name="source"> list source</param>
		/// <param name="target"> target element</param>
		/// <returns>i is the position, Or -1 of not found</returns>
		private static int FindIndex(List<Chess> source, Chess target)
		{
			for (int i = 0; i < source.Count; i++)
			{
				if (source[i].Number == target.Number && source[i].Color == target.Color)
				{
					return i;
				}

			}
			return -1;
		}


		/// <summary>
		/// Rewrite Equeal to fit this case
		/// </summary>
		/// <param name="first">The first chess to compare.</param>
		/// <param name="second">The second chess to compare</param>
		/// <returns>true if the chess are considered equal; otherwise false</returns>
		private static bool ChessIsEqual(Chess first, Chess second)
		{
			return first.Number == second.Number && first.Color == second.Color;
		}


		public bool TenPaiCheck()
		{
			Chess probablyChess;
			List<Chess> temphand = Hand.ToList();
			List<Chess> cloneForPopOne;
			List<Chess> removeOneFromClone;
			List<Chess> probablywinTile = new ();
			string[] color = new string[]{ "b","r"};

			for (int i = 0; i < temphand.Count; i++)
			{
				// reset CloneForPopOne 
				cloneForPopOne = temphand.ToList();
				// remove one from clone for popone by temphandindex [5-4]
				cloneForPopOne.RemoveAt(i);
				foreach (string c in color)
				{
					for (int j = 1; j < 8; j++)
					{
						removeOneFromClone = cloneForPopOne.ToList();
						probablyChess = new() { Number = j, Color = c };
						removeOneFromClone.Add(probablyChess);
						if (ProbablyWin(removeOneFromClone))
						{
							if (!CheckContains(probablywinTile, probablyChess))
							{
								probablywinTile.Add(probablyChess);
							}
						}
					}
				}
			}
			return probablywinTile.Any();
		}


		private static bool ProbablyWin(List<Chess> source)
		{
			Player p = new(){Hand = source,Meld = new()};
			p.SortHand();
			p.TsumoCheck();
			return p.IsWin;
		}
	}
}
