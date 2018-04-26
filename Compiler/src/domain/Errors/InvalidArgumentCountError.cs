using System;

namespace Compiler
{
	public class InvalidArgumentCountError : Error
	{
		public InvalidArgumentCountError (SyntaxTreeNode node)
			: base(ErrorConstants.SEMANTIC_ERROR_TITLE, ErrorConstants.INVALID_ARG_COUNT_ERROR_MSG, node)
		{}
	}
}

