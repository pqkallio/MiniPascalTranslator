using System;

namespace Compiler
{
	public class ArrayAssignStatement : AssignNode
	{
		public ArrayAssignStatement (VariableIdNode idNode, Scope scope, Token token, INameFactory nameFactory, ExpressionNode indexExpression, ExpressionNode assignValueExpression)
			: base(idNode, scope, token, nameFactory, assignValueExpression)
		{
			idNode.ArrayIndex = indexExpression;
		}

		public override ISemanticCheckValue Accept (INodeVisitor visitor)
		{
			return visitor.VisitArrayAssignNode (this);
		}

		public ExpressionNode IndexExpression
		{
			get { return IDNode.ArrayIndex; }
		}
	}
}

