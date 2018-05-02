using System;

namespace Compiler
{
	public abstract class Evaluee : StatementNode
	{
		protected TokenType evaluationType;
		protected bool isVariable;
		protected string subTotal;

		public Evaluee (Token token, INameFactory nameFactory = null, Scope scope = null, bool isVariable = false)
			: base(token, nameFactory, scope)
		{
			this.evaluationType = TokenType.UNDEFINED;
			this.isVariable = isVariable;
			this.subTotal = null;
		}

		public abstract TokenType EvaluationType {
			get;
		}

		public bool Variable
		{
			get { return this.isVariable; }
		}

		public string SubTotal
		{
			get { return this.subTotal; }
			set { this.subTotal = value; }
		}
	}
}

