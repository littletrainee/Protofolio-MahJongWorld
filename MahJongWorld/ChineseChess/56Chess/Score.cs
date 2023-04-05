using System;
using System.Collections.Generic;
using System.Linq;

using MahJongWorld.Abstract;
using MahJongWorld.Shared;

namespace MahJongWorld.ChineseChessMahJong._56Chess
{
	public class Score : AbstractScore<Chess>
	{
		private List<List<Chess>> Meld { get; set; }

		private List<Chess> List { get; set; }

		private List<Chess> Eye { get; set; }

		private GameState<Player> GameState { get; set; }

		private bool Concealed { get; set; }

		private bool TenPai { get; set; }

		private bool Bookmaker { get; set; }

		private int ContinueToBookmaker { get; set; }

		private int TwoKang { get; set; }

		private bool WinOnTheWallTail { get; set; }


		public (int, int, int) Win { get; set; }



		/// <summary>
		/// initilization this Player, GameState and State 
		/// </summary>
		/// <param name="player"></param>
		/// <param name="gameState"></param>
		/// <param name="state"></param>
		public void Initilization(Player player, GameState<Player> gameState, State state, BookMaker bookmaker)
		{
			Hand = player.Hand;
			Meld = player.Meld;
			GameState = gameState;
			SetList();
			Eye = player.Eye;
			WinBy = state;
			Concealed = player.Concealed;
			TenPai = player.TenPai;
			Bookmaker = bookmaker.Who == player.Code;
			ContinueToBookmaker = bookmaker.ContinueToBookMaker;
			TwoKang = player.TwoKang;
		}

		public void SetWin(int winner, int bywho, int bookmaker)
		{
			Win = new(winner, bywho, bookmaker);
		}


		private void SetList()
		{
			List = Hand.ToList();
			Meld.ForEach(meld => List.AddRange(meld));
		}


		/// <summary>
		/// 天胡
		/// </summary>
		private void TenHou()
		{
			if (GameState.GameRound == 0 && GameState.GameTurn == 0)
			{
				Total += 8;
				Console.WriteLine("天胡        8台");
			}
		}


		/// <summary>
		/// 將帥領兵、斷頭尾
		/// </summary>
		private void OnlyOrNoGeneralAndSorider()
		{
			for (int i = 0; i < List.Count; i++)
			{
				if (List[i].Number is not 1 or 7)
				{
					break;
				}
				if (i == List.Count - 1)
				{
					Total += 2;
					Console.WriteLine("將帥領兵    2台");
				}
			}
			for (int i = 0; i < List.Count; i++)
			{
				if (List[i].Number is not 1 or 7)
				{
					if (i == List.Count - 1)
					{
						Total += 1;
						Console.WriteLine("斷頭尾      1台");
					}
					else
					{
						continue;
					}
				}
				else
				{
					break;
				}
			}
		}


