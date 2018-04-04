using System;

namespace Compiler
{
	/// <summary>
	/// Used to report an error while parsing a token.
	/// </summary>
	public class TokenError : Error
	{
		public TokenError (Token token)
			: this(token, ErrorConstants.TOKEN_ERROR_MESSAGE)
		{
		}

		public TokenError (Token token, string message) 
			: base(ErrorConstants.SCANNER_ERROR_TITLE, message, token)
		{
		}
	}
}

