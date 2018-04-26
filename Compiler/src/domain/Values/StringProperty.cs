using System;

namespace Compiler
{
	/// <summary>
	/// Designed to save and access a string.
	/// </summary>
	public class StringProperty : Property
	{
		public StringProperty(int declarationRow = int.MaxValue)
			: base(declarationRow)
		{}

		public override TokenType GetTokenType ()
		{
			return TokenType.STRING_VAL;
		}
	}
}

