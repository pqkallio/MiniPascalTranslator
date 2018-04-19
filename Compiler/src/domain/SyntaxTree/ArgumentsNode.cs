using System;
using System.Collections.Generic;

namespace Compiler
{
	public class ArgumentsNode : SyntaxTreeNode
	{
		private List<IExpressionNode> arguments;
		private Scope scope;

		public ArgumentsNode (Token token, Scope scope, List<IExpressionNode> expressions)
			: base(token)
		{
			this.scope = scope;
			this.arguments = expressions;
		}

		public ArgumentsNode (Token token, Scope scope, params IExpressionNode[] expressions)
			: base (token)
		{
			this.scope = scope;
			AddExpressions (expressions);
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
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

