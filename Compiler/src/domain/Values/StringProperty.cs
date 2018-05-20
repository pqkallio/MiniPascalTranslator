using System;

namespace Compiler
{
	/// <summary>
	/// Designed to save and access a string.
	/// </summary>
	public class StringProperty : Property
	{
		public StringProperty(int declarationRow = int.MaxValue, bool assigned = false)
			: base(declarationRow, assigned, true)
		{}

		public override TokenType GetTokenType ()
		{
			return TokenType.STRING_VAL;
		}
	}
}

