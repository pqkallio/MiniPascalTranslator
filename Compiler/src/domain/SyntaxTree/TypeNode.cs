using System;

namespace Compiler
{
	public class TypeNode : SyntaxTreeNode
	{
		private TokenType type;

		public TypeNode (Token token, TokenType type)
			: base(token)
		{
			this.type = type;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}

