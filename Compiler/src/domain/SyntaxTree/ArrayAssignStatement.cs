using System;

namespace Compiler
{
	public class ArrayAssignStatement : AssignNode
	{
		private ExpressionNode indexExpression;

		public ArrayAssignStatement (VariableIdNode idNode, Scope scope, Token token, ExpressionNode indexExpression, ExpressionNode assignValueExpression)
			: base(idNode, scope, token, assignValueExpression)
		{
			this.indexExpression = indexExpression;
		}

		public ISemanticCheckValue Accept (INodeVisitor visitor)
		{
			return null;
		}
	}
}

