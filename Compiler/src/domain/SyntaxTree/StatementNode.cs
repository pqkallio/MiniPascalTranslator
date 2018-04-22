using System;

namespace Compiler
{
	public abstract class StatementNode : SyntaxTreeNode
	{
		public StatementNode (Token token, INameFactory nameFactory = null, Scope scope = null)
			: base(token, nameFactory, scope)
		{}
	}
}

