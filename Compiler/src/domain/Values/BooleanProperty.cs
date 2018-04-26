using System;

namespace Compiler
{
	/// <summary>
	/// Designed to save and access a boolean value.
	/// </summary>
	public class BooleanProperty : Property
	{
		public BooleanProperty(int declarationRow = int.MaxValue)
			: base(declarationRow)
		{}

		public override TokenType GetTokenType ()
		{
			return TokenType.BOOLEAN_VAL;
		}
	}
}

