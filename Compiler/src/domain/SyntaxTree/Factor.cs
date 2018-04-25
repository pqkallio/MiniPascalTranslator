using System;

namespace Compiler
{
	public class Factor : Evaluee
	{
		private FactorMain main;
		private FactorTail tail;

		public Factor (Token token, FactorMain main, FactorTail tail = null)
			: base(token)
		{
			this.main = main;
			this.tail = tail;
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

				if (tail != null) {
					if (tail.Token.Type == TokenType.SIZE && main.Variable) {
						evaluationType = TokenType.INTEGER_VAL;
					} else {
						evaluationType = TokenType.ERROR;
					}
				} else {
					evaluationType = main.EvaluationType;
				}

				return evaluationType;
			}
		}
	}
}

