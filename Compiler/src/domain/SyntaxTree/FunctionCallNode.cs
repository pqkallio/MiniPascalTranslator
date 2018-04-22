using System;
using System.Collections.Generic;

namespace Compiler
{
	public class FunctionCallNode : Evaluee
	{
		private VariableIdNode idNode;
		private ArgumentsNode argumentsNode;

		public FunctionCallNode (Token token, VariableIdNode idNode, INameFactory nameFactory, ArgumentsNode argumentsNode = null)
			: base(token, nameFactory)
		{
			this.idNode = idNode;
			this.argumentsNode = argumentsNode;
		}

		public ArgumentsNode ArgumentsNode
		{
			get { return argumentsNode; }
			set { this.argumentsNode = value; }
		}

		public List<ExpressionNode> GetArguments ()
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

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}

