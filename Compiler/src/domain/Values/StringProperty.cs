using System;

namespace Compiler
{
	/// <summary>
	/// Designed to save and access a string.
	/// </summary>
	public class StringProperty : Property
	{
		public StringProperty(int declarationRow = int.MaxValue, bool assigned = false, bool redeclaration = false)
			: base(declarationRow, assigned, true, redeclaration: redeclaration)
		{}

		public override TokenType GetTokenType ()
		{
			return TokenType.STRING_VAL;
		}
	}
}

