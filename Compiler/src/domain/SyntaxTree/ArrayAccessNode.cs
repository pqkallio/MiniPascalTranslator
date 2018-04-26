using System;

namespace Compiler
{
	public class ArrayAccessNode : SyntaxTreeNode, ISemanticCheckValue
	{
		private VariableIdNode arrayId;
		private ExpressionNode index;

		public ArrayAccessNode (VariableIdNode arrayId, Token token, Scope scope, ExpressionNode index)
			: base(token, scope: scope)
		{
			this.arrayId = arrayId;
			this.index = index;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return visitor.VisitArrayAccessNode (this);
		}

		public Property asProperty ()
		{
			return new ErrorProperty ();
		}

		public VariableIdNode ArrayIdNode
		{
			get { return arrayId; }
		}

		public ExpressionNode ArrayIndexExpression
		{
			get { return this.index; }
		}
	}
}

