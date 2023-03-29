using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MahJongWorld.DiceMahJong.TestsDice
{
	[TestClass()]
	public class PlayerTests
	{
		[TestMethod()]
		public void PrintToConsoleTestPrintToConsole()
		{
			Player p = new();
			Dice d1 = new() { Number = 1 };
			Dice d2 = new() { Number = 2 };
			Dice d3 = new() { Number = 3 };
			Dice d4 = new() { Number = 4 };
			Dice d5 = new() { Number = 5 };
			p.Name = "John";
			p.Hand = new() { d1, d2, d3, d4 };
			p.River = d5;
			p.PrintToConsole();

		}

		[TestMethod()]
		public void SortHandTest()
		{
			Player p = new();
			Dice d1 = new() { Number = 1 };
			Dice d2 = new() { Number = 2 };
			Dice d3 = new() { Number = 3 };
			Dice d4 = new() { Number = 4 };
			Dice d5 = new() { Number = 5 };
			p.Hand = new List<Dice>() { d1, d3, d2, d4, d5 };
			p.Hand.ForEach(x => { Console.Write($"{x.Number}, "); });
			Console.WriteLine();
			Player  tempp= p;
			//Player.SortHand(ref tempp);
			p.Hand.ForEach(x => { Console.Write($"{x.Number}, "); });

			Console.WriteLine(p.Hand);
		}

		[TestMethod()]
		public void TsumoCheck()
		{
			Player p = new();
			Dice d1 = new() { Number = 2 };
			Dice d2 = new() { Number = 3 };
			Dice d3 = new() { Number = 4 };
			Dice d4 = new() { Number = 5 };
			Dice d5 = new() { Number = 5 };
			p.Hand = new List<Dice>() { d1, d3, d2, d4, d5 };
			p.TsumoCheck();
			Console.Write(p.IsWin);

		}
	}
}