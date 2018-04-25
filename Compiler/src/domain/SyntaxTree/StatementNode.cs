using System;

namespace Compiler
{
	public abstract class StatementNode : SyntaxTreeNode
	{
		private bool returns;

		public StatementNode (Token token, INameFactory nameFactory = null, Scope scope = null, bool returns = false)
			: base(token, nameFactory, scope)
		{
			this.returns = returns;
		}

		public bool Returns
		{
			get { return returns; }
			set { returns = value; }
		}
	}
}

