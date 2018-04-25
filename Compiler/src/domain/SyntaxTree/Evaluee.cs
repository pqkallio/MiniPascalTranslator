using System;

namespace Compiler
{
	public abstract class Evaluee : StatementNode
	{
		protected TokenType evaluationType;
		protected bool isVariable;

		public Evaluee (Token token, INameFactory nameFactory = null, Scope scope = null, bool isVariable = false)
			: base(token, nameFactory)
		{
			this.evaluationType = TokenType.UNDEFINED;
			this.isVariable = isVariable;
		}

		public abstract TokenType EvaluationType {
			get;
		}

		public bool Variable
		{
			get { return this.isVariable; }
		}
	}
}

