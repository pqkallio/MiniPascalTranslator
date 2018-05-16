using System;

namespace Compiler
{
	public class ArraySizeCheckNode : FactorMain
	{
		Evaluee arrayIdNode;

		public ArraySizeCheckNode (Token token, Scope scope, Evaluee arrayIdNode = null)
			: base(token, scope, arrayIdNode)
		{
			this.arrayIdNode = arrayIdNode;
		}

		public override void Accept(INodeVisitor visitor)
		{
			visitor.VisitArraySizeCheckNode (this);
		}

		public Evaluee ArrayIDNode
		{
			get { return arrayIdNode; }
			set { this.arrayIdNode = value; }
		}

		public override string VariableID
		{
			get {
				if (arrayIdNode == null)
					return null;
				else
					return arrayIdNode.VariableID;
			}
		}
	}
}

