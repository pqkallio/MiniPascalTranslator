﻿using System;

namespace Compiler
{
	public class ExpressionNode : Evaluee
	{
		private SimpleExpression expression;
		private ExpressionTail tail;

		public ExpressionNode (Token token, SimpleExpression expression, ExpressionTail tail = null)
			: base(token)
		{
			this.expression = expression;
			this.tail = tail;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}
	}
}
