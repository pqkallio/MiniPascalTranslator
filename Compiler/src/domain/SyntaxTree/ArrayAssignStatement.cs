using System;

namespace Compiler
{
	public class ArrayAssignStatement : AssignNode
	{
		private ArrayAccessNode arrayAccessNode;

		public ArrayAssignStatement (ArrayAccessNode arrayAccessNode, Scope scope, Token token, ExpressionNode assignValueExpression)
			: base(arrayAccessNode, scope, token, assignValueExpression)
		{
			this.arrayAccessNode = arrayAccessNode;
		}

		public override void Accept (INodeVisitor visitor)
		{
			visitor.VisitArrayAssignNode (this);
		}

		public ExpressionNode IndexExpression
		{
			get { return arrayAccessNode.ArrayIndexExpression; }	
		}
	}
}

