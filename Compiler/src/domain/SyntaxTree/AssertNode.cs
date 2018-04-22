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

		public AssertNode (Token token, ExpressionNode expression, INameFactory nameFactory)
			: base(token, nameFactory)
		{
			this.expression = expression;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitAssertNode (this);
		}
	}
}

