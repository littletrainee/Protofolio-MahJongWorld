using System.Collections.Generic;

namespace MahJongWorld.Abstract
{
	/// <summary>
	/// Abstract class Player must inherit and override
	/// </summary>
	public abstract class AbstractPlayer<T>
	{
		public string Name { get; set; }
		public List<T> Hand { get; set; }
		public bool IsWin { get; set; }




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
		///	Determines <paramref name="target"/> is in the <paramref name="targetlist"/> 
		/// </summary>
		/// <param name="targetlist">target confirmed list</param>
		/// <param name="target">the target element</param>
		/// <returns>true if item is found in the 
		/// List&lt;<typeparamref name="T"/>&gt;; otherwise, false.</returns>
		protected abstract bool CheckContains(List<T> targetlist, T target);


		/// <summary>
		/// Check Player Winning is establish
		/// </summary>
		/// <param name="probablyEye"></param>
		/// <param name="hand"></param>
		/// <returns> return bool for is establish of winning </returns>
		protected abstract bool Establish(List<T> probablyEye, List<T> hand);


		/// <summary>
		/// Remove Eye From orginal list
		/// </summary>
		/// <param name="targetEye"></param>
		/// <param name="orginal"></param>
		/// <returns></returns>
		protected abstract void RemoveEye(T targetEye, ref List<T> orginal);

		/// <summary>
		/// ManualDiscard from Hand to River
		/// </summary>
		public abstract void Discard();
	}
}
