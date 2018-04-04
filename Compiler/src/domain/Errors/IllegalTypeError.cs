using System;

namespace Compiler
{
	/// <summary>
	/// Used to report an error when the type of a operation is illegal
	/// </summary>
	public class IllegalTypeError : SemanticError
	{
		public IllegalTypeError (ISyntaxTreeNode node)
			: base(ErrorConstants.ILLEGAL_TYPE_ERROR_MESSAGE, node)
		{}
	}
}

