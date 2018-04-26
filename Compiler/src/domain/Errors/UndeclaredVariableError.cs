using System;

namespace Compiler
{
	public class UndeclaredVariableError : Error
	{
		public UndeclaredVariableError (SyntaxTreeNode node)
			: base(ErrorConstants.SEMANTIC_ERROR_TITLE, ErrorConstants.UNDECLARED_VARIABLE_ERROR_MSG, node)
		{
		}
	}
}

