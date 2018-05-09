using System;

namespace Compiler
{
	public class SimpleExpression : Evaluee
	{
		private TermNode term;
		private SimpleExpressionTail tail;
		private bool additiveInverse;

		public SimpleExpression (Token token, TermNode term, SimpleExpressionTail tail, bool additiveInverse, Scope scope)
			: base(token, scope)
		{
			this.term = term;
			this.tail = tail;
			this.additiveInverse = additiveInverse;
			this.evaluationType = TokenType.UNDEFINED;
		}

		public override void Accept(INodeVisitor visitor)
		{
			visitor.VisitSimpleExpression (this);
		}

		public TermNode Term
		{
			get { return this.term; }
		}

		public SimpleExpressionTail Tail
		{
			get { return this.tail; }
		}

		public bool AdditiveInverse
		{
			get { return this.additiveInverse; }
		}
	}
}