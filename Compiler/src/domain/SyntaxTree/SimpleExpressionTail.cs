using System;

namespace Compiler
{
	public class SimpleExpressionTail : SyntaxTreeNode
	{
		private TokenType operation;
		private TermNode term;
		private SimpleExpressionTail tail;

		public SimpleExpressionTail (Token token, TokenType operation, TermNode term, SimpleExpressionTail tail)
			: base(token)
		{
			this.operation = operation;
			this.term = term;
			this.tail = tail;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}

