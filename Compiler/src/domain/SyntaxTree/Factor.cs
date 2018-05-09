using System;

namespace Compiler
{
	public class Factor : Evaluee
	{
		private FactorMain main;
		private FactorTail tail;

		public Factor (Token token, Scope scope, FactorMain main, FactorTail tail = null)
			: base(token, scope)
		{
			this.main = main;
			this.tail = tail;
		}

		public override void Accept (INodeVisitor visitor)
		{
			visitor.VisitFactorNode (this);
		}

		public FactorMain FactorMain
		{
			get { return main; }
		}

		public FactorTail FactorTail
		{
			get { return tail; }
		}
	}
}

