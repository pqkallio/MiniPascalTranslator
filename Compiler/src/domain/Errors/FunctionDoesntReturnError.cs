using System;

namespace Compiler
{
	public class FunctionDoesntReturnError : Error
	{
		public FunctionDoesntReturnError (FunctionNode node)
			: base(ErrorConstants.SEMANTIC_ERROR_TITLE, ErrorConstants.FUNCTION_DOESNT_RETURN_MSG, node)
		{
		}
	}
}

