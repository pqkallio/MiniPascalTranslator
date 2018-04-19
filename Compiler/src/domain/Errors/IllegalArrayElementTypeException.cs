using System;

namespace Compiler
{
	public class IllegalArrayElementTypeException : MiniPascalException
	{
		public IllegalArrayElementTypeException (string message, Token token)
			: base(message, token: token)
		{}
	}
}

