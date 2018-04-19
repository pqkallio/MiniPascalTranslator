using System;

namespace Compiler
{
	public class RealProperty : Property
	{
		public override TokenType GetTokenType ()
		{
			return TokenType.REAL_VAL;
		}
	}
}

