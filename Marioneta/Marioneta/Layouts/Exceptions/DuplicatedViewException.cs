using System;

namespace Marioneta
{
	/// <summary>
	/// This exception will be launched when there is an existing view that is being added to the layout.
	/// </summary>
	public class DuplicatedViewException : Exception
	{
		public DuplicatedViewException (string message) : base(message)
		{
			
		}
	}
}

