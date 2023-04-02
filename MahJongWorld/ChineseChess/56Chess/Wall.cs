using System.Linq;

using MahJongWorld.Interface;

namespace MahJongWorld.ChineseChessMahJong._56Chess
{
	public class Wall : ChineseChessMahJong.Wall, IWall
	{
		public void AppendToHand()
		{
			Hand = new();
			for (int i = 0; i < code.Length; i++)
			{
				foreach (int j in Enumerable.Range(0, 4))
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
