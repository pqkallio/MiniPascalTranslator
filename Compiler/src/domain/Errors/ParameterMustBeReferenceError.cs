using System;

namespace Compiler
{
	public class ParameterMustBeReferenceError : Error
	{
		public ParameterMustBeReferenceError (ParametersNode node, TokenType type)
			: base(ErrorConstants.PARAM_ERROR_TITLE, ErrorConstants.PARAM_REFERENCE_MSG(type), node)
		{
		}
	}
}

