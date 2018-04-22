using System;

namespace Compiler
{
	public abstract class Evaluee : StatementNode
	{
		public Evaluee (Token token, INameFactory nameFactory = null, Scope scope = null)
			: base(token, nameFactory)
		{}
	}
}

