using System;

namespace Compiler
{
	public class SimpleExpressionTail : Evaluee
	{
		private TokenType operation;
		private TermNode term;
		private SimpleExpressionTail tail;

		public SimpleExpressionTail (Token token, TokenType operation, TermNode term, SimpleExpressionTail tail)
			: base(token)
		{
			this.operation = operation;
			this.term = term;
			this.tail = tail;
		}

		public TokenType Operation
		{
			get { return operation; }
		}

		public TermNode Term
		{
			get { return this.term; }
		}

		public SimpleExpressionTail Tail
		{
			get { return this.tail; }
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}

		public override TokenType EvaluationType
		{
			get {
				if (evaluationType != TokenType.UNDEFINED) {
					return evaluationType;
				}

				TokenType termEval = term.EvaluationType;

				if (tail != null) {
					TokenType tailEval = tail.EvaluationType;

					if (!LegitOperationChecker.IsLegitOperationForEvaluations(tail.Operation, termEval, tailEval)) {
						termEval = TokenType.ERROR;
					}
				}

				evaluationType = termEval;

				return evaluationType;
			}
		}
	}
}

