using System;

namespace Compiler
{
	public class WhileNode : StatementNode
	{
		private ExpressionNode condition;
		private StatementNode statement;

		public WhileNode (Token token, Scope scope, ExpressionNode condition, StatementNode statement)
			: base(token, scope)
		{
			this.condition = condition;
			this.statement = statement;
		}

		public override void Accept(INodeVisitor visitor)
		{
			visitor.VisitWhileLoopNode (this);
		}

		public ExpressionNode Condition
		{
			get { return condition; }
		}

		public StatementNode Statement
		{
			get { return statement; }
		}
	}
}