using System;

namespace Compiler
{
	public class ArrayTypeNode : TypeNode
	{
		private IExpressionNode size;
		private TokenType elementType;

		public ArrayTypeNode (Token token, TokenType type, TokenType elementType, IExpressionNode size)
			: base (token, type)
		{
			this.elementType = elementType;
			this.size = size;
		}
	}
}

