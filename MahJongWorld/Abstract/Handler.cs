﻿using System.Collections.Generic;

using MahJongWorld.Shared;

namespace MahJongWorld.Abstract
{
	public abstract class AbstractHandler<T>
	{
		public List<T> Players { get; set; }
		public GameState GameState { get; set; }
		protected List<T> Order { get; set; }
		protected PrintToConsole Print { get; set; }





		/// <summary>
		/// Start This Class with Initization
		/// </summary>
		public abstract void Start();


		/// <summary>
		///	Set Max Player and initialization 
		/// </summary>
		protected abstract void SetEachListAndGameState();


		/// <summary>
		/// Set Each Player Name
		/// </summary>
		protected abstract void SetPlayerName();

		//<typeparamref name="T"/>.
		/// <summary>
		/// Each Player Draw.
		/// </summary>
		protected abstract void Draw();


		/// <summary>
		/// Each Player Sort Hand
		/// </summary>
		protected abstract void SortHand();


		/// <summary>
		/// Add Each Player Print To Delegate
		/// </summary>
		protected abstract void AddPrintToDelegate();


		/// <summary>
		/// Game Loop
		/// </summary>
		public abstract void Update();


		/// <summary>
		/// Reset Order
		/// </summary>
		protected abstract void ResetOrder();


		/// <summary>
		/// Player Discard From Hand To River
		/// </summary>
		/// <param name="player"></param>
		/// <param name="gameState"></param>
		protected abstract void Discard(ref T player, ref GameState gamestate);


		/// <summary>
		/// Each Player Check Ron At Different Thread
		/// </summary>
		/// <param name="order">player list</param>
		protected abstract void CheckRon(List<T> order);

	}
}