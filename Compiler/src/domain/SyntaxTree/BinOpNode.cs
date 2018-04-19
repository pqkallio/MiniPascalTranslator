using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents a binary operation in the AST
	/// </summary>
	public class BinOpNode : StatementNode, IExpressionNode
	{
		private IExpressionNode leftOperand;
		private IExpressionNode rightOperand;
		private TokenType operation;
		private TokenType evaluationType;

		public BinOpNode (Token t)
			: base(t)
		{
			// by default the operation is a "no operation",
			// meaning that in this case, only the lefthandside would
			// be evaluated
			this.operation = TokenType.BINARY_OP_NO_OP;
			this.evaluationType = TokenType.UNDEFINED;
		}

		public IExpressionNode LeftOperand {
			get { return leftOperand; }
			set { this.leftOperand = value; }
		}

		public IExpressionNode RightOperand {
			get { return rightOperand; }
			set { this.rightOperand = value; }
		}

		public TokenType Operation {
			get { return this.operation; }
			set { this.operation = value; }
		}

		public void AddExpression(IExpressionNode expressionNode)
		{
			if (leftOperand == null) {
				leftOperand = expressionNode;
			} else if (rightOperand == null) {
				rightOperand = expressionNode;
			}
		}

		public override string ToString ()
		{
			string str;

			if (RightOperand != null) {
				if (RightOperand.Operation != TokenType.BINARY_OP_NO_OP) {
					str = LeftOperand.ToString () + " " + StringFormattingConstants.TOKEN_TYPE_STRINGS [Operation] + " (" + RightOperand.ToString () + ")";
				} else {
					str = LeftOperand.ToString () + " " + StringFormattingConstants.TOKEN_TYPE_STRINGS [Operation] + " " + RightOperand.ToString ();
				}
			} else {
				str = LeftOperand.ToString ();
			}

			return str;
		}

		public TokenType EvaluationType
		{
			get { return this.evaluationType; }
			set { this.evaluationType = value; }
		}

		public IExpressionNode[] GetExpressions()
		{
			IExpressionNode[] expressions;

			if (rightOperand != null) {
				expressions = new IExpressionNode[] { leftOperand, rightOperand };
			} else {
				expressions = new IExpressionNode[] { leftOperand };
			}

			return expressions;
		}

		public TokenType GetOperation ()
		{
			return operation;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitBinOpNode (this);
		}
	}
}