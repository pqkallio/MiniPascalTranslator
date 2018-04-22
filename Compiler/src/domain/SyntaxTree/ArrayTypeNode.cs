using System;

namespace Compiler
{
	public class ArrayTypeNode : TypeNode
	{
		private ExpressionNode size;
		private TokenType elementType;

		public ArrayTypeNode (Token token, TokenType type, TokenType elementType, ExpressionNode size)
			: base (token, type)
		{
			this.elementType = elementType;
			this.size = size;
		}
	}
}

