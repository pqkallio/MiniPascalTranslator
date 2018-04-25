using System;

namespace Compiler
{
	public class TermTail : Evaluee
	{
		private TokenType operation;
		private Factor factor;
		private TermTail termTail;

		public TermTail (Token token, TokenType operation, Factor factor, TermTail termTail = null)
			: base(token)
		{
			this.operation = operation;
			this.factor = factor;
			this.termTail = termTail;
		}

		public TokenType Operation
		{
			get { return operation; }
		}

		public override ISemanticCheckValue Accept (INodeVisitor visitor)
		{
			return null;
		}

		public override TokenType EvaluationType
		{
			get {
				if (evaluationType != TokenType.UNDEFINED) {
					return evaluationType;
				}

				TokenType factorEval = factor.EvaluationType;

				if (termTail != null) {
					TokenType tailEval = termTail.EvaluationType;

					if (!LegitOperationChecker.IsLegitOperationForEvaluations (termTail.Operation, factorEval, tailEval)) {
						factorEval = TokenType.ERROR;
					}
				}

				evaluationType = factorEval;

				return evaluationType;
			}
		}
	}
}

