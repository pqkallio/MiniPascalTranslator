using System;

namespace Compiler
{
	public class FunctionProperty : Property
	{
		public FunctionProperty (int declarationRow = int.MaxValue)
			: base(declarationRow, true)
		{}

		public override TokenType GetTokenType ()
		{
			return TokenType.FUNCTION;
		}
	}
}

