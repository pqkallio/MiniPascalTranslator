using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents an integer value in the AST
	/// </summary>
	public class IntValueNode : Evaluee, ISemanticCheckValue
	{
		private string value;

		public IntValueNode (string value)
			: this(value, new Token (0, 0, "", TokenType.INTEGER_VAL))
		{}

		public IntValueNode (string value, Token token)
			: base (token, null)
		{
			this.value = value;
		}

		public override TokenType EvaluationType
		{
			get { return TokenType.INTEGER_VAL; }
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
			return visitor.VisitIntValueNode (this);
		}

		public override string ToString ()
		{
			return Value.ToString ();
		}
	}
}

