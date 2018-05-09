using System;

namespace Compiler
{
	public class TypeNode : SyntaxTreeNode
	{
		private TokenType type;
		private ExpressionNode arraySizeExpression;
		private TokenType arrayElementType;

		public TypeNode (Token token, TokenType type, ExpressionNode arraySizeExpression = null, TokenType arrayElementType = TokenType.UNDEFINED)
			: base(token)
		{
			this.type = type;
			this.arraySizeExpression = arraySizeExpression;
			this.arrayElementType = arrayElementType;
		}

		public TokenType PropertyType
		{
			get { return type; }
		}

		public ExpressionNode ArraySizeExpression
		{
			get { return arraySizeExpression; }
		}

		public TokenType ArrayElementType
		{
			get { return arrayElementType; }
		}

		public override void Accept(INodeVisitor visitor)
		{
			visitor.VisitTypeNode (this);
		}
	}
}

