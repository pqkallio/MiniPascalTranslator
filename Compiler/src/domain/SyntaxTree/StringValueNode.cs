using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents a string value in the AST
	/// </summary>
	public class StringValueNode : Evaluee, ISemanticCheckValue
	{
		private string value;

		public StringValueNode (string value)
			: this(value, new Token (0, 0, "", TokenType.STRING_VAL))
		{}

		public StringValueNode (string value, Token token)
			: base (token)
		{
			this.value = value;
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

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitStringValueNode (this);
		}

		public Property asProperty ()
		{
			return new StringProperty();
		}

		public override string ToString ()
		{
			return '\"' + Value + '\"';
		}

		public override string GetValue ()
		{
			return ToString ();
		}
	}
}

