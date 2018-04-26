using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Compiler
{
	/// <summary>
	/// Represents an assignment in the AST
	/// </summary>
	public class AssignNode : StatementNode
	{
		private VariableIdNode idNode;
		private ExpressionNode assignValueExpression;

		public AssignNode (VariableIdNode idNode, Scope scope, INameFactory nameFactory)
			: this (idNode, scope, null, nameFactory)
		{}

		public AssignNode (VariableIdNode idNode, Scope scope, Token token, INameFactory nameFactory, ExpressionNode assignValueExpression = null)
			: base(token, nameFactory, scope)
		{
			this.idNode = idNode;
			this.assignValueExpression = assignValueExpression;
		}

		public VariableIdNode IDNode {
			get { return idNode; }
			set { idNode = value; }
		}

		public ExpressionNode AssignValueExpression {
			get { return assignValueExpression; }
			set { assignValueExpression = value; }
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitAssignNode (this);
		}
	}
}

