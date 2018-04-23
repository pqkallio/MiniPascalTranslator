using System;

namespace Compiler
{
	public class ArrayTypeNode : TypeNode
	{
		public ArrayTypeNode (Token token, TokenType type, TokenType elementType, ExpressionNode size)
			: base (token, type, size, elementType)
		{
		}
	}
}

