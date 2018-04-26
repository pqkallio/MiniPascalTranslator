using System;

namespace Compiler
{
	public class WhileNode : StatementNode
	{
		private ExpressionNode condition;
		private StatementNode statement;

		public WhileNode (Token token, INameFactory nameFactory, Scope scope, ExpressionNode condition, StatementNode statement)
			: base(token, nameFactory, scope)
		{
			this.condition = condition;
			this.statement = statement;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return visitor.VisitWhileLoopNode (this);
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