using System;

namespace Compiler
{
	public abstract class Evaluee : StatementNode
	{
		protected TokenType evaluationType;
		protected bool isVariable;
		protected string subTotal;

		public Evaluee (Token token, Scope scope = null, bool isVariable = false)
			: base(token, scope: scope)
		{
			this.evaluationType = TokenType.UNDEFINED;
			this.isVariable = isVariable;
			this.subTotal = null;
		}

		public TokenType EvaluationType {
			get { return this.evaluationType; }
			set { this.evaluationType = value; }
		}

		public virtual bool Variable
		{
			get { return this.isVariable; }
		}

		public string SubTotal
		{
			get { return this.subTotal; }
			set { this.subTotal = value; }
		}

		public bool HasAlreadyBeenEvaluated
		{
			get { return evaluationType != TokenType.UNDEFINED; }
		}

		public virtual string VariableID
		{
			get { return null; }
		}
	}
}

