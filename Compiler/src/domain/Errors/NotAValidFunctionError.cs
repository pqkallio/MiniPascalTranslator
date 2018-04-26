using System;

namespace Compiler
{
	public class NotAValidFunctionError : Error
	{
		public NotAValidFunctionError (SyntaxTreeNode node)
			: base(ErrorConstants.SEMANTIC_ERROR_TITLE, ErrorConstants.NOT_A_VALID_FUNCTION_ERROR_MSG, node)
		{
		}
	}
}

