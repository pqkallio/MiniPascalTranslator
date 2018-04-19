using System;

namespace Compiler
{
	public class IfNode : SyntaxTreeNode
	{
		private ExpressionNode ifCondition;
		private ExpressionNode elseCondition;
		private StatementNode ifBranch;
		private StatementNode elseBranch;

		public IfNode (Token token, ExpressionNode ifCondition, StatementNode ifBranch, ExpressionNode elseCondition = null, StatementNode elseBranch = null)
			: base(token)
		{
			this.ifCondition = ifCondition;
			this.elseCondition = elseCondition;
			this.ifBranch = ifBranch;
			this.elseBranch = elseBranch;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}

