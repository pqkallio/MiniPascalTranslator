using System;

namespace Compiler
{
	public class SimpleTypeNode : TypeNode
	{
		public SimpleTypeNode (Token token, TokenType type)
			: base (token, type)
		{}
	}
}

