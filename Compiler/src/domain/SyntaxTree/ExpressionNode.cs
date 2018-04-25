using System;

namespace Compiler
{
	public class ExpressionNode : Evaluee
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

		public override TokenType EvaluationType {
			get {
				if (this.evaluationType != TokenType.UNDEFINED) {
					return this.evaluationType;
				}

				TokenType expressionEvaluation = expression.EvaluationType;

				if (tail != null) {
					TokenType tailEvaluation = tail.RightHandSide.EvaluationType;

					if (!LegitOperationChecker.IsLegitOperationForEvaluations(tail.Operation, expressionEvaluation, tailEvaluation)) {
						expressionEvaluation = TokenType.ERROR;
					} else {
						expressionEvaluation = tail.EvaluationType;
					}
				}

				this.evaluationType = expressionEvaluation;

				return this.evaluationType;
			}
		}
	}
}

