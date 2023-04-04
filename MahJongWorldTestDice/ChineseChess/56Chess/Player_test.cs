using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MahJongWorld.ChineseChessMahJong._56Chess.Tests
{
	[TestClass()]
	public class Player_test
	{
		[TestMethod()]
		public void TestTsumoCheck()
		{
			Chess c1 = new(){Number =5,Color = "b",Surface="馬"};
			Chess c2 = new(){Number =5,Color = "r",Surface="傌"};
			Chess c3 = new(){Number =6,Color = "r",Surface="炮"};
			Chess c4 = new(){Number =7,Color = "r",Surface="兵"};
			Chess c5 = new(){Number =2,Color = "r",Surface="仕"};
			Chess c6 = new(){Number =3,Color = "r",Surface="相"};
			Chess c7 = new(){Number =4,Color = "r",Surface="俥"};
			Chess c8 = new(){Number =5,Color = "b",Surface="馬"};

			Player P = new(){Hand = new(){c1,c2,c3,c4,c8 }, Meld= new(){new(){c5,c6,c7}} };
			//P.RonCheck(c8);
			//Player P = new (){Hand = new(){c1,c2,c3,c4,c5,c6,c7,c8}};
			P.TsumoCheck();
			Assert.IsTrue(P.IsWin);
		}


		[TestMethod()]
		public void TestMakeTriple()
		{
			Chess c1 = new(){Number =1,Color = "r",Surface="將"};
			Chess c2 = new(){Number =1,Color = "r",Surface="將"};
			Chess c3 = new(){Number =2,Color = "r",Surface="士"};
			Chess c4 = new(){Number =2,Color = "r",Surface="士"};
			Chess c5 = new(){Number =3,Color = "r",Surface="象"};
			Chess c6 = new(){Number =3,Color = "r",Surface="象"};
			Chess c7 = new(){Number =4,Color = "b",Surface="車"};
			Chess c8 = new(){Number =1,Color = "r",Surface="將"};
			Player p1 = new (){Hand = new(){c1,c2,c3,c4,c5,c6,c7},Meld = new()};
			Player p2 = new (){River = new(){c8}};
			p1.MakeTriple(ref p2);

		}


		[TestMethod()]
		public void TestMakeBigQuadruple()
		{
			Chess c1 = new(){Number =1,Color = "r",Surface="將"};
			Chess c2 = new(){Number =1,Color = "r",Surface="將"};
			Chess c3 = new(){Number =2,Color = "r",Surface="士"};
			Chess c4 = new(){Number =2,Color = "r",Surface="士"};
			Chess c5 = new(){Number =3,Color = "r",Surface="象"};
			Chess c6 = new(){Number =3,Color = "r",Surface="象"};
			Chess c7 = new(){Number =4,Color = "b",Surface="車"};
			Chess c8 = new(){Number =1,Color = "r",Surface="將"};
			Chess c9 = new(){Number =1,Color = "r",Surface="將"};
			Player p1 = new (){Hand = new(){c1,c2,c8,c3,c4,c5,c6},Meld = new()};
			Player p2 = new (){River = new(){c9}};
			Wall W = new(){Hand = new(){c7}};
			p1.MakeBigQuadruple(ref p2, ref W);
			Assert.IsTrue(p1.Meld.Count == 1);
		}


		[TestMethod()]
		public void TestCheckSmallOrConcealedQuadruple()
		{
			Chess c1 = new(){Number =1,Color = "r",Surface="將"};
			Chess c2 = new(){Number =1,Color = "r",Surface="將"};
			Chess c3 = new(){Number =1,Color = "r",Surface="將"};
			Chess c4 = new(){Number =2,Color = "r",Surface="士"};
			Chess c5 = new(){Number =3,Color = "r",Surface="象"};
			Chess c6 = new(){Number =3,Color = "r",Surface="象"};
			Chess c7 = new(){Number =4,Color = "b",Surface="車"};
			Chess c8 = new(){Number =1,Color = "r",Surface="將"};
			Player P= new(){Hand = new(){c1,c2,c3,c4,c5,c6,c7,c8}};
			P.CheckSmallOrConcealedQuadruple();

		}


		[TestMethod()]
		public void TestFindProbablyEye()
		{
			Chess c1 = new(){Number =1,Color = "r",Surface="將"};
			Chess c2 = new(){Number =1,Color = "r",Surface="將"};
			Chess c3 = new(){Number =2,Color = "r",Surface="士"};
			Chess c4 = new(){Number =2,Color = "r",Surface="士"};
			Chess c5 = new(){Number =3,Color = "r",Surface= "象"};
			Chess c6 = new(){Number =3,Color = "r",Surface= "象"};
			Chess c7 = new(){Number =4,Color = "b", Surface= "車"};
			Chess c8 = new(){Number =4, Color = "b",Surface= "車"};
			List<Chess> li = new(){c1,c2,c3,c4,c5,c6,c7,c8};
			Player P = new Player();
			Assert.IsTrue(P.FindProbablyEye(li).Count == 4);
		}


		[TestMethod()]
		public void TestCheckTripleAndBigQuadruple()
		{
			Chess c1 = new(){Number =1,Color = "r",Surface="將"};
			Chess c2 = new(){Number =1,Color = "r",Surface="將"};
			Chess c3 = new(){Number =1,Color = "r",Surface="將"};
			Chess c4 = new(){Number =2,Color = "r",Surface="士"};
			Chess c5 = new(){Number =3,Color = "r",Surface= "象"};
			Chess c6 = new(){Number =3,Color = "r",Surface= "象"};
			Chess c7 = new(){Number =4,Color = "b", Surface= "車"};
			Chess c8 = new(){Number =1,Color = "r",Surface="將"};

			Player P = new (){Hand = new(){c1,c2,c3,c4,c5,c6,c7}};
			P.CheckTripleAndBigQuadruple(c8);
			Task.WaitAll();
			Console.WriteLine(P.MeldState == Shared.MeldState.BigQuadruple);
		}


		[TestMethod()]
		public void TestMakeSmallQuadruple()
		{
			Chess c1 = new(){Number =1,Color = "r",Surface="將"};
			Chess c2 = new(){Number =1,Color = "r",Surface="將"};
			Chess c3 = new(){Number =1,Color = "r",Surface="將"};
			Chess c4 = new(){Number =2,Color = "r",Surface="士"};
			Chess c5 = new(){Number =3,Color = "r",Surface= "象"};
			Chess c6 = new(){Number =3,Color = "r",Surface= "象"};
			Chess c7 = new(){Number =4,Color = "b", Surface= "車"};
			Chess c8 = new(){Number =1,Color = "r",Surface="將"};
			Chess c9 = new(){Number =7,Color = "r",Surface="兵"};

			Player P = new(){Hand = new(){c4,c5,c6,c7,c8},Meld = new(){ new() { c1, c2, c3 } }};

			Wall W = new(){Hand = new(){c9}};
			P.MakeSmallQuadruple(ref W);

			Assert.IsTrue(P.Meld[0].Count == 4);

		}


		[TestMethod()]
		public void TestMakeConcealedQuadruple()
		{
			Chess c1 = new(){Number =1,Color = "r",Surface="將"};
			Chess c2 = new(){Number =1,Color = "r",Surface="將"};
			Chess c3 = new(){Number =1,Color = "r",Surface="將"};
			Chess c4 = new(){Number =2,Color = "r",Surface="士"};
			Chess c5 = new(){Number =3,Color = "r",Surface= "象"};
			Chess c6 = new(){Number =3,Color = "r",Surface= "象"};
			Chess c7 = new(){Number =4,Color = "b", Surface= "車"};
			Chess c8 = new(){Number =1,Color = "r",Surface="將"};
			Chess c9 = new(){Number =7,Color = "r",Surface="兵"};

			Player P = new(){Hand = new(){c1,c2,c3,c4,c5,c6,c7,c8}, Meld = new()};

			Wall W = new(){Hand = new(){c9}};
			P.MakeConcealedQuadruple(ref W);

			Assert.IsTrue(P.Meld[0].Count == 4);

		}


		[TestMethod()]
		public void TestCheckSequence()
		{
			Chess c1 = new(){Number =1,Color = "b",Surface="將"};
			Chess c2 = new(){Number =2,Color = "b",Surface="士"};
			Chess c3 = new(){Number =3,Color = "b",Surface="象"};
			Chess c4 = new(){Number =4,Color = "b",Surface="車"};
			Chess c5 = new(){Number =5,Color = "b",Surface="馬"};
			Chess c6 = new(){Number =6,Color = "b",Surface="包"};
			Chess c7 = new(){Number =7,Color = "b",Surface="卒"};
			Chess c8 = new(){Number =2,Color = "b",Surface="士"};

			Player P = new(){ Hand = new(){c1,c2,c3,c4,c5,c6,c7}, HasMeld = new()};
			P.CheckSequence(c8);
			Assert.IsTrue(P.HasMeld.Count == 2);

		}


		[TestMethod()]
		public void TestRonCheck()
		{
			Chess c1 = new(){Number =5,Color = "b",Surface="馬"};
			Chess c2 = new(){Number =5,Color = "r",Surface="傌"};
			Chess c3 = new(){Number =6,Color = "r",Surface="炮"};
			Chess c4 = new(){Number =7,Color = "r",Surface="兵"};
			Chess c5 = new(){Number =2,Color = "r",Surface="仕"};
			Chess c6 = new(){Number =3,Color = "r",Surface="相"};
			Chess c7 = new(){Number =4,Color = "r",Surface="俥"};
			Chess c8 = new(){Number =5,Color = "b",Surface="馬"};

			Player P = new(){Hand = new(){c1,c2,c3,c4 }, Meld= new(){new(){c5,c6,c7}} };
			P.RonCheck(c8);
			Assert.IsTrue(P.IsWin);

		}
	}
}