using System;

namespace Compiler
{
	public class FunctionNode : SyntaxTreeNode
	{
		private BlockNode block;
		private ParametersNode parameters;
		private TokenType returnType;
		private VariableIdNode idNode;

		public FunctionNode (Token token, INameFactory nameFactory, VariableIdNode idNode, ParametersNode parametersNode, BlockNode block, Scope scope)
			: base(token, nameFactory, scope)
		{
			this.block = block;
			this.parameters = parametersNode;
			this.returnType = idNode.VariableType;
			this.idNode = idNode;
		}

		public override ISemanticCheckValue Accept (INodeVisitor visitor)
		{
			return visitor.VisitFunctionNode (this);
		}

		public virtual TokenType ReturnType
		{
			get { return returnType; }
			set { returnType = value; }
		}

		public BlockNode Block
		{
			get { return block; }
		}

		public ParametersNode Parameters
		{
			get { return parameters; }
		}

		public VariableIdNode IDNode
		{
			get { return this.idNode; }
		}
	}
}

