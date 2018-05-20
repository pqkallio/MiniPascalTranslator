using System;

namespace Compiler
{
	public class ArrayAccessNode : VariableEvaluee
	{
		private VariableIdNode arrayId;
		private ExpressionNode index;

		public ArrayAccessNode (VariableIdNode arrayId, Token token, Scope scope, ExpressionNode index)
			: base(token, scope: scope)
		{
			this.arrayId = arrayId;
			this.index = index;
		}

		public override void Accept(INodeVisitor visitor)
		{
			visitor.VisitArrayAccessNode (this);
		}

		public VariableIdNode ArrayIdNode
		{
			get { return arrayId; }
		}

		public ExpressionNode ArrayIndexExpression
		{
			get { return this.index; }
		}

		public override VariableIdNode IDNode {
			get { return arrayId; }
		}

		public override string VariableID
		{
			get { return IDNode.ID; }
		}
	}
}

