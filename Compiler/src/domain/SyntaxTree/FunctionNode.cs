using System;

namespace Compiler
{
	public class FunctionNode : SyntaxTreeNode
	{
		private BlockNode block;
		private ParametersNode parameters;
		private TokenType returnType;

		public FunctionNode (Token token, INameFactory nameFactory, VariableIdNode idNode, ParametersNode parametersNode, BlockNode block, Scope scope)
			: base(token, nameFactory, scope)
		{
			this.block = block;
			this.returnType = idNode.VariableType;
		}

		public override ISemanticCheckValue Accept (INodeVisitor visitor)
		{
			return new VoidProperty ();
		}

		public virtual TokenType ReturnType
		{
			get { return returnType; }
			set { returnType = value; }
		}
	}
}

