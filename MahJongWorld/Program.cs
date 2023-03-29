using MahJongWorld.DiceMahJong;

namespace MahJongWorld
{
	public class Program
	{
		public static void Main()
		{
			Handler handler = new();
			handler.Start();
			handler.Update();
		}
	}
}
