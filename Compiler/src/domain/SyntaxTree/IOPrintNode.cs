using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents a print statement in the AST
	/// </summary>
	public class IOPrintNode: StatementNode
	{
		private IExpressionNode expression;

		public IOPrintNode (Token t)
			: base(t)
		{}

		public IExpressionNode Expression
		{
			get { return expression; }
		}

		public void AddExpression(IExpressionNode expressionNode)
		{
			this.expression = expressionNode;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitIOPrintNode (this);
		}
	}
}

