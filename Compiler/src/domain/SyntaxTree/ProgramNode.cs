using System;

namespace Compiler
{
	public class ProgramNode : ISyntaxTreeNode
	{
		private Token token;
		private FunctionNode functionNode;
		private BlockNode mainBlock;
		private Scope scope;

		public ProgramNode (Token token, FunctionNode functionNode, BlockNode mainBlock, Scope scope)
		{
			this.token = token;
			this.functionNode = functionNode;
			this.mainBlock = mainBlock;
			this.scope = scope;
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