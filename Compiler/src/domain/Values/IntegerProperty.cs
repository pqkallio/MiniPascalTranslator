using System;

namespace Compiler
{
	/// <summary>
	/// Designed to save and access an integer.
	/// </summary>
	public class IntegerProperty : Property
	{
		public override TokenType GetTokenType ()
		{
			return TokenType.INTEGER_VAL;
		}
	}
}

