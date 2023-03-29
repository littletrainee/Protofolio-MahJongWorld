using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MahJongWorld.Abstract;

namespace MahJongWorld.ChineseChess
{
	public class Player : AbstractPlayer<Chess>
	{
		public List<List<Chess>> Meld { get; set; }
		public List<Chess> River { get; set; }
		public List<(Chess, Chess)> HasMeld { get; set; }






		public void Initialization(string s)
		{
			Name = s;
			Hand = new();
			Meld = new();
			River = new();
			HasMeld = new();
		}


		public void Draw(ref Wall wall)
		{
			Hand.Add(wall.Hand[0]);
			wall.Hand.RemoveAt(0);
		}


		public override void SortHand()
		{
			string[] color = new string[]{ "b","r"};
			List<Chess> temphand = new();
			foreach (string s in color)
			{
				foreach (int n in Enumerable.Range(0, 8))
				{
					foreach (Chess c in Hand)
					{
						if (c.Number == n && c.Color == s)
						{
							temphand.Add(c);
						}
					}
				}
			}
			Hand = temphand;
		}


		public override void PrintToConsole()
		{
			Console.Write($"{Name}'s Hand: ");
			foreach (Chess c in Hand)
			{
				if (c.Color == "r")
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write(c.Surface);
					Console.ResetColor();
				}
				else
				{
					Console.Write(c.Surface);
				}
				if (c != Hand.Last())
				{
					Console.Write(", ");
				}
				if (c == Hand.Last())
				{
					Console.WriteLine();
				}
			}
			Console.Write($"{Name}'s Meld: [ ");
			foreach (List<Chess> meld in Meld)
			{
				foreach (Chess c in meld)
				{
					if (c == meld.First())
					{
						Console.Write("[ ");
					}
					if (c != meld.First())
					{
						Console.Write(", ");
					}
					if (c.Color == "r")
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write(c.Surface);
						Console.ResetColor();
					}
					else
					{
						Console.Write(c.Surface);
					}
				}
				if (meld != Meld.Last())
				{
					Console.Write(", ");
				}
				if (meld == Meld.Last())
				{
					Console.Write(" ]");

				}
			}
			Console.WriteLine(" ]");
			Console.Write($"{Name}'s River: ");
			foreach (Chess c in River)
			{
				if (c.Color == "r")
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write(c.Surface);
					Console.ResetColor();
				}
				else
				{
					Console.Write(c.Surface);
				}
				if (c != River.Last())
				{
					Console.Write(", ");
				}
				if (c == River.Last())
				{
					Console.WriteLine();
				}
			}
			Console.WriteLine("\n");
		}


		public override void TsumoCheck()
		{
			// Clone player
			Player tempPlayer = new(){Hand = Hand.ToList(),Meld = Meld.ToList()};
			// sorthand
			tempPlayer.SortHand();
			Establish(FindProbablyEye(tempPlayer.Hand), tempPlayer.Hand);
		}


		public override void RonCheck(Chess target)
		{
			Task Task = Task.Factory.StartNew(() =>
			{
				// Clone player
				Player tempPlayer = new(){Hand = Hand.ToList(),Meld = Meld.ToList()};
				// sorthand
				tempPlayer.Hand.Add(target);
				tempPlayer.SortHand();
				Establish(FindProbablyEye(tempPlayer.Hand), tempPlayer.Hand);
			});
		}


		protected override List<Chess> FindProbablyEye(List<Chess> source)
		{
			List<Chess> result = new();
			for (int i = 0; i < source.Count; i++)
			{
				if (i != source.Count - 1 && source[i].Number == source[i + 1].Number && source[i].Color == source[i + 1].Color)
				{
					if (!CheckContains(result, source[i]))
					{
						result.Add(source[i]);
					}
				}
			}
			return result;
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


		protected override void Establish(List<Chess> probablyEye, List<Chess> hand)
		{
			foreach (Chess eye in probablyEye)
			{
				List<Chess> temp = hand.ToList();
				RemoveEye(eye, ref temp);

				// temp is empty
				while (!temp.Any())
				{
					Chess first = temp[0];
					Chess second= new(){Number= temp[0].Number+1, Color= temp[0].Color,};
					Chess third = new(){Number = temp[0].Number +2, Color = temp[0].Color,};
					if (CheckContains(temp, second) && CheckContains(temp, third))
					{
						temp.Remove(first);
						temp.Remove(second);
						temp.Remove(third);
						IsWin = true;
						continue;
					}
					else
					{
						IsWin = false;
					}
				}
			}
		}


		protected override void RemoveEye(Chess targetEye, ref List<Chess> orginal)
		{
			List<Chess> temp = orginal.ToList();
			for (int i = 0; i < 2; i++)
			{
				temp.RemoveAt(orginal.IndexOf(targetEye));
			}
			orginal = temp;
		}


		public override void Discard()
		{
			// TODO
		}


		/// <summary>
		/// Check Meld is in hand
		/// </summary>
		/// <param name="chess"> the previous one is discard to river </param>
		/// <returns> true if the HasMeld contains any probably meld; otherwise, false.</returns>
		public bool CheckMeld(Chess chess)
		{
			Task Task = Task.Factory.StartNew(()=>
			{
				Chess M2 = new(){Number = chess.Number-2,Color= chess.Color};
				Chess M1 = new(){Number = chess.Number-1,Color= chess.Color};
				Chess P1 = new(){Number = chess.Number+1, Color = chess.Color};
				Chess P2 = new(){Number = chess.Number+2,Color= chess.Color};

				// m2m1
				if (CheckContains(Hand, M2) && CheckContains(Hand, M1))
				{
					HasMeld.Add((M2, M1));
				}

				// m1p1
				if (CheckContains(Hand, M1) && CheckContains(Hand, P1))
				{
					HasMeld.Add((M1, P1));
				}

				// p1p2
				if (!CheckContains(Hand, P1) && CheckContains(Hand, P2))
				{
					HasMeld.Add((P1, P2));
				}

			});
			return HasMeld.Any();
		}


		/// <summary>
		/// Make Chi Meld to Meld List
		/// </summary>
		/// <param name="choice"> which one player select.</param>
		/// <param name="previousPlayer">the previous player.</param>
		public void MakeChiMeld(int choice, ref Player previousPlayer)
		{
			// Add List<Chess> to Meld List, it is include
			// HasMeld[choice - 1].Item1,
			// otherPlayer.River.Last and 
			// HasMeld[choice - 1].Item2
			Meld.Add(new()
			{
				HasMeld[choice - 1].Item1,
				previousPlayer.River.Last(),
				HasMeld[choice - 1].Item2
			});

			// remove pair from Hand
			Hand.Remove(HasMeld[choice - 1].Item1);
			Hand.Remove(HasMeld[(choice - 1)].Item2);
			// previousPlayer.River Remove Last one
			previousPlayer.River.RemoveAt(previousPlayer.River.Count - 1);

			// clear HasMeld
			HasMeld = new();
		}
	}
}
