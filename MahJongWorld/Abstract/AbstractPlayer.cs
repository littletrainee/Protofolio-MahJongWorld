using System.Collections.Generic;

namespace MahJongWorld.Abstract
{
	/// <summary>
	/// Abstract class Player must inherit and override
	/// </summary>
	public abstract class AbstractPlayer<C>
	{
		public string Name { get; set; }
		public List<C> Hand { get; set; }
		public bool IsWin { get; set; }



		/// <summary>
		/// Instantiate List of Any List and assignment Name from <paramref name="s"/>
		/// </summary>
		/// <param name="s">the assignment Name by <paramref name="s"/></param>
		public abstract void Initialization(string s);


		/// <summary>
		/// Draw <typeparamref name="C"/> from Wall
		/// </summary>
		/// <param name="wall">List&lt;<typeparamref name="C"/>&gt;</param>
		public void Draw(ref List<C> wall)
		{
			Hand.Add(wall[0]);
			wall.RemoveAt(0);
		}


		/// <summary>
		/// Sort This Player Hand
		/// </summary>
		public abstract void SortHand();


		/// <summary>
		/// Print Hand and River to Console
		/// </summary>
		public abstract void PrintToConsole();


		/// <summary>
		/// Check is Tsumo
		/// </summary>
		public abstract void TsumoCheck();


		/// <summary>
		/// Check is Ron
		/// </summary>
		/// <param name="target">target is <typeparamref name="C"/></param>
		public abstract void RonCheck(C target);


		/// <summary>
		/// Find Probably Eye is in hand list
		/// </summary>
		/// <param name="source"> The Source of hand</param>
		/// <returns> return a List of Probably Eye.</returns>
		public abstract List<C> FindProbablyEye(List<C> source);


		/// <summary>
		///	Determines <paramref name="target"/> is in the <paramref name="targetlist"/> 
		/// </summary>
		/// <param name="targetlist">target confirmed list</param>
		/// <param name="target">the target element</param>
		/// <returns>true if item is found in the 
		/// List&lt;<typeparamref name="C"/>&gt;; otherwise, false.</returns>
		protected abstract bool CheckContains(List<C> targetlist, C target);


		/// <summary>
		/// Check Player Winning is establish
		/// </summary>
		/// <param name="probablyEye"></param>
		/// <param name="hand"></param>
		/// <returns> return bool for is establish of winning </returns>
		protected abstract bool Establish(List<C> probablyEye, List<C> hand);


		/// <summary>
		/// Remove Eye From orginal list
		/// </summary>
		/// <param name="targetEye"></param>
		/// <param name="orginal"></param>
		/// <returns></returns>
		protected abstract void RemoveEye(C targetEye, ref List<C> orginal);


		/// <summary>
		/// Discard From Hand To River
		/// </summary>
		public abstract void Discard();
	}
}
