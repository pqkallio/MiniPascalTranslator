using System;

namespace Compiler
{
	public class TermNode : Evaluee
	{
		private Factor factorNode;
		private TermTail termTailNode;

		public TermNode (Token token, Scope scope, Factor factorNode, TermTail termTailNode = null)
			: base(token, scope)
		{
			this.factorNode = factorNode;
			this.termTailNode = termTailNode;
		}

		public override void Accept(INodeVisitor visitor)
		{
			visitor.VisitTermNode (this);
		}

		public Factor Factor
		{
			get { return factorNode; }
		}

		public TermTail TermTail
		{
			get { return termTailNode; }
		}
	}
}

