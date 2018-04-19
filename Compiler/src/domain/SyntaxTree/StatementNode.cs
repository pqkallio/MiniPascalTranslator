using System;

namespace Compiler
{
	public abstract class StatementNode : SyntaxTreeNode
	{
		public StatementNode (Token token)
			: base(token)
		{}
	}
}

