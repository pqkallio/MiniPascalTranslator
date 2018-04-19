using System;

namespace Compiler
{
	public class SimpleExpression : SyntaxTreeNode
	{
		public SimpleExpression (Token token)
			: base(token)
		{}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}

