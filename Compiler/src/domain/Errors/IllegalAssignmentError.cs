using System;

namespace Compiler
{
	/// <summary>
	/// Used to report an error when an illegal assignment to a variable is made.
	/// </summary>
	public class IllegalAssignmentError : SemanticError
	{
		public IllegalAssignmentError (ISyntaxTreeNode node)
			: base(ErrorConstants.ILLEGAL_ASSIGNMENT_ERROR_MESSAGE, node)
		{}
	}
}