		private void IsContinueToBookmaker()
		{
			if (Bookmaker && ContinueToBookmaker > 0)
			{
				Total += ContinueToBookmaker * 2;
				Console.WriteLine($"連{ContinueToBookmaker}拉{ContinueToBookmaker}      {ContinueToBookmaker * 2}台");
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


		public void FourPair()
		{
			if (Concealed)
			{
				List<Chess> temp  = List.Distinct().ToList();
				if (temp.Count == 4)
				{
					Total += 1;
					Console.Write("四對子      1台");
				}
			}
		}


		private void IsBookmaker()
		{
			if (Bookmaker)
			{
				Total += 1;
				Console.WriteLine("莊家        1台");
			}
		}


		private void IsTenPai()
		{
			if (TenPai)
			{
				Total += 1;
				Console.WriteLine("聽牌        1台");
			}
		}


		private void OneDragon()
		{
			// check color 
			string Color = List[0].Color;
			foreach (Chess chess in List)
			{
				if (!chess.Color.Equals(Color))
				{
					return;
				}
			}
			int[] compare = new int[]{1,2,3,4,5,6,7,7};
			List<Chess> temp = Sort().ToList();
			for (int i = 0; i < compare.Length; i++)
			{
				if (temp[i].Number != compare[i])
				{
					break;
				}
				if (i == compare.Length - 1)
				{
					Total += 4;
					Console.WriteLine("一條龍      4台");
				}
			}
		}


		private List<Chess> Sort()
		{
			List<Chess> temp = new();
			for (int i = 0; i < List.Count; i++)
			{
				foreach (Chess chess in List)
				{
					if (chess.Number == i)
					{
						temp.Add(chess);
					}
				}
			}
			return temp;
		}


		private void TwoDragonHug()
		{
			if (Concealed && Eye.Count == 1)
			{
				List<Chess> temp = Sort().ToList();
				for (int i = 0; i < 2; i++)
				{
					temp.RemoveAt(Player.FindIndex(temp, Eye[0]));
				}
				for (int i = 0; i < 6; i += 2)
				{
					if (!Player.ChessIsEqual(temp[i], temp[i]) || temp[i].Number != temp[i + 2].Number)
					{
						break;
					}

					if (i == temp.Count - 2)
					{
						Total += 4;
						Console.WriteLine("雙龍抱      4台");
					}
				}
			}

		}


		/// <summary>
		/// check is win on the last one from wall
		/// </summary>
		private void WinOnLastOne()
		{
			if (GameState.LastOne)
			{
				if (WinBy == State.IsTsumo)
				{
					Total += 3;
					Console.WriteLine("海底撈月    3台");
				}
				else
				{
					Total += 1;
					Console.WriteLine("河底撈魚    1台");
				}
			}
		}


		private void ConcealedAndTsumo()
		{
			if (Concealed && WinBy == State.IsTsumo)
			{
				Total += 3;
				Console.WriteLine("門清自摸    3台");
			}
			if (Concealed && WinBy == State.IsRon)
			{
				Total += 1;
				Console.WriteLine("門清        1台");
				Total += 1;
				Console.WriteLine("胡牌        1台");
			}
			if (!Concealed && WinBy == State.IsTsumo)
			{
				Total += 1;
				Console.WriteLine("自摸        1台");
			}
			if (!Concealed && WinBy == State.IsRon)
			{
				Total += 1;
				Console.WriteLine("胡牌        1台");
			}
		}


		private void AllPaired()
		{
			if (Eye.Count == 1)
			{
				List<Chess> temp = Sort().ToList();
				for (int i = 0; i < 2; i++)
				{
					temp.RemoveAt(Player.FindIndex(temp, Eye[0]));
				}
				for (int i = 0; i < 2; i++)
				{
					if (Player.ChessIsEqual(temp[0], temp[1]) && Player.ChessIsEqual(temp[1], temp[2]))
					{
						for (int j = 0; j < 3; j++)
						{
							temp.RemoveAt(0);
						}
					}
				}
				if (!temp.Any())
				{
					Total += 1;
					Console.WriteLine("碰碰胡      2台");
				}
			}

		}


		private void IsTwoKang()
		{
			if (TwoKang == 2)
			{
				Total += 2;
				Console.WriteLine("二槓子      2台");
			}
		}


		private void IsWinOnTheWallTail()
		{
			if (WinOnTheWallTail)
			{
				Total += 2;
				Console.WriteLine("槓上開花    2台");
			}
		}


		private void AllOrHalfRequire()
		{
			if (Meld.Count == 2 && WinBy == State.IsTsumo)
			{
				Total += 2;
				Console.WriteLine("全求        2台");
			}
			if (Meld.Count == 2 && WinBy == State.IsRon)
			{
				Total += 1;
				Console.WriteLine("半求        1台");
			}
		}


		/// <summary>
		/// print patterns to console
		/// </summary>
		public void PrintPatterns()
		{
			Console.WriteLine("牌型        台數");
			Console.WriteLine("---------------");
			TenHou();
			IsTenPai();
			IsBookmaker();
			IsContinueToBookmaker();
			OnlyOrNoGeneralAndSorider();
			ConcealedAndTsumo();
			AllOrHalfRequire();
			IsWinOnTheWallTail();
			WinOnLastOne();
			SameColor();
			if (Eye.Count != 4)
			{
				AllPaired();
				IsTwoKang();
				TwoDragonHug();
				OneDragon();
			}
			else
			{
				FourPair();
			}
			Console.WriteLine("===============");
			Console.WriteLine($"共：        {Total}台\n");
		}
	}
}
