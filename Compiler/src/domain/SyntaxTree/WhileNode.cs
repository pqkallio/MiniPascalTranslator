using System;

namespace Compiler
{
	public class WhileNode : SyntaxTreeNode
	{
		private ExpressionNode condition;
		private StatementNode statement;

		public WhileNode (Token token, ExpressionNode condition, StatementNode statement)
			: base(token)
		{
			this.condition = condition;
			this.statement = statement;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}