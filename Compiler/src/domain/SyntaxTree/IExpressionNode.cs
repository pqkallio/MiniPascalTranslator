using System;

namespace Compiler
{
	public interface IExpressionNode
	{
		IExpressionNode[] GetExpressions ();
		TokenType Operation { 
			get;
			set;
		}
		TokenType EvaluationType {
			get;
			set;
		}
	}
}

