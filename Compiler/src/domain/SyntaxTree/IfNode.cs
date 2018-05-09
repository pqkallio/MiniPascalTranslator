using System;

namespace Compiler
{
	public class IfNode : StatementNode
	{
		private ExpressionNode condition;
		private StatementNode ifBranch;
		private StatementNode elseBranch;

		public IfNode (Token token, Scope scope, ExpressionNode condition, StatementNode ifBranch, StatementNode elseBranch = null)
			: base(token, scope)
		{
			this.condition = condition;
			this.ifBranch = ifBranch;
			this.elseBranch = elseBranch;
		}

		public override void Accept(INodeVisitor visitor)
		{
			visitor.VisitIfNode (this);
		}

		public ExpressionNode Condition
		{
			get { return condition; }
		}

		public StatementNode IfBranch
		{
			get { return ifBranch; }
		}

		public StatementNode ElseBranch
		{
			get { return elseBranch; }
		}
	}
}

