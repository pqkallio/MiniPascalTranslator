using System;

namespace Compiler
{
	public class ProgramNode : ISyntaxTreeNode
	{
		Token token;
		VariableIdNode idNode;
		FunctionNode functionNode;
		BlockNode mainBlock;

		public ProgramNode (Token token, VariableIdNode idNode, FunctionNode functionNode, BlockNode mainBlock)
		{
			this.token = token;
			this.idNode = idNode;
			this.functionNode = functionNode;
			this.mainBlock = mainBlock;
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