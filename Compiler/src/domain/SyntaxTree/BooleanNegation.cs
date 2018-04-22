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
	}
}

