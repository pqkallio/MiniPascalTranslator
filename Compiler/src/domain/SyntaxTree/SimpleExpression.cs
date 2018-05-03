using System;

namespace Compiler
{
	public class SimpleExpression : Evaluee
	{
		private TermNode term;
		private SimpleExpressionTail tail;
		private bool additiveInverse;

		public SimpleExpression (Token token, TermNode term, SimpleExpressionTail tail, bool additiveInverse)
			: base(token)
		{
			this.term = term;
			this.tail = tail;
			this.additiveInverse = additiveInverse;
			this.evaluationType = TokenType.UNDEFINED;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return visitor.VisitSimpleExpression (this);
		}

		public TermNode Term
		{
			get { return this.term; }
		}

		public SimpleExpressionTail Tail
		{
			get { return this.tail; }
		}

		public bool AdditiveInverse
		{
			get { return this.additiveInverse; }
		}

		public override TokenType EvaluationType
		{
			get {
				if (evaluationType != TokenType.UNDEFINED) {
					return evaluationType;
				}

				TokenType termEval = term.EvaluationType;

				if (this.tail != null) {
					TokenType tailEval = tail.EvaluationType;

					if (!LegitOperationChecker.IsLegitOperationForEvaluations (tail.Operation, termEval, tailEval)) {
						termEval = TokenType.ERROR;
					}
				}

				if (additiveInverse && 
					!LegitOperationChecker.IsLegitOperationForEvaluations(TokenType.UNARY_OP_NEGATIVE, termEval)) {
					termEval = TokenType.ERROR;
				}

				this.evaluationType = termEval;

				return this.evaluationType;
			}
		}
	}
}