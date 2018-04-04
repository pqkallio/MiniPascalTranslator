using System;

namespace Compiler
{
	/// <summary>
	/// Used to throw an exception in runtime.
	/// </summary>
	public class RuntimeException : Exception
	{
		private Token token;

		public RuntimeException (string message, Token token)
			: base (message)
		{
			this.token = token;
		}

		public Token Token
		{
			get { return this.token; }
		}

		public override string ToString ()
		{
			return string.Format ("{0}: {1}", ErrorConstants.RUNTIME_EXCEPTION_TITLE, Message);
		}
	}
}

