using System;

namespace Compiler
{
	public class ReturnStatement : StatementNode
	{
		private ExpressionNode returnValue;

		public ReturnStatement (Token token, ExpressionNode returnValue = null)
			: base(token, returns: true)
		{
			this.returnValue = returnValue;
		}

		public override void Accept(INodeVisitor visitor)
		{
			visitor.VisitReturnStatement(this);
		}

		public ExpressionNode ReturnValue
		{
			get { return returnValue; }
		}

		public TokenType EvaluationType
		{
			get {
				if (returnValue == null) {
					return TokenType.VOID;
				}

				return returnValue.EvaluationType;
			}
		}
	}
}

