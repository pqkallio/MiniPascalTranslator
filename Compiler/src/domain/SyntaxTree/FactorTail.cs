using System;

namespace Compiler
{
	public class FactorTail : SyntaxTreeNode
	{
		public FactorTail (Token token)
			: base(token)
		{
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}

