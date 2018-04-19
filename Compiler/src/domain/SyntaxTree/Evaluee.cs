using System;

namespace Compiler
{
	public abstract class Evaluee : SyntaxTreeNode
	{
		public Evaluee (Token token)
			: base(token)
		{}

		public abstract string GetValue();
	}
}

