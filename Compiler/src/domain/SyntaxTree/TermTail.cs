using System;

namespace Compiler
{
	public class TermTail : Evaluee
	{
		private TokenType operation;
		private Factor factor;
		private TermTail termTail;

		public TermTail (Token token, Scope scope, TokenType operation, Factor factor, TermTail termTail = null)
			: base(token, scope)
		{
			this.operation = operation;
			this.factor = factor;
			this.termTail = termTail;
		}

		public TokenType Operation
		{
			get { return operation; }
		}

		public override void Accept (INodeVisitor visitor)
		{
			visitor.VisitTermTailNode (this);
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