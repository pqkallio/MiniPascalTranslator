using System;
using System.Collections.Generic;

namespace Compiler
{
	public interface IExpressionContainer
	{
		void AddExpression(IExpressionNode expressionNode);
	}
}

