using System;

namespace Compiler
{
	public class TermNode : Evaluee
	{
		private Factor factorNode;
		private TermTail termTailNode;

		public TermNode (Token token, Factor factorNode, TermTail termTailNode)
			: base(token)
		{}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return visitor.VisitTermNode (this);
		}

		public Factor Factor
		{
			get { return factorNode; }
		}

		public TermTail TermTail
		{
			get { return termTailNode; }
		}

		public override TokenType EvaluationType
		{
			get {
				if (evaluationType != TokenType.UNDEFINED) {
					return evaluationType;
				}

				TokenType factorEval = factorNode.EvaluationType;

				if (termTailNode != null) {
					TokenType tailEval = termTailNode.EvaluationType;

					if (!LegitOperationChecker.IsLegitOperationForEvaluations(termTailNode.Operation, factorEval, tailEval)) {
						factorEval = TokenType.ERROR;
					}
				}

				evaluationType = factorEval;

				return evaluationType;
			}
		}
	}
}

