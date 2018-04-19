using System;

namespace Compiler
{
	public class SimpleExpressionTail : SyntaxTreeNode
	{
		public SimpleExpressionTail (Token token)
			: base(token)
		{}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}

