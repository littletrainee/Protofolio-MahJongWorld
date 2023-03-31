using System;

using MahJongWorld.Abstract;
using MahJongWorld.Shared;

namespace MahJongWorld.ChineseChessMahJong._32Chess
{
	public class Score : AbstractScore<Chess>
	{
		public void Initilization(Player player, GameState gameState, State state)
		{
			Hand = player.Hand;
			GameState = gameState;
			WinBy = state;
		}


		private void TenHou()
		{
			if (GameState.GameRound == 0 && GameState.GameTurn == 0)
			{
				Total += 6;
				Console.WriteLine("天胡        6台");
			}
		}


		private void FiveSolider()
		{
			if (Five(new() { Number = 7, Color = "b" }) ||
				Five(new() { Number = 7, Color = "r" }))
			{
				Total += 5;
				Console.WriteLine("五卒連橫    5台");
			}
		}


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


		public void DifferentGeneralBeenPair()
		{
			if (CheckContains(new() { Number = 1, Color = "b" }) && CheckContains(new() { Number = 1, Color = "r" }))
			{
				Total += 2;
				Console.WriteLine("將帥聽令    2台");
			}

		}

		public int GetTotal()
		{
			return Total;
		}

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


		private void SameColor()
		{
			if (Color("b") || Color("r"))
			{
				Total += 1;
				Console.WriteLine("清一色      1台");
			}
		}


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


		private void WinOnLastOne()
		{
			if (GameState.LastOne)
			{
				Total += 1;
				Console.WriteLine("海底撈月    1台");
			}
		}


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
