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
		private VariableEvaluee idNode;
		private ExpressionNode assignValueExpression;

		public AssignNode (VariableEvaluee idNode, Scope scope, Token token, ExpressionNode assignValueExpression = null)
			: base(token, scope: scope)
		{
			this.idNode = idNode;
			this.assignValueExpression = assignValueExpression;
		}

		public VariableEvaluee IDNode {
			get { return idNode; }
			set { idNode = value; }
		}

		public ExpressionNode AssignValueExpression {
			get { return assignValueExpression; }
			set { assignValueExpression = value; }
		}

		public override void Accept(INodeVisitor visitor) {
			visitor.VisitAssignNode (this);
		}
	}
}

