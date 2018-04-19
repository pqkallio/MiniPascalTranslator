using System;

namespace Compiler
{
	public class MiniPascalException : Exception
	{
		private Token token;

		public MiniPascalException (string message, Token token)
			: base(message)
		{
			this.token = token;
		}

		public Token Token
		{
			get { return this.token; }
		}
	}
}

