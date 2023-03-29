using System;
using System.Collections.Generic;
using System.Linq;

using MahJongWorld.ChineseChessMahJong;

namespace MahJongWorld.Abstract
{
	/// <summary>
	/// Abstract class ChessWall must inherit and override
	/// </summary>
	public abstract class AbstractChessWall
	{
		public string Name { get; set; }
		public List<Chess> Hand { get; set; }




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
					Console.WriteLine("\n");
				}
			}
		}
	}
}
