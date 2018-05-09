using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents an integer value in the AST
	/// </summary>
	public class IntValueNode : Evaluee
	{
		private string value;

		public IntValueNode (string value, Token token, Scope scope)
			: base (token, scope)
		{
			this.value = value;
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

		public override void Accept(INodeVisitor visitor) {
			visitor.VisitIntValueNode (this);
		}

		public override string ToString ()
		{
			return Value.ToString ();
		}
	}
}

