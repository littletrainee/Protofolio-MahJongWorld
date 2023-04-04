using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MahJongWorld.ChineseChessMahJong._56Chess.TestChineseChess
{
	[TestClass()]
	public class TestHandler
	{
		[TestMethod()]
		public void TestCheckRon()
		{
			Player p1 = new()
			{
				Name = "p1",
				Hand = new()
				{
					new(){Number =1,Color = "r",Surface= "帥"},
					new(){Number =2,Color = "r",Surface= "仕"},
					new(){Number =3,Color = "r",Surface= "相"},
					new(){Number =4,Color = "r",Surface= "俥"}
				},
				Meld = new(),
				River = new()
				{
					new(){Number =4,Color= "b",Surface="車"}
				},
				HasMeld = new(),
			};
			Player p2 = new()
			{
				Name = "p2",
				Hand = new()
				{
					new(){Number =1,Color = "b",Surface="將"},
					new(){Number =2,Color = "b",Surface="士"},
					new(){Number =3,Color = "b",Surface= "象"},
					new(){Number =4,Color = "b",Surface= "車"}

				},
				Meld = new(),
				River = new(),
				HasMeld = new(),
			};
			Handler H = new()
			{
				Players =new()
				{
					p1,p2
				}
			};
			H.CheckRon();
			Console.WriteLine(H.Players[0].IsWin);
			Console.WriteLine(H.Players[1].IsWin);
		}
	}
}