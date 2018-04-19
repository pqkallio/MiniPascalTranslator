using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents a boolean value in the AST
	/// </summary>
	public class BoolValueNode : SyntaxTreeNode, ISemanticCheckValue
	{
		private bool value;

		public BoolValueNode (bool value)
			: this(value, new Token (0, 0, "", TokenType.BOOLEAN_VAL_FALSE))
		{}

		public BoolValueNode (bool value, Token t)
			: base(t)
		{
			this.value = value;
		}

		public Property asProperty()
		{
			return new BooleanProperty ();
		}

		public TokenType EvaluationType
		{
			get { return TokenType.BOOLEAN_VAL_FALSE; }
			set { }
		}

		public bool Value {
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
			return visitor.VisitBoolValueNode (this);
		}

		public override string ToString ()
		{
			return Value.ToString ();
		}
	}
}

