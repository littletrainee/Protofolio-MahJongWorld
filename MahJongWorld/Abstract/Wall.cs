using System;
using System.Collections.Generic;
using System.Linq;

using MahJongWorld.ChineseChess;

namespace MahJongWorld.Abstract
{
	/// <summary>
	/// Abstract class ChessWall must inherit and override
	/// </summary>
	public abstract class AbstractChessWall
	{
		public string Name { get; set; }
		public List<Chess> Hand { get; set; }
		protected readonly string[] code = new string[]
		{
			"1b", "2b", "3b", "4b", "5b", "6b", "7b",
			"1r", "2r", "3r", "4r", "5r", "6r", "7r"
		};
		protected readonly string[] surface = new string[]
		{
			"將", "士", "象", "車", "馬", "砲", "卒",
			"帥", "仕", "相", "俥", "傌", "炮", "兵",
		};






		/// <summary>
		/// Append Chess To Wall
		/// </summary>
		public abstract void AppendToHand();


		/// <summary>
		/// Shuffle Wall
		/// </summary>
		public void Shuffle()
		{
			for (int i = 0; i < Hand.Count; i++)
			{
				int k = new Random().Next(i+1);
				(Hand[i], Hand[k]) = (Hand[k], Hand[i]);
			}
		}


		/// <summary>
		/// Print Wall to Console
		/// </summary>
		public void PrintToConsole()
		{
			Console.Write($"{Name}: ");
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
		}
	}
}
