using System;

namespace Compiler
{
	/// <summary>
	/// Used to report a Semantic error
	/// </summary>
	public class SemanticError : Error, ISemanticCheckValue
	{
		public SemanticError (string title, string message, SyntaxTreeNode node = null, Token token = null)
			: base (title, message, node, token)
		{}

		public SemanticError (string message, SyntaxTreeNode node)
			: this (ErrorConstants.SEMANTIC_ERROR_TITLE, message, node)
		{}

		public SemanticError (string message)
			: this (ErrorConstants.SEMANTIC_ERROR_TITLE, message)
		{}

		public SemanticError (SyntaxTreeNode node)
			: this (ErrorConstants.SEMANTIC_ERROR_TITLE, ErrorConstants.SEMANTIC_ERROR_MESSAGE, node)
		{}

		public Property asProperty ()
		{
			return null;
		}
	}
}

