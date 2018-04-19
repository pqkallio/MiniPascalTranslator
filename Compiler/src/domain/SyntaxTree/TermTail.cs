using System;

namespace Compiler
{
	public class TermTail : SyntaxTreeNode
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

		public override ISemanticCheckValue Accept (INodeVisitor visitor)
		{
			return null;
		}
	}
}

