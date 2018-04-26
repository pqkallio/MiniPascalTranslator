using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents a boolean value in the AST
	/// </summary>
	public class BoolValueNode : Evaluee, ISemanticCheckValue
	{
		private bool value;

		public BoolValueNode (bool value)
			: this(value, new Token (0, 0, "", TokenType.BOOLEAN_VAL_FALSE))
		{}

		public BoolValueNode (bool value, Token token)
			: base(token)
		{
			this.value = value;
		}

		public override TokenType EvaluationType
		{
			get { return TokenType.BOOLEAN_VAL; }
		}

		public bool Value {
			get { return this.value; }
			set { this.value = value; }
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitBoolValueNode (this);
		}

		public Property asProperty ()
		{
			return new BooleanProperty (token.Row);
		}

		public override string ToString ()
		{
			return Value.ToString ();
		}
	}
}

