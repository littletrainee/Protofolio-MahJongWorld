namespace MahJongWorld.Shared
{
	public class GameState
	{
		public bool GameOn { get; set; }
		public int GameTurn { get; set; }
		public int MaxPlayer { get; set; }
		public int GameRound { get; set; }



		/// <summary>
		/// Set GameOn ,GameTurn and MaxPlayer.
		/// </summary>
		/// <param name="maxPlayer"></param>
		public void Initialization(int maxPlayer)
		{
			GameOn = true;
			GameTurn = 0;
			MaxPlayer = maxPlayer;
		}


		/// <summary>
		/// Go To Next Round
		/// </summary>
		/// <param name="name"></param>
		public void NextRound(string name)
		{
			if (name == "Player 1")
			{
				GameRound++;
			}
		}


		/// <summary>
		/// Turn To Next Player
		/// </summary>
		public void TurnNext()
		{
			if (GameTurn < MaxPlayer - 1)
			{
				GameTurn++;
			}
			else
			{
				GameTurn = 0;
			}
		}
	}
}
