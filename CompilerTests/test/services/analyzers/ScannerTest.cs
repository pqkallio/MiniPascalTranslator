using NUnit.Framework;
using System;
using System.Collections.Generic;
using Compiler;

namespace CompilerTests
{
	[TestFixture ()]
	public class ScannerTest
	{
		private Scanner scanner;
		private List<Token> tokens;

		private void InitScanner (string[] testInput)
		{
			this.scanner = new Scanner (testInput);
		}

		private void tokenizeInput (string[] input, bool print=false) {
			InitScanner (input);

			this.tokens = new List<Token> ();
			Token token = null;

			while (token == null || token.Type != TokenType.END_OF_FILE) {
				if (print) Console.WriteLine (token);
				token = scanner.getNextToken (token);

				this.tokens.Add (token);
			}
		}

		private void checkErrorCount (int expected, bool print=false)
		{
			if (print) {
				foreach (Error e in scanner.getErrors()) {
					Console.WriteLine (e);
				}
			}
			Assert.AreEqual (scanner.getErrors ().Count, expected);
		}

		public void TestEmpty ()
		{
			tokenizeInput (ScannerTestInputs.empty);

			Assert.AreEqual (tokens.Count, 1);
			Assert.AreEqual (tokens [0].Type, TokenType.END_OF_FILE);
			checkErrorCount (0);
		}

		[Test]
		public void TestSingleTokens ()
		{
			Dictionary<string[], TokenType> singleTokens = ScannerTestInputs.singleTokens;
			foreach (string[] input in singleTokens.Keys) {
				tokenizeInput (input);
				Assert.AreEqual (tokens.Count, 2);
				Assert.AreEqual (tokens [0].Type, singleTokens [input]);
				checkErrorCount (0);
			}
		}

		[Test]
		public void TestEmptyString ()
		{
			tokenizeInput (ScannerTestInputs.emptyString);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.STRING_VAL);
			checkErrorCount (0);
		}

		[Test]
		public void TestValidString ()
		{
			tokenizeInput (ScannerTestInputs.validString);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.STRING_VAL);
			checkErrorCount (0);
		}

		[Test]
		public void TestEscapeCharsInString ()
		{
			tokenizeInput (ScannerTestInputs.escapeCharsInString);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.STRING_VAL);
			string val = tokens [0].Value;
			Console.WriteLine (val);
			Assert.IsTrue (val.StartsWith ("b\ra\n"));
			Assert.IsTrue (val.Contains ("\""));
			Assert.IsTrue (val.Contains ("\t"));
			checkErrorCount (0);
		}

		[Test]
		public void TestInvalidStringSpansMultipleLines ()
		{
			tokenizeInput (ScannerTestInputs.invalidStringSpansMultipleLines);
			checkErrorCount (1);
			Assert.AreEqual (scanner.getErrors () [0].GetType ().Name, "StringLiteralError");
		}

		[Test]
		public void TestInvalidStringNeverEnds ()
		{
			tokenizeInput (ScannerTestInputs.invalidStringNeverEnds);
			checkErrorCount (1, true);
			Assert.AreEqual (scanner.getErrors () [0].GetType ().Name, "StringLiteralError");
		}

		[Test]
		public void TestGcd ()
		{
			tokenizeInput (ScannerTestInputs.gcd);

			Assert.IsTrue (true);
		}

		[Test]
		public void TestWtf ()
		{
			tokenizeInput (ScannerTestInputs.wtf);

			Assert.IsTrue (true);
		}

		[Test]
		public void TestInvertedArrays ()
		{
			tokenizeInput (ScannerTestInputs.invertedArrays);

			Assert.IsTrue (true);
		}
	}
}

