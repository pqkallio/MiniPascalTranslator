using System;

namespace Compiler
{
	/// <summary>
	/// Used to report a Semantic error
	/// </summary>
	public class SemanticError : Error, ISemanticCheckValue
	{
		public SemanticError (string title, string message, ISyntaxTreeNode node)
			: base (title, message, node)
		{}

		public SemanticError (string message, ISyntaxTreeNode node)
			: this (ErrorConstants.SEMANTIC_ERROR_TITLE, message, node)
		{}

		public SemanticError (ISyntaxTreeNode node)
			: this (ErrorConstants.SEMANTIC_ERROR_TITLE, ErrorConstants.SEMANTIC_ERROR_MESSAGE, node)
		{}

		public IProperty asProperty ()
		{
			return null;
		}
	}
}

