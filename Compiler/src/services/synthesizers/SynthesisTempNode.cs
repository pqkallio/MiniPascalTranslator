using System;

namespace Compiler
{
	public class SynthesisTempNode : SyntaxTreeNode
	{
		public SynthesisTempNode ()
			: base (new Token (0, 0))
		{}

		public override void Accept(INodeVisitor visitor) {}
	}
}

