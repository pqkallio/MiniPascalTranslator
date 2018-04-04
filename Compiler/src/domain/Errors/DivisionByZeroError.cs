using System;

namespace Compiler
{
	/// <summary>
	/// Used to report an error when an integer is being divided by zero.
	/// </summary>
	public class DivisionByZeroError : Error
	{
		public DivisionByZeroError (Token token)
			: base(ErrorConstants.DIVISION_BY_ZERO_TITLE, ErrorConstants.DIVISION_BY_ZERO_MESSAGE, token)
		{}
	}
}

