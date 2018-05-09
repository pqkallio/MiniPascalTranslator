using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents an assert statement in the AST.
	/// </summary>
	public class AssertNode : StatementNode
	{
		private ExpressionNode expression;

		public AssertNode (Token token, ExpressionNode expression, Scope scope)
			: base(token, scope)
		{
			this.expression = expression;
		}

		public override void Accept(INodeVisitor visitor) {
			visitor.VisitAssertNode (this);
		}

		public ExpressionNode AssertExpression
		{
			get { return expression; }
		}
	}
}

