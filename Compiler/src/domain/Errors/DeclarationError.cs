using System;

namespace Compiler
{
	/// <summary>
	/// Used to report an error when a variable is beaing declared multiple times.
	/// </summary>
	public class DeclarationError : SemanticError
	{
		public DeclarationError (SyntaxTreeNode node)
			:base(ErrorConstants.DECLARATION_ERROR_MESSAGE, node)
		{
		}
	}
}

