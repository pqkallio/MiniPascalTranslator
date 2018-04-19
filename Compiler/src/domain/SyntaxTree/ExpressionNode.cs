using System;

namespace Compiler
{
	public class ExpressionNode : SyntaxTreeNode
	{
		private SimpleExpression expression;
		private ExpressionTail tail;

		public ExpressionNode (Token token, SimpleExpression expression, ExpressionTail tail = null)
			: base(token)
		{
			this.expression = expression;
			this.tail = tail;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}

