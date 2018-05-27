using System;

namespace Compiler
{
	public class ExpressionNode : Evaluee
	{
		private SimpleExpression expression;
		private ExpressionTail tail;

		public ExpressionNode (Token token, Scope scope, SimpleExpression expression, ExpressionTail tail = null)
			: base(token, scope)
		{
			this.expression = expression;
			this.tail = tail;
		}

		public override void Accept(INodeVisitor visitor)
		{
			visitor.VisitExpressionNode (this);
		}

		public SimpleExpression SimpleExpression
		{
			get { return expression; }
		}

		public ExpressionTail ExpressionTail
		{
			get { return tail; }
		}

		public override bool Variable {
			get {
				bool variable = this.SimpleExpression.Term.Factor.FactorMain.Evaluee != null;
				variable &= this.SimpleExpression.Term.Factor.FactorMain.Evaluee.Variable;
				variable &= this.SimpleExpression.Term.Factor.FactorTail == null;
				variable &= this.SimpleExpression.Term.TermTail == null;
				variable &= this.SimpleExpression.Tail == null;
				variable &= this.ExpressionTail == null;

				return variable;
			}
		}

		public override string VariableID {
			get {
				if (this.Variable) {
					return this.SimpleExpression.Term.Factor.FactorMain.Evaluee.VariableID;
				}

				return null;
			}
		}
	}
}

