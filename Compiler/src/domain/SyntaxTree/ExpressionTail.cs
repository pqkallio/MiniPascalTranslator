using System;

namespace Compiler
{
	public class ExpressionTail : Evaluee
	{
		private TokenType operation;
		private SimpleExpression rightHandSide;

		public ExpressionTail (Token token, TokenType operation, SimpleExpression rightHandSide, Scope scope)
			: base(token, scope)
		{
			this.operation = operation;
			this.rightHandSide = rightHandSide;
		}

		public SimpleExpression RightHandSide {
			get { return rightHandSide; }
		}

		public override void Accept (INodeVisitor visitor)
		{
			visitor.VisitExpressionTail (this);
		}

		public TokenType Operation
		{
			get { return operation; }
		}
	}
}

