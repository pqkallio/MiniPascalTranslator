using System;

namespace Compiler
{
	public class ExpressionNode : Evaluee
	{
		private SimpleExpression expression;
		private ExpressionTail tail;

		public ExpressionNode (Token token, Scope scope, SimpleExpression expression, ExpressionTail tail = null)
			: base(token, scope)
		{
			this.expression = expression;
			this.tail = tail;
		}

		public override void Accept(INodeVisitor visitor)
		{
			visitor.VisitExpressionNode (this);
		}

		public SimpleExpression SimpleExpression
		{
			get { return expression; }
		}

		public ExpressionTail ExpressionTail
		{
			get { return tail; }
		}
	}
}

