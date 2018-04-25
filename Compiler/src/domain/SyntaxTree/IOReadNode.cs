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

		public IOReadNode (List<VariableIdNode> idNodes, Scope scope, Token token, INameFactory nameFactory)
			: base(token, nameFactory, scope)
		{
			this.idNodes = idNodes;
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

