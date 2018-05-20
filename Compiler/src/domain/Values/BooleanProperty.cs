using System;

namespace Compiler
{
	/// <summary>
	/// Designed to save and access a boolean value.
	/// </summary>
	public class BooleanProperty : Property
	{
		public BooleanProperty(int declarationRow = int.MaxValue, bool assigned = false, bool reference = false)
			: base(declarationRow, assigned, reference)
		{}

		public override TokenType GetTokenType ()
		{
			return TokenType.BOOLEAN_VAL;
		}
	}
}

