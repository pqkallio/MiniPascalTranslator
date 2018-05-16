using System;

namespace Compiler
{
	public class ArrayProperty : Property
	{
		private ExpressionNode sizeExpression;
		private TokenType arrayElementType;

		public ArrayProperty (TokenType valueType, ExpressionNode sizeExpression = null, int declarationRow = int.MaxValue)
			: base(declarationRow, true)
		{
			this.arrayElementType = valueType;
			this.sizeExpression = sizeExpression;
		}

		public override TokenType GetTokenType ()
		{
			return TokenType.TYPE_ARRAY;
		}

		public TokenType ArrayElementType
		{
			get { return arrayElementType; }
		}

		public ExpressionNode SizeExpression
		{
			get { return sizeExpression; }
		}
	}
}

