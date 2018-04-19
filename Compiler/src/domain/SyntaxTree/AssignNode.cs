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
		private Scope scope;

		public AssignNode (VariableIdNode idNode, Scope scope)
			: this (idNode, scope, null)
		{}

		public AssignNode (VariableIdNode idNode, Scope scope, Token token, ExpressionNode assignValueExpression = null)
			: base(token)
		{
			this.idNode = idNode;
			this.scope = scope;
			this.assignValueExpression = assignValueExpression;
		}

		public VariableIdNode IDNode {
			get { return idNode; }
			set { idNode = value; }
		}

		public ExpressionNode ExprNode {
			get { return assignValueExpression; }
			set { assignValueExpression = value; }
		}

		public void AddExpression(ExpressionNode expressionNode)
		{
			this.assignValueExpression = expressionNode;
		}

		public override string ToString ()
		{
			return "ASSIGN";
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitAssignNode (this);
		}
	}
}

