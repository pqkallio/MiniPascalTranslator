using System;

namespace Compiler
{
	/// <summary>
	/// Used to report an error when scanning a string literal
	/// </summary>
	public class StringLiteralError : Error
	{
		public StringLiteralError (Token token) 
			: this(token,
				ErrorConstants.STRING_LITERAL_ERROR_MESSAGE)
		{}

		public StringLiteralError (Token token, string message) 
			: base(ErrorConstants.SCANNER_ERROR_TITLE,
				message, token)
		{}
	}
}

