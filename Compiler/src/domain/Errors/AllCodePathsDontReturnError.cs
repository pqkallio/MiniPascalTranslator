using System;

namespace Compiler
{
	public class AllCodePathsDontReturnError : Error
	{
		public AllCodePathsDontReturnError (FunctionNode node)
			: base(ErrorConstants.SEMANTIC_ERROR_TITLE, ErrorConstants.ALL_CODE_PATHS_DONT_RETURN_MSG, node) 
		{
		}
	}
}

