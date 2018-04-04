using System;

namespace Compiler
{
	/// <summary>
	/// Used to report an error when an integer operation overflows.
	/// </summary>
	public class IntegerOverflowError : Error
	{
		public IntegerOverflowError (Token token)
			: base(ErrorConstants.INTEGER_OVERFLOW_ERROR_TITLE, ErrorConstants.INTEGER_OVERFLOW_ERROR_MESSAGE, token)
		{}
	}
}

