using System;
using System.Collections.Generic;

namespace Compiler
{
	public class FunctionCallNode : SyntaxTreeNode
	{
		private VariableIdNode idNode;
		private ArgumentsNode argumentsNode;

		public FunctionCallNode (Token token, VariableIdNode idNode, ArgumentsNode argumentsNode = null)
			: base(token)
		{
			this.idNode = idNode;
			this.argumentsNode = argumentsNode;
		}

		public ArgumentsNode ArgumentsNode
		{
			get { return argumentsNode; }
			set { this.argumentsNode = value; }
		}

		public List<IExpressionNode> GetArguments ()
		{
			if (ArgumentsNode == null) {
				return null;
			}

			return ArgumentsNode.GetArguments ();
		}

		public VariableIdNode IdNode
		{
			get { return idNode; }
			set { idNode = value; }
		}

		public TokenType EvaluationType {
			get { return idNode.EvaluationType; }
			set {  }
		}

		public IExpressionNode[] GetExpressions ()
		{
			return null;
		}

		public TokenType Operation { 
			get { return TokenType.BINARY_OP_NO_OP; }
			set { }
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}

