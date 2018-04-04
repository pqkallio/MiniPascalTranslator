using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Compiler
{
	/// <summary>
	/// Represents an assignment in the AST
	/// </summary>
	public class AssignNode : IExpressionContainer, IIdentifierContainer
	{
		private VariableIdNode idNode;
		private IExpressionNode exprNode;
		private Token token;

		public AssignNode (VariableIdNode idNode, Dictionary<string, IProperty> symbolTable)
			: this (idNode, symbolTable, null)
		{}

		public AssignNode (VariableIdNode idNode, Dictionary<string, IProperty> symbolTable, Token token)
		{
			this.idNode = idNode;
			this.exprNode = null;
			this.token = token;
		}

		public VariableIdNode IDNode {
			get { return idNode; }
			set { idNode = value; }
		}

		public IExpressionNode ExprNode {
			get { return exprNode; }
			set { exprNode = value; }
		}

		public void AddExpression(IExpressionNode expressionNode)
		{
			this.exprNode = expressionNode;
		}

		public override string ToString ()
		{
			return "ASSIGN";
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitAssignNode (this);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}
	}
}

