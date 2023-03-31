using MahJongWorld.Abstract;

namespace MahJongWorld.ChineseChessMahJong
{

	public class Wall : AbstractChessWall
	{
		/// <summary>
		/// the readonly string array for check code
		/// </summary>
		protected readonly string[] code = new string[]
		{
			"1b", "2b", "3b", "4b", "5b", "6b", "7b",
			"1r", "2r", "3r", "4r", "5r", "6r", "7r"
		};
		/// <summary>
		/// the readonly string array for surface mark
		/// </summary>
		protected readonly string[] surface = new string[]
		{
			"將", "士", "象", "車", "馬", "砲", "卒",
			"帥", "仕", "相", "俥", "傌", "炮", "兵",
		};
	}
}
