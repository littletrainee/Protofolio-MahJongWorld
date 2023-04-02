using MahJongWorld.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MahJongWorld.ChineseChessMahJong._56Chess.TestChineseChess
{
	[TestClass()]
	public class TestScore
	{
		[TestMethod()]
		public void TestDifferentGeneralBeenPair()
		{
			Chess c1 = new(){Number =1,Color = "r",Surface="將"};
			Chess c2 = new(){Number =2,Color = "r",Surface="士"};
			Chess c3 = new(){Number =3,Color = "r",Surface= "象"};
			Chess c4 = new(){Number =4,Color = "b", Surface= "車"};
			Chess c5 = new(){Number =4, Color = "b",Surface= "車"};
			Player P = new(){Hand = new(){ c1,c2,c3,c4,c5} };
			GameState G = new();
			Score S = new();
			State ST = State.IsTsumo;
			S.Initilization(P, G, ST);
			S.DifferentGeneralBeenPair();
		}
	}
}