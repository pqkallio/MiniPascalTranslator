using System;

namespace Compiler
{
	public class FactorMain : SyntaxTreeNode
	{
		private Evaluee evaluee;

		public FactorMain (Token token, Evaluee evaluee)
			: base(token)
		{
			this.evaluee = evaluee;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}

