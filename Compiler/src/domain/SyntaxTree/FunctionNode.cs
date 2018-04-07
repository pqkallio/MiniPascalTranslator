using System;

namespace Compiler
{
	public class FunctionNode : ISyntaxTreeNode
	{
		private Token token;
		private BlockNode block;
		private FunctionNode sequitor;
		private ParametersNode parameters;
		private TokenType returnType;

		public FunctionNode (Token token, VariableIdNode idNode, ParametersNode parametersNode, BlockNode block)
		{
			this.token = token;
			this.block = block;
			this.sequitor = null;
			this.returnType = idNode.VariableType;
		}

		public ISemanticCheckValue Accept (INodeVisitor visitor)
		{
			return new VoidProperty ();
		}

		public Token Token { 
			get { return token; } 
			set { token = value; }
		}

		public FunctionNode Sequitor {
			get { return sequitor; }
			set { this.sequitor = value; }
		}
	}
}

