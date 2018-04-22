using System;

namespace Compiler
{
	public class ArrayAssignStatement : AssignNode
	{
		private ExpressionNode indexExpression;

		public ArrayAssignStatement (VariableIdNode idNode, Scope scope, Token token, INameFactory nameFactory, ExpressionNode indexExpression, ExpressionNode assignValueExpression)
			: base(idNode, scope, token, nameFactory, assignValueExpression)
		{
			this.indexExpression = indexExpression;
		}

		public override ISemanticCheckValue Accept (INodeVisitor visitor)
		{
			return null;
		}
	}
}

