﻿using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents a unary operation in the AST
	/// </summary>
	public class UnOpNode : StatementNode, IExpressionNode
	{
		private IExpressionNode operand;
		private TokenType operation;
		private TokenType evaluationType;

		public UnOpNode (Token t, IExpressionNode operand=null)
			: base(t)
		{
			this.operand = operand;
			this.evaluationType = TokenType.UNDEFINED;
		}

		public IExpressionNode Operand {
			get { return operand; }
			set { this.operand = value; }
		}

		public TokenType Operation {
			get { return operation; }
			set { this.operation = value; }
		}

		public void AddExpression(IExpressionNode expressionNode)
		{
			this.operand = expressionNode;
		}

		public override string ToString ()
		{
			string str = StringFormattingConstants.TOKEN_TYPE_STRINGS [Operation] + "(" + Operand.ToString () + ")";

			return str;
		}

		public TokenType EvaluationType
		{
			get { return this.evaluationType; }
			set { this.evaluationType = value; }
		}

		public IExpressionNode[] GetExpressions()
		{
			IExpressionNode[] expressions = { this.operand };

			return expressions;
		}

		public TokenType GetOperation ()
		{
			return operation;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitUnOpNode (this);
		}
	}
}

