using System;

namespace Compiler
{
	public class TermNode : SyntaxTreeNode
	{
		private Factor factorNode;
		private TermTail termTailNode;

		public TermNode (Token token, Factor factorNode, TermTail termTailNode)
			: base(token)
		{}

		public IExpressionNode[] GetExpressions ()
		{
			return null;
		}

		TokenType Operation { 
			get { return TokenType.UNDEFINED; }
			set { }
		}
		TokenType EvaluationType {
			get { return TokenType.UNDEFINED; }
			set { }
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}

