using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MahJongWorld.Abstract;

namespace MahJongWorld.DiceMahJong
{


	/// <summary>
	/// Player Object With Name, Hand And River
	/// </summary>
	public class Player : AbstractPlayer<Dice>
	{
		public Dice River { get; set; }




		/// <summary>
		/// Instantiate List of Hand and River, And Assignment Name from s
		/// </summary>
		/// <param name="s"> Set the parameter to "Name". </param>
		public void Initialization(string s)
		{
			Name = s;
			Hand = new();
			River = new();
		}


		/// <summary>
		/// Print Hand and River to Console
		/// </summary>
		public override void PrintToConsole()
		{
			Console.Write($"{Name}'s Hand: ");
			foreach (Dice d in Hand)
			{
				if (d.Number == 1 || d.Number == 4)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write(d.Number);
					Console.ResetColor();
				}
				else
				{
					Console.Write(d.Number);
				}
				if (d != Hand.Last())
				{
					Console.Write(", ");
				}
				if (d == Hand.Last())
				{
					Console.WriteLine();
				}
			}
			Console.Write($"{Name}'s River: ");
			if (River.Number is not 0)
			{
				if (River.Number == 1 || River.Number == 4)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine($"{River.Number}\n");
					Console.ResetColor();
				}
				else
				{
					Console.WriteLine($"{River.Number}\n");
				}
			}
			else
			{
				Console.WriteLine("\n");
			}
		}


		public override void SortHand()
		{
			List<Dice> newtemp = new();
			for (int i = 1; i < 7; i++)
			{
				foreach (Dice dice in Hand)
				{
					if (dice.Number == i)
					{
						newtemp.Add(dice);
					}
				}
			}
			Hand = newtemp;
		}


		public override void TsumoCheck()
		{
			// Clone player
			Player tempPlayer = new(){Hand = Hand.ToList() };
			// sorthand
			tempPlayer.SortHand();
			Establish(FindProbablyEye(tempPlayer.Hand), tempPlayer.Hand);
		}


		public override void RonCheck(Dice target)
		{
			Task Task = Task.Factory.StartNew(()=>
			{
				Player tempPlayer = new(){Hand= Hand.ToList()};
				tempPlayer.Hand.Add(target);
				tempPlayer.SortHand();
				Establish(FindProbablyEye(tempPlayer.Hand), tempPlayer.Hand);
			});
		}


		public override List<Dice> FindProbablyEye(List<Dice> source)
		{
			List<Dice> result = new();
			for (int i = 0; i < source.Count; i++)
			{
				if (i != source.Count - 1 && source[i].Number == source[i + 1].Number)
				{
					if (!CheckContains(result, source[i]))
					{
						result.Add(source[i]);
					}
				}
			}
			return result;
		}


		protected override bool CheckContains(List<Dice> source, Dice target)
		{
			foreach (Dice d in source)
			{
				if (d.Number == target.Number)
				{
					return true;
				}
			}
			return false;
		}


		protected override void Establish(List<Dice> probablyEye, List<Dice> hand)
		{
			foreach (Dice eye in probablyEye)
			{
				List<Dice> temp = hand.ToList();
				RemoveEye(eye, ref temp);
				if (temp[0].Number == temp[1].Number && temp[1].Number == temp[2].Number
					|| temp[0].Number + 1 == temp[1].Number && temp[1].Number + 1 == temp[2].Number)
				{
					IsWin = true;
				}
			}
			IsWin = false;
		}


		protected override void RemoveEye(Dice targetEye, ref List<Dice> orginal)
		{
			List<Dice> temp = orginal.ToList();
			for (int i = 0; i < 2; i++)
			{
				temp.RemoveAt(orginal.IndexOf(targetEye));
			}
			orginal = temp;
		}


		public override void Discard()
		{
			int keyint;
			bool ok;
			Console.Write("Please select whitch one do you want to discard from index 1-5:");
			while (true)
			{
				string keystr = Console.ReadLine();
				ok = int.TryParse(keystr, out keyint);
				if (!ok || keyint > 5 || keyint < 1)
				{
					Console.Write("Wrong Enter Please Renter:");
					continue;
				}
				break;
			}
			River = Hand[keyint - 1];
			Hand.RemoveAt(keyint - 1);
		}


		/// <summary>
		/// Get Dice From Previous Player
		/// </summary>
		/// <param name="previsousPlayerRiverDice"></param>
		public void GetDice(ref Dice previsousPlayerRiverDice)
		{
			Hand.Add(previsousPlayerRiverDice);
			previsousPlayerRiverDice = null;
		}
	}
}
