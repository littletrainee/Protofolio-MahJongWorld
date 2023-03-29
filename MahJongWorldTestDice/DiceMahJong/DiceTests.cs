using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MahJongWorld.DiceMahJong.TestsDice
{
    [TestClass()]
    public class DiceTests
    {
        [TestMethod()]
        public void RollTest()
        {
            Dice d1 = new();
            Dice d2 = new();
            Dice d3 = new();
            Dice d4 = new();
            Dice d5 = new();

            Random rnd = new();
            d1.Roll(rnd);
            d2.Roll(rnd);
            d3.Roll(rnd);
            d4.Roll(rnd);
            d5.Roll(rnd);
        }
    }
}