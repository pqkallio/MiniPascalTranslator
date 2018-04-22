using System;

namespace Compiler
{
	public class SimpleExpression : SyntaxTreeNode
	{
		private TermNode term;
		private SimpleExpressionTail tail;
		private bool additiveInverse;

		public SimpleExpression (Token token, TermNode term, SimpleExpressionTail tail, bool additiveInverse)
			: base(token)
		{
			this.term = term;
			this.tail = tail;
			this.additiveInverse = additiveInverse;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}

