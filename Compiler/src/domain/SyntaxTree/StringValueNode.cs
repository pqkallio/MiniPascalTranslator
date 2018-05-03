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
			: base (token, null)
		{
			this.value = value;
		}

		public override TokenType EvaluationType
		{
			get { return TokenType.STRING_VAL; }
		}

		public string Value {
			get { return this.value; }
			set { this.value = value; }
		}

		public TokenType Operation
		{
			get { return TokenType.BINARY_OP_NO_OP; }
			set { }
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitStringValueNode (this);
		}

		public override string ToString ()
		{
			return '\"' + Value + '\"';
		}
	}
}

