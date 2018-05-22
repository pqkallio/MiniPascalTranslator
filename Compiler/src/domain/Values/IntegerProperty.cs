using System;

namespace Compiler
{
	/// <summary>
	/// Designed to save and access an integer.
	/// </summary>
	public class IntegerProperty : Property
	{
		public IntegerProperty(int declarationRow = int.MaxValue, bool assigned = false, bool reference = false, bool redeclaration = false)
			: base(declarationRow, assigned, reference, redeclaration)
		{}

		public override TokenType GetTokenType ()
		{
			return TokenType.INTEGER_VAL;
		}
	}
}

