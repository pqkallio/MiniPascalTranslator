using System;

namespace Compiler
{
	/// <summary>
	/// Used to report an error when an identifier with illegal character's is being declared or accessed.
	/// </summary>
	public class InvalidIdentifierError : Error
	{
		public InvalidIdentifierError (Token token)
			: base(ErrorConstants.SCANNER_ERROR_TITLE, ErrorConstants.INVALID_IDENTIFIER_MESSAGE, token)
		{}
	}
}

