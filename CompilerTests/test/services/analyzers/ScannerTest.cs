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
			checkErrorCount (2, true);
			Assert.AreEqual (scanner.getErrors () [0].GetType ().Name, "StringLiteralError");
			Assert.AreEqual (scanner.getErrors () [1].GetType ().Name, "StringLiteralError");
		}

		[Test]
		public void TestValidInteger1 ()
		{
			tokenizeInput (ScannerTestInputs.validInteger1);
			checkErrorCount (0);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.INTEGER_VAL);
		}

		[Test]
		public void TestValidInteger2 ()
		{
			tokenizeInput (ScannerTestInputs.validInteger2);
			checkErrorCount (0);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.INTEGER_VAL);
		}

		[Test]
		public void TestValidInteger3 ()
		{
			tokenizeInput (ScannerTestInputs.validInteger3);
			checkErrorCount (0);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.INTEGER_VAL);
		}

		[Test]
		public void TestSignAndInteger1 ()
		{
			tokenizeInput (ScannerTestInputs.signAndInteger1);
			checkErrorCount (0);
			Assert.AreEqual (tokens.Count, 3);
			Assert.AreEqual (tokens [0].Type, TokenType.SIGN_PLUS);
			Assert.AreEqual (tokens [1].Type, TokenType.INTEGER_VAL);
		}

		[Test]
		public void TestSignAndInteger2 ()
		{
			tokenizeInput (ScannerTestInputs.signAndInteger2);
			checkErrorCount (0);
			Assert.AreEqual (tokens.Count, 3);
			Assert.AreEqual (tokens [0].Type, TokenType.SIGN_MINUS);
			Assert.AreEqual (tokens [1].Type, TokenType.INTEGER_VAL);
		}

		[Test]
		public void TestValidReal1 ()
		{
			tokenizeInput (ScannerTestInputs.validReal1);
			checkErrorCount (0);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.REAL_VAL);
		}

		[Test]
		public void TestValidReal2 ()
		{
			tokenizeInput (ScannerTestInputs.validReal2);
			checkErrorCount (0);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.REAL_VAL);
		}

		[Test]
		public void TestValidReal3 ()
		{
			tokenizeInput (ScannerTestInputs.validReal3);
			checkErrorCount (0);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.REAL_VAL);
		}

		[Test]
		public void TestValidReal4 ()
		{
			tokenizeInput (ScannerTestInputs.validReal4);
			checkErrorCount (0);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.REAL_VAL);
		}

		[Test]
		public void TestValidReal5 ()
		{
			tokenizeInput (ScannerTestInputs.validReal5);
			checkErrorCount (0);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.REAL_VAL);
		}

		[Test]
		public void TestInvalidReal1 ()
		{
			tokenizeInput (ScannerTestInputs.invalidReal1);
			checkErrorCount (1);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.ERROR);
			Assert.AreEqual (scanner.getErrors () [0].GetType ().Name, "InvalidRealNumberError");
		}

		[Test]
		public void TestInvalidReal2 ()
		{
			tokenizeInput (ScannerTestInputs.invalidReal2);
			checkErrorCount (1);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.ERROR);
			Assert.AreEqual (scanner.getErrors () [0].GetType ().Name, "InvalidRealNumberError");
		}

		[Test]
		public void TestInvalidReal3 ()
		{
			tokenizeInput (ScannerTestInputs.invalidReal3);
			checkErrorCount (1);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.ERROR);
			Assert.AreEqual (scanner.getErrors () [0].GetType ().Name, "InvalidRealNumberError");
		}

		[Test]
		public void TestSignAndReal1 ()
		{
			tokenizeInput (ScannerTestInputs.signAndReal1);
			checkErrorCount (0);
			Assert.AreEqual (tokens.Count, 3);
			Assert.AreEqual (tokens [0].Type, TokenType.SIGN_PLUS);
			Assert.AreEqual (tokens [1].Type, TokenType.REAL_VAL);
		}

		[Test]
		public void TestSignAndReal2 ()
		{
			tokenizeInput (ScannerTestInputs.signAndReal2);
			checkErrorCount (0);
			Assert.AreEqual (tokens.Count, 3);
			Assert.AreEqual (tokens [0].Type, TokenType.SIGN_MINUS);
			Assert.AreEqual (tokens [1].Type, TokenType.REAL_VAL);
		}

		[Test]
		public void TestValidId1 ()
		{
			tokenizeInput (ScannerTestInputs.validId1);
			checkErrorCount (0);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.ID);
		}

		[Test]
		public void TestValidId2 ()
		{
			tokenizeInput (ScannerTestInputs.validId2);
			checkErrorCount (0);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.ID);
		}

		[Test]
		public void TestInvalidId1 ()
		{
			tokenizeInput (ScannerTestInputs.invalidId);
			checkErrorCount (1);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.ERROR);
			Assert.AreEqual (scanner.getErrors () [0].GetType ().Name, "InvalidIdentifierError");
		}

		[Test]
		public void TestErrorToken ()
		{
			tokenizeInput (ScannerTestInputs.errorToken);
			checkErrorCount (1);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.ERROR);
			Assert.AreEqual (scanner.getErrors () [0].GetType ().Name, "InvalidIdentifierError");
		}

		[Test]
		public void TestSize ()
		{
			tokenizeInput (ScannerTestInputs.size);
			checkErrorCount (0);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.SIZE);
		}

		[Test]
		public void TestDot1 ()
		{
			tokenizeInput (ScannerTestInputs.dot1);
			checkErrorCount (0);
			Assert.AreEqual (tokens.Count, 2);
			Assert.AreEqual (tokens [0].Type, TokenType.DOT);
		}

		[Test]
		public void TestDot2 ()
		{
			tokenizeInput (ScannerTestInputs.dot2);
			checkErrorCount (0);
			Assert.AreEqual (tokens.Count, 3);
			Assert.AreEqual (tokens [0].Type, TokenType.DOT);
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

