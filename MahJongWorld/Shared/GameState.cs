using System;
using System.Collections.Generic;

namespace MahJongWorld.Shared
{
	public class GameState<T>
	{
		private string FirstPlayer { get; set; }

		public bool GameOn { get; set; }

		public int GameTurn { get; set; }


		public int GameRound { get; set; }

		public bool LastOne { get; set; }




		/// <summary>
		/// Set GameOn ,GameTurn and MaxPlayer.
		/// </summary>
		/// <param name="maxPlayer"></param>
		public void Initialization()
		{
			GameOn = true;
			GameTurn = 0;
		}

		public void SetFirstPlayerName(string name)
		{
			FirstPlayer = name;
		}



		/// <summary>
		/// Go To Next Round
		/// </summary>
		/// <param name="name"></param>
		public void NextRound(string name)
		{
			if (name == FirstPlayer)
			{
				GameRound++;
			}
		}


		/// <summary>
		/// Turn To Next Player
		/// </summary>
		public void TurnNext(ref List<T> Players, int MaxPlayer)
		{
			if (GameTurn < MaxPlayer - 1)
			{
				GameTurn++;
			}
			else
			{
				GameTurn = 0;
			}
			Players.Add(Players[0]);
			Players.RemoveAt(0);
		}


		public void PrintToConsole()
		{
			Console.WriteLine($"第 {GameRound} 巡");
		}
	}
}
