﻿using System;

namespace Compiler
{
	public class ArrayAccessNode : SyntaxTreeNode, ISemanticCheckValue
	{
		private string arrayId;
		private ExpressionNode index;

		public ArrayAccessNode (string arrayId, Token token, Scope scope, ExpressionNode index)
			: base(token, scope: scope)
		{
			this.arrayId = arrayId;
			this.index = index;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}

		public Property asProperty ()
		{
			return new ErrorProperty ();
		}
	}
}

