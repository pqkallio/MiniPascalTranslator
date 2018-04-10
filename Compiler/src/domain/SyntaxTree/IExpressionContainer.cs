using System;
using System.Collections.Generic;

namespace Compiler
{
	public interface IExpressionContainer : ISyntaxTreeNode
	{
		void AddExpression(IExpressionNode expressionNode);
	}
}

