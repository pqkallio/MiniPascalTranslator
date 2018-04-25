using System;

namespace Compiler
{
	public class FactorMain : Evaluee
	{
		private Evaluee evaluee;

		public FactorMain (Token token, Evaluee evaluee)
			: base(token)
		{
			this.evaluee = evaluee;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}

		public override TokenType EvaluationType
		{
			get {
				if (evaluationType == TokenType.UNDEFINED) {
					evaluationType = evaluee.EvaluationType;
				}

				return evaluationType;
			}
		}
	}
}

