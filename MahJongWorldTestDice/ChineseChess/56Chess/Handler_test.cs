using MahJongWorld.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MahJongWorld.ChineseChessMahJong._56Chess.Tests
{
	[TestClass()]
	public class TestHandler
	{
		[TestMethod()]
		public void TestAskMakeMeldAndChoose()
		{
			//Handler H = new();
			Player P = new(){MeldState= MeldState.Triple, HasMeld = new()};
			Handler.AskMakeMeld(P);

		}
	}
}