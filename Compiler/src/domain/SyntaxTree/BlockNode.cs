using System;

namespace Compiler
{
	public class BlockNode : ISyntaxTreeNode
	{
		private Token token;

		public BlockNode ()
		{}

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

