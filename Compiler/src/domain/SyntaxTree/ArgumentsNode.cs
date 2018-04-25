using System;
using System.Collections.Generic;

namespace Compiler
{
	public class ArgumentsNode : SyntaxTreeNode
	{
		private List<ExpressionNode> arguments;

		public ArgumentsNode (Token token, Scope scope, List<ExpressionNode> expressions)
			: base(token, scope: scope)
		{
			this.arguments = expressions;
		}

		public ArgumentsNode (Token token, Scope scope, params ExpressionNode[] expressions)
			: base (token)
		{
			this.scope = scope;
			AddExpressions (expressions);
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}

		private void AddExpressions (ExpressionNode[] expressions)
		{
			this.arguments = new List<ExpressionNode> ();

			for (int i = 0; i < expressions.Length; i++) {
				this.arguments.Add (expressions [i]);
			}
		}

		public List<ExpressionNode> Arguments
		{
			get { return arguments; }
			set { this.arguments = value; }
		}

		public void AddArgument(ExpressionNode expressionNode)
		{
			this.arguments.Add (expressionNode);
		}

		public List<ExpressionNode> GetArguments ()
		{
			return arguments;
		}
	}
}

