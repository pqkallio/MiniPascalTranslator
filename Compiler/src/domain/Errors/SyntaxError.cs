using System;

namespace Compiler
{
	/// <summary>
	/// Used to report an error during syntax analysis
	/// </summary>
	public class SyntaxError : Error
	{
		private TokenType expectedType;		// the expected type
		private string[] expectationSet;	// the expected set of token in string form

		public SyntaxError (Token token, string[] expectationSet)
			: this(token, TokenType.UNDEFINED, expectationSet)
		{}

		public SyntaxError (Token token, TokenType expectedType)
			: this(token, expectedType, null)
		{}

		public SyntaxError (Token token, TokenType expectedType = TokenType.UNDEFINED, string[] expectationSet = null, string message = null) 
			: base(ErrorConstants.SYNTAX_ERROR_TITLE, message == null ? ErrorConstants.SYNTAX_ERROR_MESSAGE : message, token)
		{
			this.expectedType = expectedType;
			this.expectationSet = expectationSet;
		}

		public TokenType ExpectedType
		{
			get { return this.expectedType; }
		}

		public string[] ExpectationSet
		{
			get { return expectationSet; }
		}

		public override string ToString ()
		{
			string error = Title + ": ";

			if (ExpectationSet != null && ExpectationSet.Length != 0) {
				return error + formatExpectationSetString ();
			} else {
				return error + formatExpectedTokenString ();
			}
		}

		private string formatExpectationSetString ()
		{
			string expectation = "";

			if (ExpectationSet.Length == 1) {
				expectation += ExpectationSet [0];
			} else {
				int i;
				for (i = 0; i < ExpectationSet.Length - 2; i++) {
					expectation += ExpectationSet [i] + ", ";
				}
				expectation += ExpectationSet [i];
				expectation += " or " + ExpectationSet [i + 1];
			}

			return expectation + " expected";
		}

		private string formatExpectedTokenString ()
		{
			return StringFormattingConstants.TOKEN_TYPE_STRINGS [ExpectedType] + " expected";
		}
	}
}

