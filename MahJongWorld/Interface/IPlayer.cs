using System.Collections.Generic;
namespace MahJongWorld.Interface
{
	public interface IPlayer<W, T>
	{
		/// <summary>
		/// Draw <typeparamref name="T"/> From Wall
		/// </summary>
		/// <param name="wall"></param>
		public void Draw(ref W wall);


		/// <summary>
		/// Print Hand and River to Console
		/// </summary>
		public abstract void PrintToConsole();


		/// <summary>
		/// Sort self Hand
		/// </summary>
		public abstract void SortHand();

		/// <summary>
		/// Check is Tsumo 
		/// </summary>
		public abstract void TsumoCheck();


		/// <summary>
		/// Check is Ron by <paramref name="target"/>
		/// </summary>
		/// <param name="target"><typeparamref name="T"/></param>
		public abstract void RonCheck(T target);


		/// <summary>
		/// Find Probably Eye is in hand list
		/// </summary>
		/// <param name="source"> The Source of hand</param>
		/// <returns> return a List of Probably Eye.</returns>
		public abstract List<T> FindProbablyEye(List<T> source);


		/// <summary>
		/// ManualDiscard from Hand to River
		/// </summary>
		public abstract void ManualDiscard();
	}
}
