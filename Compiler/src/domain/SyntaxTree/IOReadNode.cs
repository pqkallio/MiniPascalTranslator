using System;
using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
	/// <summary>
	/// Represents a read statement in the AST
	/// </summary>
	public class IOReadNode : StatementNode
	{
		private List<VariableIdNode> idNodes;
		private Token token;

		public IOReadNode (List<VariableIdNode> idNodes, Scope scope, Token t, INameFactory nameFactory)
			: base(t, nameFactory, scope)
		{
			this.idNodes = idNodes;
			this.token = t;
		}

		public List<VariableIdNode> IDNodes
		{
			get { return this.idNodes; }
			set { this.idNodes = value; }
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitIOReadNode (this);
		}
	}
}

