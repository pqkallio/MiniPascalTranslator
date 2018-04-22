using System;

namespace Compiler
{
	public class ArraySizeCheckNode : FactorTail
	{
		public ArraySizeCheckNode (Token token, Scope scope)
			: base(token, scope: scope)
		{}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}

