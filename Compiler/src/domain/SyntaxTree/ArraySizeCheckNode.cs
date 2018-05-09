using System;

namespace Compiler
{
	public class ArraySizeCheckNode : FactorTail
	{
		VariableIdNode arrayIdNode;

		public ArraySizeCheckNode (Token token, Scope scope, VariableIdNode arrayIdNode = null)
			: base(token, scope: scope)
		{
			this.arrayIdNode = arrayIdNode;
		}

		public override void Accept(INodeVisitor visitor)
		{
			visitor.VisitArraySizeCheckNode (this);;
		}

		public VariableIdNode IDNode
		{
			get { return arrayIdNode; }
			set { this.arrayIdNode = value; }
		}
	}
}

