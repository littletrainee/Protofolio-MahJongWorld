using System.Linq;

using MahJongWorld.Abstract;

namespace MahJongWorld.ChineseChessMahJong
{

	public class Wall : AbstractChessWall
	{
		private readonly string[] code = new string[]
		{
			"1b", "2b", "3b", "4b", "5b", "6b", "7b",
			"1r", "2r", "3r", "4r", "5r", "6r", "7r"
		};
		private readonly string[] surface = new string[]
		{
			"將", "士", "象", "車", "馬", "砲", "卒",
			"帥", "仕", "相", "俥", "傌", "炮", "兵",
		};




		/// <summary>
		/// combine code and surface to Chess class and append it to wall list
		/// </summary>
		public override void AppendToHand()
		{
			Hand = new();
			for (int i = 0; i < code.Length; i++)
			{
				int repeattime = (code[i][0] - 48) switch
				{
					1 => 1,
					2 or 3 or 4 or 5 or 6=>2,
					7 =>5,
					_ => 0,
				};
				foreach (int j in Enumerable.Range(0, repeattime))
				{
					Hand.Add(new()
					{
						Number = code[i][0] - 48,
						Color = code[i][1].ToString(),
						Surface = surface[i]
					});
				}
			}
		}
	}
}
