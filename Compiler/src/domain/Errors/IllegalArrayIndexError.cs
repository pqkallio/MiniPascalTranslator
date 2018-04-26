using System;

namespace Compiler
{
	public class IllegalArrayIndexError : Error
	{
		public IllegalArrayIndexError (SyntaxTreeNode node)
			: base(ErrorConstants.SEMANTIC_ERROR_TITLE, ErrorConstants.ILLEGAL_ARRAY_INDEX_ERROR_MSG, node)
		{
		}
	}
}

