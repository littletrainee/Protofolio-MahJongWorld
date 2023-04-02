using System;

using MahJongWorld.Abstract;
using MahJongWorld.Shared;

namespace MahJongWorld.ChineseChessMahJong._56Chess
{
	public class Score : AbstractScore<Chess>
	{
		protected GameState<Player> GameState { get; set; }
		/// <summary>
		/// initilization this Player, GameState and State 
		/// </summary>
		/// <param name="player"></param>
		/// <param name="gameState"></param>
		/// <param name="state"></param>
		public void Initilization(Player player, GameState<Player> gameState, State state)
		{
			Hand = player.Hand;
			GameState = gameState;
			WinBy = state;
		}


		/// <summary>
		/// 天胡
		/// </summary>
		private void TenHou()
		{
			if (GameState.GameRound == 0 && GameState.GameTurn == 0)
			{
				Total += 6;
				Console.WriteLine("天胡        6台");
			}
		}


		/// <summary>
		/// 五兵合縱、五卒連橫 
		/// </summary>
		private void FiveSolider()
		{
			if (Five(new() { Number = 7, Color = "b" }))
			{
				Total += 5;
				Console.WriteLine("五卒連橫    5台");
			}
			if (Five(new() { Number = 7, Color = "r" }))
			{

				Total += 5;
				Console.WriteLine("五兵合縱    5台");
			}
		}


		/// <summary>
		/// check hand is all equal to compare
		/// </summary>
		/// <param name="compare"> compare by <paramref name="compare"/></param>
		/// <returns>true is all same;or false</returns>
		private bool Five(Chess compare)
		{
			foreach (Chess chess in Hand)
			{
				if (chess.Number != compare.Number || chess.Color != compare.Color)
				{
					return false;
				}
			}
			return true;
		}


		/// <summary>
		/// 將帥聽令
		/// </summary>
		public void DifferentGeneralBeenPair()
		{
			if (CheckContains(new() { Number = 1, Color = "b" }) && CheckContains(new() { Number = 1, Color = "r" }))
			{
				Total += 2;
				Console.WriteLine("將帥聽令    2台");
			}

		}


		/// <summary>
		/// check hand is contain <paramref name="compare"/>
		/// </summary>
		/// <param name="compare"></param>
		/// <returns>true is contains <paramref name="compare"/>;or false</returns>
		private bool CheckContains(Chess compare)
		{
			foreach (Chess chess in Hand)
			{
				if (chess.Number == compare.Number && chess.Color == compare.Color)
				{
					return true;
				}
			}
			return false;
		}


		/// <summary>
		/// Tsumo Or Ron this game
		/// </summary>
		private void TsumoOrRon()
		{
			if (WinBy == State.IsTsumo)
			{
				Total += 2;
				Console.WriteLine("自摸        2台");
			}
			else
			{
				Total += 1;
				Console.WriteLine("胡牌        1台");
			}
		}


		/// <summary>
		/// All color is Same
		/// </summary>
		private void SameColor()
		{
			if (Color("b") || Color("r"))
			{
				Total += 1;
				Console.WriteLine("清一色      1台");
			}
		}


		/// <summary>
		///  check all color is same 
		/// </summary>
		/// <param name="target"></param>
		/// <returns>true is all same color; ortherwise false</returns>
		private bool Color(string target)
		{
			foreach (Chess chess in Hand)
			{
				if (chess.Color != target)
				{
					return false;
				}
			}
			return true;
		}


		/// <summary>
		/// check is win on the last one from wall
		/// </summary>
		private void WinOnLastOne()
		{
			if (GameState.LastOne)
			{
				Total += 1;
				Console.WriteLine("海底撈月    1台");
			}
		}


		/// <summary>
		/// print patterns to console
		/// </summary>
		public void PrintPatterns()
		{
			TenHou();
			WinOnLastOne();
			TsumoOrRon();
			SameColor();
			DifferentGeneralBeenPair();
			FiveSolider();
			Console.WriteLine("===============");
			Console.WriteLine($"共：        {Total}台\n");
		}
	}
}
