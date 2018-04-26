using System;

namespace Compiler
{
	public class BooleanNegation : Evaluee
	{
		private Factor factor;

		public BooleanNegation (Token token, Factor factor)
			: base (token)
		{
			this.factor = factor;
		}

		public override ISemanticCheckValue Accept (INodeVisitor visitor)
		{
			return null;
		}

		public Factor Factor
		{
			get { return factor; }
		}

		public override TokenType EvaluationType
		{
			get {
				if (evaluationType != TokenType.UNDEFINED) {
					return evaluationType;
				}

				TokenType factorEval = factor.EvaluationType;

				evaluationType = factorEval != TokenType.BOOLEAN_VAL ? factorEval : TokenType.ERROR;

				return evaluationType;
			}
		}
	}
}

