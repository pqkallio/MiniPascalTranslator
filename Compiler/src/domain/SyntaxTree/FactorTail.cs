using System;

namespace Compiler
{
	public class FactorTail : Evaluee
	{
		public FactorTail (Token token, Scope scope = null)
			: base(token, scope: scope)
		{}

		public override void Accept(INodeVisitor visitor)
		{
			visitor.VisitFactorTail (this);
		}
	}
}

