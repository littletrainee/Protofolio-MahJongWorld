using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MahJongWorld.Abstract;
using MahJongWorld.Interface;

namespace MahJongWorld.ChineseChessMahJong
{
	public class Player : AbstractPlayer<Chess>, IPlayer<Wall, Chess>
	{
		public List<List<Chess>> Meld { get; set; }
		public List<Chess> River { get; set; }


		public void Draw(ref Wall wall)
		{
			Hand.Add(wall.Hand[0]);
			wall.Hand.RemoveAt(0);
		}


		public void SortHand()
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


		public void PrintToConsole()
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


		public void TsumoCheck()
		{
			// Clone player
			Player tempPlayer = new(){Hand = Hand.ToList(),Meld = Meld.ToList()};
			// sorthand
			tempPlayer.SortHand();
			IsWin = Establish(FindProbablyEye(tempPlayer.Hand), tempPlayer.Hand);
		}


		public void RonCheck(Chess target)
		{
			IsWin = Task.Factory.StartNew(() =>
			{
				// Clone player
				Player tempPlayer = new(){Hand = Hand.ToList(),Meld = Meld.ToList()};
				// sorthand
				tempPlayer.Hand.Add(target);
				tempPlayer.SortHand();
				return Establish(FindProbablyEye(tempPlayer.Hand), tempPlayer.Hand);
			}).Result;
		}


		public List<Chess> FindProbablyEye(List<Chess> source)
		{
			(Chess,Chess) special = (
				new(){Number= 1, Color= "b",Surface="將"},
				new() {Number =1 , Color = "r", Surface ="帥"});
			if (CheckContains(source, special.Item1) && CheckContains(source, special.Item2))
			{
				return new() { special.Item1, special.Item1 };
			}

			List<Chess> result = new();
			for (int i = 0; i < source.Count; i++)
			{
				if (i != source.Count - 1 &&
					source[i].Number == source[i + 1].Number &&
					source[i].Color == source[i + 1].Color)
				{
					if (!CheckContains(result, source[i]))
					{
						result.Add(source[i]);
					}
				}
			}
			return result;
		}


		public void ManualDiscard()
		{
			int keyint;
			Console.Write($"Please select whitch one do you want to discard from index 1-{Hand.Count}:");
			while (true)
			{
				string keystring = Console.ReadLine();
				Console.WriteLine($"the select is: {keystring}");
				bool err = int.TryParse(keystring,out keyint);
				if (!err || keyint > Hand.Count || keyint < 1)
				{
					Console.Write("Wrong Enter Please Renter:");
				}
				else
				{
					break;
				}
			}
			River.Add(Hand[keyint - 1]);
			Hand.RemoveAt(keyint - 1);
		}


		protected override bool CheckContains(List<Chess> targetlist, Chess target)
		{
			throw new NotImplementedException();
		}


		protected override bool Establish(List<Chess> probablyEye, List<Chess> hand)
		{
			throw new NotImplementedException();
		}


		protected override void RemoveEye(Chess targetEye, ref List<Chess> orginal)
		{
			throw new NotImplementedException();
		}
	}
}
