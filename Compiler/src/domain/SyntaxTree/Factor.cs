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
			return visitor.VisitFactorNode (this);
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

