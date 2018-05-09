using System;

namespace Compiler
{
	public class FactorMain : Evaluee
	{
		private Evaluee evaluee;

		public FactorMain (Token token, Scope scope, Evaluee evaluee)
			: base(token, scope)
		{
			this.evaluee = evaluee;
		}

		public override void Accept(INodeVisitor visitor)
		{
			visitor.VisitFactorMain (this);
		}

		public Evaluee Evaluee
		{
			get { return evaluee; }
		}
	}
}

