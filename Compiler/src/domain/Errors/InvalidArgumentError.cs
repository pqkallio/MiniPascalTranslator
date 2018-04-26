using System;

namespace Compiler
{
	public class InvalidArgumentError : Error
	{
		public InvalidArgumentError (FunctionCallNode callNode, int position, TokenType parameterEvaluation = TokenType.UNDEFINED, TokenType argumentEvaluation  = TokenType.UNDEFINED)
			: base(ErrorConstants.SEMANTIC_ERROR_TITLE, ErrorConstants.INVALID_ARGUMENT_ERROR_MSG(position, parameterEvaluation, argumentEvaluation), callNode)
		{}
	}
}

