using System;

namespace Compiler
{
	/// <summary>
	/// Designed to save and access a boolean value.
	/// </summary>
	public class BooleanProperty : Property
	{
		public override TokenType GetTokenType ()
		{
			return TokenType.BOOLEAN_VAL;
		}
	}
}

