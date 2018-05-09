using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents a boolean value in the AST
	/// </summary>
	public class BoolValueNode : Evaluee
	{
		private bool value;

		public BoolValueNode (bool value, Token token, Scope scope)
			: base(token, scope)
		{
			this.value = value;
		}

		public bool Value {
			get { return this.value; }
			set { this.value = value; }
		}

		public override void Accept(INodeVisitor visitor) {
			visitor.VisitBoolValueNode (this);
		}

		public override string ToString ()
		{
			return Value.ToString ();
		}
	}
}

