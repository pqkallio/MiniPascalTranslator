using System;

namespace Compiler
{
	public class IfNode : StatementNode
	{
		private ExpressionNode ifCondition;
		private StatementNode ifBranch;
		private StatementNode elseBranch;

		public IfNode (Token token, INameFactory nameFactory, ExpressionNode ifCondition, StatementNode ifBranch, StatementNode elseBranch = null)
			: base(token, nameFactory)
		{
			this.ifCondition = ifCondition;
			this.ifBranch = ifBranch;
			this.elseBranch = elseBranch;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}

