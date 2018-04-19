using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents an assert statement in the AST.
	/// </summary>
	public class AssertNode : StatementNode
	{
		private IExpressionNode expressionNode;
		private IOPrintNode ioPrintNode;
		private Token token;

		public AssertNode (Token t)
			: base(t)
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

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitAssertNode (this);
		}
	}
}

