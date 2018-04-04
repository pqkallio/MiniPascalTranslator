using System;

namespace Compiler
{
	public interface IExpressionContainer : ISyntaxTreeNode
	{
		void AddExpression(IExpressionNode expressionNode);
	}
}

