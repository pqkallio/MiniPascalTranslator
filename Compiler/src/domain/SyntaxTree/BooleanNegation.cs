using System;

namespace Compiler
{
	public class BooleanNegation : Evaluee
	{
		private Factor factor;

		public BooleanNegation (Token token, Scope scope, Factor factor)
			: base (token, scope)
		{
			this.factor = factor;
		}

		public override void Accept (INodeVisitor visitor)
		{
			visitor.VisitBooleanNegation(this);
		}

		public Factor Factor
		{
			get { return factor; }
		}
	}
}

