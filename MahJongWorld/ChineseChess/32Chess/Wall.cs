using System.Linq;

using MahJongWorld.Interface;

namespace MahJongWorld.ChineseChessMahJong._32Chess
{
	public class Wall : ChineseChessMahJong.Wall, IWall
	{
		/// <summary>
		/// combine code and surface to Chess class and append it to wall list
		/// </summary>
		public void AppendToHand()
		{
			Hand = new();
			for (int i = 0; i < code.Length; i++)
			{
				int repeattime = (code[i][0] - 48) switch
				{
					1 => 1,
					2 or 3 or 4 or 5 or 6 => 2,
					7 => 5,
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
