using System;

namespace Compiler
{
	public class FunctionProperty : Property
	{
		public FunctionProperty ()
			: base(true)
		{}

		public override TokenType GetTokenType ()
		{
			return TokenType.FUNCTION;
		}
	}
}

