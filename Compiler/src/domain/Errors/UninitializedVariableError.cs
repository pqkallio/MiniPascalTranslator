using System;

namespace Compiler
{
	/// <summary>
	/// Used to report an error when an uninitialized variable is accessed. 
	/// </summary>
	public class UninitializedVariableError : SemanticError
	{
		public UninitializedVariableError (ISyntaxTreeNode node)
			: base(ErrorConstants.UNINITIALIZED_VARIABLE_ERROR_MESSAGE, node)
		{}
	}
}

