using System;

namespace Compiler
{
	public class RealValueNode : Evaluee, ISemanticCheckValue
	{
		private string value;

		public RealValueNode (string value, Token token, Scope scope)
			: base(token, scope)
		{
			this.value = value;
		}

		public string Value {
			get { return this.value; }
			set { this.value = value; }
		}

		public override void Accept(INodeVisitor visitor) {
			visitor.VisitRealValueNode(this);
		}

		public override string ToString ()
		{
			return Value.ToString ();
		}
	}
}

