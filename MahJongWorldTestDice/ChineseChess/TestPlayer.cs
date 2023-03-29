﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MahJongWorld.ChineseChess.TestChineseChess
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
	}
}