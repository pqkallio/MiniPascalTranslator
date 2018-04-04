using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents a boolean value in the AST
	/// </summary>
	public class BoolValueNode : IExpressionNode, ISemanticCheckValue
	{
		private bool value;
		private Token token;

		public BoolValueNode (bool value)
			: this(value, new Token (0, 0, "", TokenType.BOOLEAN_VAL_FALSE))
		{}

		public BoolValueNode (bool value, Token t)
		{
			this.value = value;
			this.token = t;
		}

		public TokenType EvaluationType
		{
			get { return TokenType.BOOLEAN_VAL_FALSE; }
			set { }
		}

		public bool Value {
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
			return visitor.VisitBoolValueNode (this);
		}

		public IProperty asProperty ()
		{
			return new BooleanProperty(Value);
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

