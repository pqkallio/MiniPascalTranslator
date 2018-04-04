using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents a string value in the AST
	/// </summary>
	public class StringValueNode : IExpressionNode, ISemanticCheckValue
	{
		private string value;
		private Token token;

		public StringValueNode (string value)
		{
			this.value = value;
			this.token = new Token (0, 0, "", TokenType.STRING_VAL);
		}

		public StringValueNode (string value, Token t)
		{
			this.value = value;
			this.token = t;
		}

		public TokenType EvaluationType
		{
			get { return TokenType.STRING_VAL; }
			set { }
		}

		public string Value {
			get { return this.value; }
			set { this.value = value; }
		}

		public IExpressionNode[] GetExpressions()
		{
			return null;
		}

		public TokenType Operation
		{
			get { return TokenType.BINARY_OP_NO_OP; }
			set { }
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitStringValueNode (this);
		}

		public IProperty asProperty ()
		{
			return new StringProperty(Value);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}

		public override string ToString ()
		{
			return '\"' + Value + '\"';
		}
	}
}

