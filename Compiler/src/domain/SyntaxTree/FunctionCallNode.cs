using System;
using System.Collections.Generic;

namespace Compiler
{
	public class FunctionCallNode : IIdentifierContainer, IExpressionNode
	{
		private Token token;
		private VariableIdNode idNode;
		private ArgumentsNode argumentsNode;

		public FunctionCallNode (Token token, VariableIdNode idNode, ArgumentsNode argumentsNode = null)
		{
			this.token = token;
			this.idNode = idNode;
			this.argumentsNode = argumentsNode;
		}

		public Token Token
		{
			get{ return token; }
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
	}
}

