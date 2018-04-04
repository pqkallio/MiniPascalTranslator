using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents an assert statement in the AST.
	/// </summary>
	public class AssertNode : IExpressionContainer
	{
		private IExpressionNode expressionNode;
		private IOPrintNode ioPrintNode;
		private Token token;

		public AssertNode (Token t)
		{
			this.token = t;
		}

		public IOPrintNode IOPrintNode {
			get { return this.ioPrintNode; }
			set { this.ioPrintNode = value; }
		}
			
		public IExpressionNode Expression
		{
			get { return expressionNode; }
		}

		public void AddExpression (IExpressionNode expressionNode)
		{
			this.expressionNode = expressionNode;
		}

		public override string ToString ()
		{
			return "ASSERT";
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitAssertNode (this);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}
	}
}

