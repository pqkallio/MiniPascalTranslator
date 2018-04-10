using System;
using System.Collections.Generic;

namespace Compiler
{
	public class ArgumentsNode : ISyntaxTreeNode
	{
		private List<IExpressionNode> arguments;
		private Scope scope;
		private Token token;

		public ArgumentsNode (Token token, Scope scope, params IExpressionNode[] expressions)
		{
			this.token = token;
			this.scope = scope;
			AddExpressions (expressions);
		}

		private void AddExpressions (IExpressionNode[] expressions)
		{
			this.arguments = new List<IExpressionNode> ();

			for (int i = 0; i < expressions.Length; i++) {
				this.arguments.Add (expressions [i]);
			}
		}

		public List<IExpressionNode> Arguments
		{
			get { return arguments; }
			set { this.arguments = value; }
		}

		public void AddArgument(IExpressionNode expressionNode)
		{
			this.arguments.Add (expressionNode);
		}

		public List<IExpressionNode> GetArguments ()
		{
			return arguments;
		}
	}
}

