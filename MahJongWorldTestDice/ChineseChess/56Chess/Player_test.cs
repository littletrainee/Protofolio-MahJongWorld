using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MahJongWorld.ChineseChessMahJong._56Chess.Tests
{
	[TestClass()]
	public class TestPlayer
	{
		[TestMethod()]
		public void TestTsumoCheck()
		{
			Chess c1 = new(){Number =1,Color = "r",Surface="將"};
			Chess c2 = new(){Number =1,Color = "r",Surface="將"};
			Chess c3 = new(){Number =2,Color = "r",Surface="士"};
			Chess c4 = new(){Number =2,Color = "r",Surface="士"};
			Chess c5 = new(){Number =3,Color = "r",Surface= "象"};
			Chess c6 = new(){Number =3,Color = "r",Surface= "象"};
			Chess c7 = new(){Number =4,Color = "b", Surface= "車"};
			Chess c8 = new(){Number =4, Color = "b",Surface= "車"};
			Player P = new (){Hand = new(){c1,c2,c3,c4,c5,c6,c7,c8}};
			P.TsumoCheck();
			Assert.IsTrue(P.IsWin);
		}
	}
}