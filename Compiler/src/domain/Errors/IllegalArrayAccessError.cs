using System;

namespace Compiler
{
	public class IllegalArrayAccessError : Error
	{
		public IllegalArrayAccessError (SyntaxTreeNode node)
			: base(ErrorConstants.SEMANTIC_ERROR_TITLE, ErrorConstants.ILLEGAL_ARRAY_ACCESS_ERROR_MSG, node)
		{}
	}
}

