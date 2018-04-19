using System;

namespace Compiler
{
	/// <summary>
	/// Represents an unusable property.
	/// </summary>
	public class ErrorProperty : Property
	{
		public override TokenType GetTokenType ()
		{
			return TokenType.ERROR;
		}
	}
}

