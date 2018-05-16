using System;

namespace Compiler
{
	public class FunctionProperty : Property
	{
		private TokenType returnType;

		public FunctionProperty (TokenType returnType, int declarationRow = int.MaxValue)
			: base(declarationRow, true)
		{
			this.returnType = returnType;
		}

		public override TokenType GetTokenType ()
		{
			return returnType;
		}
	}
}

