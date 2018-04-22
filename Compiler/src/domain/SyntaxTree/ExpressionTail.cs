using System;

namespace Compiler
{
	public class ExpressionTail : SyntaxTreeNode
	{
		private TokenType operation;
		private SimpleExpression rightHandSide;

		public ExpressionTail (Token token, TokenType operation, SimpleExpression rightHandSide)
			: base(token)
		{
			this.operation = operation;
			this.rightHandSide = rightHandSide;
		}

		public override ISemanticCheckValue Accept (INodeVisitor visitor)
		{
			return null;
		}
	}
}

