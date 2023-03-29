using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MahJongWorld.ChineseChessMahJong.TestChineseChess
{
	[TestClass()]
	public class TestPlayer
	{
		[TestMethod()]
		public void TestPrintToConsole()
		{
			Chess c1 = new(){Number =1,Color = "r",Surface="將"};
			Chess c2 = new(){Number =2,Color = "r",Surface="士"};
			Chess c3 = new(){Number =3,Color = "r",Surface= "象"};
			Chess c4 = new(){Number =4,Color = "b", Surface= "車"};
			Chess c5 = new(){Number =4, Color = "b",Surface= "車"};
			List<Chess> li = new(){c4,c5};

			Player p = new()
			{
				Name = "Logan",
				Hand = li,
				Meld= new(){new(){c1,c2,c3} },
				River= new()
			};
			p.PrintToConsole();
		}

		[TestMethod()]
		public void TestFindProbablyEye()
		{
			Chess c1 = new(){Number =1,Color = "b",Surface="將"};
			Chess c2 = new(){Number =1,Color = "r",Surface="帥"};
			Chess c3 = new(){Number =2,Color = "b",Surface= "象"};
			Chess c4 = new(){Number =3,Color = "b", Surface= "車"};
			Chess c5 = new(){Number =4, Color = "b",Surface= "馬"};
			List<Chess> li = new() { c1,c2,c3,c4,c5};
			Player p = new();
			p.FindProbablyEye(li);
		}

		[TestMethod()]
		public void TestCheckMeld()
		{

			Chess c1 = new(){Number =4,Color = "b",Surface="車"};
			Chess c2 = new(){Number =5,Color = "b",Surface="馬"};
			Chess c3 = new(){Number =6,Color = "b",Surface= "包"};
			Chess c4 = new(){Number =7,Color = "b", Surface= "卒"};

			Chess T = new(){Number =6,Color= "b",Surface="包"};
			Player p  = new()
			{
				Hand = new(){ c1,c2,c3,c4},
				HasMeld = new()
			};
			p.CheckMeld(T);
		}

		[TestMethod()]
		public void TestRonCheck()
		{
			Chess c1 = new(){Number =1,Color = "b",Surface="將"};
			Chess c2 = new(){Number =2,Color = "b",Surface="士"};
			Chess c3 = new(){Number =3,Color = "b",Surface= "象"};
			Chess c4 = new(){Number =4,Color = "b",Surface= "車"};

			Chess T = new(){Number =4,Color= "b",Surface="車"};
			Player p  = new()
			{
				Name = "lo",
				Hand = new(){ c1,c2,c3,c4},
				Meld = new(),
				HasMeld = new()
			};
			p.RonCheck(T);
			Task.WaitAll();
		}

		[TestMethod()]
		public void TestTsumoCheck()
		{
			Chess c1 = new(){Number =7,Color = "b",Surface= "卒"};
			Chess c2 = new(){Number =7,Color = "b",Surface= "卒"};
			Chess c3 = new(){Number =7,Color = "b",Surface= "卒"};
			Chess c4 = new(){Number =7,Color = "r",Surface= "兵"};
			Chess c5 = new(){Number =7,Color = "r",Surface= "兵"};

			Player p = new()
			{
				Hand = new(){c1,c2,c3,c4,c5},
				Meld = new(),
				HasMeld = new(),
			};
			p.TsumoCheck();
			Assert.IsTrue(p.IsWin);

		}
	}
}