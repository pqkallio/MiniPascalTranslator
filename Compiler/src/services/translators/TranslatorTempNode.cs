using System;

namespace Compiler
{
	public class TranslatorTempNode : SyntaxTreeNode
	{
		public TranslatorTempNode ()
			: base (new Token (0, 0))
		{}

		public override void Accept(INodeVisitor visitor) {}
	}
}

