using System;

namespace Compiler
{
	public class ArraySizeCheckNode : FactorTail
	{
		private Scope scope;

		public ArraySizeCheckNode (Token token, Scope scope)
			: base(token)
		{
			this.scope = scope;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}

