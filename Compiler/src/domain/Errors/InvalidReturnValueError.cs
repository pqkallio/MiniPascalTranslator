using System;

namespace Compiler
{
	public class InvalidReturnValueError : Error
	{
		public InvalidReturnValueError (ReturnStatement node, TokenType requiredType)
			: base(ErrorConstants.INVALID_RETURN_VALUE_TITLE, ErrorConstants.INVALID_RETURN_VALUE_MSG(node, requiredType), node)
		{
		}
	}
}

