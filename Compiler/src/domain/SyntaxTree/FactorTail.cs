using System;

namespace Compiler
{
	public class FactorTail : SyntaxTreeNode
	{
		public FactorTail (Token token, Scope scope = null)
			: base(token, scope: scope)
		{}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return visitor.VisitFactorTail (this);
		}
	}
}

