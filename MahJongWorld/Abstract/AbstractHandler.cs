using System.Collections.Generic;

using MahJongWorld.Shared;

namespace MahJongWorld.Abstract
{
	public abstract class AbstractHandler<T>
	{
		public List<T> Players { get; set; }
		public GameState<T> GameState { get; set; }
		//public List<T> Order { get; set; }
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

		//<typeparamref name="C"/>.
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
		//protected abstract void ResetOrder();

		/// <summary>
		/// Check Players[0] is Tsumo
		/// </summary>
		protected abstract void CheckTsumo();


		/// <summary>
		/// Player Discard From Hand To River
		/// </summary>
		protected abstract void Discard();


		/// <summary>
		/// Each Player Check Ron At Different Thread
		/// </summary>
		/// <param name="order">player list</param>
		//public abstract void CheckRon(List<C> order);
		public abstract void CheckRon();


		/// <summary>
		/// which one is ron and ron by who
		/// </summary>
		/// <returns>(which one ron , ron by which one)</returns>
		protected abstract (T, T) WhoRon();

		/// <summary>
		/// Next Player Draw Chess From Wall
		/// </summary>
		/// <param name="nextPlayerCode"> the latter one who was after Order[0]</param>
		protected abstract void DrawFromWall();

	}
}
