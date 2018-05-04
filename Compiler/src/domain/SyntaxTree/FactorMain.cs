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
			return visitor.VisitFactorMain (this);
		}

		public Evaluee Evaluee
		{
			get { return evaluee; }
		}
	}
}

