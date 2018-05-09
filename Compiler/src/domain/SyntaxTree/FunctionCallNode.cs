using System;
using System.Collections.Generic;

namespace Compiler
{
	public class FunctionCallNode : Evaluee
	{
		private VariableIdNode idNode;
		private ArgumentsNode argumentsNode;

		public FunctionCallNode (Token token, Scope scope, VariableIdNode idNode, ArgumentsNode argumentsNode = null)
			: base(token, scope)
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

		public override void Accept(INodeVisitor visitor)
		{
			visitor.VisitFunctionCallNode (this);
		}
	}
}

