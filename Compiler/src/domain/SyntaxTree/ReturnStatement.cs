using System;

namespace Compiler
{
	public class ReturnStatement : StatementNode
	{
		private ExpressionNode returnValue;

		public ReturnStatement (Token token, ExpressionNode returnValue = null)
			: base(token)
		{
			this.returnValue = returnValue;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}

