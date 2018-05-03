using System;

namespace Compiler
{
	public class RealValueNode : Evaluee, ISemanticCheckValue
	{
		private string value;

		public RealValueNode (string value, Token token)
			: base(token)
		{
			this.value = value;
		}

		public override TokenType EvaluationType
		{
			get { return TokenType.REAL_VAL; }
		}

		public string Value {
			get { return this.value; }
			set { this.value = value; }
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return null;
		}

		public override string ToString ()
		{
			return Value.ToString ();
		}
	}
}

