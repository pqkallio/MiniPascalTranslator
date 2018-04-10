using System;

namespace Compiler
{
	public class BlockNode : ISyntaxTreeNode
	{
		private Token token;
		private Scope blockScope;
		StatementsNode statements;

		public BlockNode (Scope blockScope, StatementsNode statements)
		{
			this.blockScope = blockScope;
			this.statements = statements;
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return new VoidProperty ();
		}

		public Token Token { 
			get { return token; }
			set { token = value; } 
		}
	}
}

