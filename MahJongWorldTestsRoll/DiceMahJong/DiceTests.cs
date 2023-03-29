using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MahJongWorld.DiceMahJong.Tests
{
    [TestClass()]
    public class DiceTests
    {
        [TestMethod()]
        public void RollTest()
        {
            Dice d = new();
            Random rnd = new ();
            d.Roll(rnd);
            if (d.Number == 0)
            {
                Assert.Fail();
            }
        }
    }
}