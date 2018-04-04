using System;

namespace Compiler
{
	/// <summary>
	/// A general class for errors.
	/// </summary>
	public class Error
	{
		private string title;			// the error's title
		private string errorMessage;	// the error message
		private Token token;			// the token where the error occured
		private ISyntaxTreeNode node;	// the node where the error occured

		public Error (string title, string errorMessage, ISyntaxTreeNode node)
		{
			this.title = title;
			this.errorMessage = errorMessage;
			this.node = node;
		}

		public Error (string title, string errorMessage, Token token)
		{
			this.title = title;
			this.errorMessage = errorMessage;
			this.token = token;
		}

		public string Title
		{
			get { return this.title; }
		}

		public string ErrorMessage
		{
			get { return this.errorMessage; }
		}

		public virtual Token Token
		{
			get {
					if (this.node != null) {
						return this.node.Token;
					}

					return this.token;
				}
		}

		public ISyntaxTreeNode Node {
			get { return this.Node; }
		}

		public override string ToString ()
		{
			return string.Format ("{0}: {1}", Title, ErrorMessage);
		}
	}
}

