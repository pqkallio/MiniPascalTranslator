using System;

namespace Compiler
{
	public class TermNode : Evaluee
	{
		private Factor factorNode;
		private TermTail termTailNode;

		public TermNode (Token token, Factor factorNode, TermTail termTailNode = null)
			: base(token)
		{
			this.factorNode = factorNode;
			this.termTailNode = termTailNode;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return visitor.VisitTermNode (this);
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

