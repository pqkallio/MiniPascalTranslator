using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents an integer value in the AST
	/// </summary>
	public class IntValueNode : Evaluee, ISemanticCheckValue
	{
		private int value;

		public IntValueNode (int value)
			: this(value, new Token (0, 0, "", TokenType.INTEGER_VAL))
		{}

		public IntValueNode (int value, Token token)
			: base (token, null)
		{
			this.value = value;
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

		public TokenType Operation 
		{
			get { return TokenType.BINARY_OP_NO_OP; }
			set { }
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitIntValueNode (this);
		}

		public Property asProperty ()
		{
			return new IntegerProperty ();
		}

		public override string ToString ()
		{
			return Value.ToString ();
		}
	}
}

