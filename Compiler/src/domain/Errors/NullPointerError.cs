using System;

namespace Compiler
{
	/// <summary>
	/// Used to report an error when a null pointer is encountered.
	/// </summary>
	public class NullPointerError : SemanticError
	{
		public NullPointerError (ISyntaxTreeNode node)
			: base(ErrorConstants.NULL_POINTER_ERROR_MESSAGE, node)
		{}
	}
}

