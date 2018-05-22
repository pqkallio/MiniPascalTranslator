using System;

namespace Compiler
{
	public class RealProperty : Property
	{
		public RealProperty(int declarationRow = int.MaxValue, bool assigned = false, bool reference = false, bool redeclaration = false)
			: base(declarationRow, assigned, reference, redeclaration: redeclaration)
		{}

		public override TokenType GetTokenType ()
		{
			return TokenType.REAL_VAL;
		}
	}
}

