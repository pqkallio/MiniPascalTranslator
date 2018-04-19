using System;
using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
	/// <summary>
	/// Represents a read statement in the AST
	/// </summary>
	public class IOReadNode : SyntaxTreeNode
	{
		private List<VariableIdNode> idNodes;
		private List<AssignNode> assignNodes;
		private Token token;

		public IOReadNode (List<VariableIdNode> idNodes, Scope scope, Token t)
			: base(t)
		{
			this.idNodes = idNodes;
			this.assignNodes = new List<AssignNode> ();
			foreach (VariableIdNode idNode in idNodes) {
				this.assignNodes.Add (new AssignNode (idNode, scope, idNode.Token));
			}
			this.token = t;
		}

		public List<VariableIdNode> IDNodes
		{
			get { return this.idNodes; }
			set { this.idNodes = value; }
		}

		public List<AssignNode> AssignNodes
		{
			get { return this.assignNodes; }
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitIOReadNode (this);
		}
	}
}

