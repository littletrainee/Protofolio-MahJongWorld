namespace MahJongWorld.Shared
{
	public class BookMaker
	{
		public int MaxPlayer { get; set; }

		public int Who { get; set; }

		public int ContinueToBookMaker { get; set; }

		public int Winner { get; set; }

		public int WinbyWho { get; set; }




		public void Initialization(int maxPlayer)
		{
			MaxPlayer = maxPlayer;
			Who = 0;
			ContinueToBookMaker = 0;
		}


		public void ContinueOrNext()
		{
			// bookmaker is winner
			if (Who == Winner)
			{
				ContinueToBookMaker++;
				return;
			}

			// winner is last player
			if (MaxPlayer - 1 == Winner)
			{
				Who = 0;
			}
			else
			{
				Who++;
			}
		}

	}
}
