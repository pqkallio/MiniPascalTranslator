using System;

namespace Compiler
{
	public class IllegalArrayElementTypeError : SemanticError
	{
		public IllegalArrayElementTypeError (SyntaxTreeNode node)
			: base(node, ErrorConstants.ILLEGAL_ARRAY_ELEMENT_TYPE_ERROR_MSG)
		{}

		public IllegalArrayElementTypeError (Token token)
			: base(token, ErrorConstants.ILLEGAL_ARRAY_ELEMENT_TYPE_ERROR_MSG)
		{}
	}
}

