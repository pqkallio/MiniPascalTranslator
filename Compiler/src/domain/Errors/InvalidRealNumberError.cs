using System;

namespace Compiler
{
	public class InvalidRealNumberError : Error
	{
		public InvalidRealNumberError (Token token)
			: base (ErrorConstants.SCANNER_ERROR_TITLE, ErrorConstants.INVALID_REAL_NUM_MESSAGE, token)
		{}
	}
}

