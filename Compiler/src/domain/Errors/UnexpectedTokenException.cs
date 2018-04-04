using System;

namespace Compiler
{
	/// <summary>
	/// Used to throw an exception when an unexpected token is encountered while parsing.
	/// </summary>
	public class UnexpectedTokenException : Exception
	{
		private Token token;
		private string[] expectationSet;
		private TokenType expectedType;

		public UnexpectedTokenException ()
		{}

		public UnexpectedTokenException (string message)
			: base(message)
		{}

		public UnexpectedTokenException(Token token, TokenType expectedType)
			: this("Unexpected token")
		{
			this.token = token;
			this.expectedType = expectedType;
		}

		public UnexpectedTokenException (Token token, string[] expectationSet)
			: this("Unexpected token")
		{
			this.token = token;
			this.expectationSet = expectationSet;
			this.expectedType = TokenType.UNDEFINED;
		}

		public UnexpectedTokenException(Token token, TokenType expectedType, string[] expectationSet)
			: this("Unexpected token")
		{
			this.token = token;
			this.expectationSet = expectationSet;
			this.expectedType = expectedType;
		}

		public Token Token
		{
			get { return token; }
		}

		public TokenType ExpectedType
		{
			get { return expectedType; }
		}

		public string[] ExpectationSet
		{
			get { return expectationSet; }
		}
	}
}

