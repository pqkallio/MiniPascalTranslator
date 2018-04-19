using System;

namespace Compiler
{
	public class FunctionNode : SyntaxTreeNode
	{
		private BlockNode block;
		private ParametersNode parameters;
		private TokenType returnType;
		private Scope scope;

		public FunctionNode (Token token, ILabelFactory labelFactory, VariableIdNode idNode, ParametersNode parametersNode, BlockNode block, Scope scope)
			: base(token, labelFactory)
		{
			this.block = block;
			this.returnType = idNode.VariableType;
			this.scope = scope;
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

