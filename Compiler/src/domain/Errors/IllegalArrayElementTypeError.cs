using System;

namespace Compiler
{
	public class IllegalArrayElementTypeError : SemanticError
	{
		public IllegalArrayElementTypeError ()
			: base(ErrorConstants.ILLEGAL_ARRAY_ELEMENT_TYPE_ERROR_MSG)
		{}
	}
}

