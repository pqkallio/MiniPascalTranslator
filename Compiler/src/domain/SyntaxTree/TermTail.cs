using System;

namespace Compiler
{
	public class TermTail : Evaluee
	{
		private TokenType operation;
		private Factor factor;
		private TermTail termTail;

		public TermTail (Token token, TokenType operation, Factor factor, TermTail termTail = null)
			: base(token)
		{
			this.operation = operation;
			this.factor = factor;
			this.termTail = termTail;
		}

		public TokenType Operation
		{
			get { return operation; }
		}

		public override ISemanticCheckValue Accept (INodeVisitor visitor)
		{
			return visitor.VisitTermTailNode (this);
		}

		public Factor Factor
		{
			get { return factor; }
		}

		public TermTail ChildTermTail
		{
			get { return termTail; }
		}
	}
}