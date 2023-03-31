using System.Collections.Generic;

using MahJongWorld.Shared;

namespace MahJongWorld.Abstract
{
	public abstract class AbstractScore<T>
	{
		protected List<T> Hand { get; set; }
		protected int Total { get; set; }
		protected GameState GameState { get; set; }
		protected State WinBy { get; set; }
	}
}
