using System;

namespace Compiler
{
	public class RealValueNode : Evaluee, ISemanticCheckValue
	{
		private float value;

		public RealValueNode (float value, Token token)
			: base(token)
		{
			this.value = value;
		}

		public TokenType EvaluationType
		{
			get { return TokenType.REAL_VAL; }
			set { }
		}

		public float Value {
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
			return null;
		}

		public Property asProperty ()
		{
			return new RealProperty();
		}

		public override string ToString ()
		{
			return Value.ToString ();
		}

		public override string GetValue ()
		{
			return Value.ToString ();
		}
	}
}

