using System;

namespace Compiler
{
	public class ExpressionTail : Evaluee
	{
		private TokenType operation;
		private SimpleExpression rightHandSide;

		public ExpressionTail (Token token, TokenType operation, SimpleExpression rightHandSide)
			: base(token)
		{
			this.operation = operation;
			this.rightHandSide = rightHandSide;
		}

		public SimpleExpression RightHandSide {
			get { return rightHandSide; }
		}

		public override ISemanticCheckValue Accept (INodeVisitor visitor)
		{
			return visitor.VisitExpressionTail (this);
		}

		public override TokenType EvaluationType {
			get { return TokenType.BOOLEAN_VAL; }
		}

		public TokenType Operation
		{
			get { return operation; }
		}
	}
}

