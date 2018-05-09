using System;

namespace Compiler
{
	public class FunctionNode : SyntaxTreeNode
	{
		private BlockNode block;
		private ParametersNode parameters;
		private TokenType returnType;
		private VariableIdNode idNode;

		public FunctionNode (Token token, VariableIdNode idNode, ParametersNode parametersNode, BlockNode block, Scope scope)
			: base(token, scope)
		{
			this.block = block;
			this.parameters = parametersNode;
			this.returnType = idNode != null ? idNode.VariableType : TokenType.UNDEFINED;
			this.idNode = idNode;
		}

		public override void Accept (INodeVisitor visitor)
		{
			visitor.VisitFunctionNode (this);
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

