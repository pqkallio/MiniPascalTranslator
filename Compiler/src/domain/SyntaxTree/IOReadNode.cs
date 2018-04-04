using System;
using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
	/// <summary>
	/// Represents a read statement in the AST
	/// </summary>
	public class IOReadNode : ISyntaxTreeNode, IIdentifierContainer
	{
		private VariableIdNode idNode;
		private AssignNode assignNode;
		private Token token;

		public IOReadNode (VariableIdNode idNode, Dictionary<string, IProperty> symbolTable, Token t)
		{
			this.idNode = idNode;
			this.assignNode = new AssignNode (this.idNode, symbolTable, t);
			this.token = t;
		}

		public VariableIdNode IDNode
		{
			get { return this.idNode; }
			set { this.idNode = value; }
		}

		public AssignNode AssignNode
		{
			get { return this.assignNode; }
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitIOReadNode (this);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}
	}
}

