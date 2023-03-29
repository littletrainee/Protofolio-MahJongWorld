using System;

namespace MahJongWorld.DiceMahJong
{
	public class Dice
	{
		public int Number { get; set; }


		/// <summary>
		/// Roll this Dice
		/// </summary>
		/// <param name="random"></param>
		public void Roll(Random random)
		{
			Number = random.Next(1, 7);
		}
	}
}
