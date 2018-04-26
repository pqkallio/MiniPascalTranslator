using System;

namespace Compiler
{
	public class RealProperty : Property
	{
		public RealProperty(int declarationRow = int.MaxValue)
			: base(declarationRow)
		{}

		public override TokenType GetTokenType ()
		{
			return TokenType.REAL_VAL;
		}
	}
}

