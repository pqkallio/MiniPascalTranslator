using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents an integer value in the AST
	/// </summary>
	public class IntValueNode : IExpressionNode, ISemanticCheckValue
	{
		private int value;
		private Token token;

		public IntValueNode (int value)
			: this(value, new Token (0, 0, "", TokenType.INTEGER_VAL))
		{}

		public IntValueNode (int value, Token t)
		{
			this.value = value;
			this.token = t;
		}

		public TokenType EvaluationType
		{
			get { return TokenType.INTEGER_VAL; }
			set { }
		}

		public int Value {
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
			return visitor.VisitIntValueNode (this);
		}

		public IProperty asProperty ()
		{
			return new IntegerProperty(Value);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}

		public override string ToString ()
		{
			return Value.ToString ();
		}
	}
}

