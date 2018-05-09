using System;

namespace Compiler
{
	public abstract class VariableEvaluee : Evaluee
	{
		public VariableEvaluee (Token token, Scope scope)
			: base (token, scope, true)
		{}

		public abstract VariableIdNode IDNode {
			get;
		}
	}
}

