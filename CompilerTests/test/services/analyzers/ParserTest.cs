using System;
using System.Collections.Generic;
using NUnit.Framework;
using Compiler;

namespace CompilerTests
{
	[TestFixture ()]
	public class ParserTest
	{
		private Scanner scanner;
		private Parser parser;

		private void InitScanner (string[] testInput)
		{
			this.scanner = new Scanner (testInput);
		}

		private void InitParser (string[] testInput)
		{
			InitScanner (testInput);
			this.parser = new Parser (scanner);
		}

		[Test]
		public void TestEmptyProgram ()
		{
			InitParser (ParserTestInputs.emptyProgram);
			parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestOneFunction ()
		{
			InitParser (ParserTestInputs.oneFunction);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestOneProcedure ()
		{
			InitParser (ParserTestInputs.oneProcedure);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestFunctionAndProcedure ()
		{
			InitParser (ParserTestInputs.functionAndProcedure);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestFunctionWithParams ()
		{
			InitParser (ParserTestInputs.functionWithParams);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareInteger ()
		{
			InitParser (ParserTestInputs.declareInteger);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareReal ()
		{
			InitParser (ParserTestInputs.declareReal);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareString ()
		{
			InitParser (ParserTestInputs.declareString);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareBoolean ()
		{
			InitParser (ParserTestInputs.declareBoolean);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareArrayOfInteger ()
		{
			InitParser (ParserTestInputs.declareArrayOfInteger);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			foreach (Error error in parser.getErrors()) {
				Console.WriteLine (error);
			}
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareArrayOfReal ()
		{
			InitParser (ParserTestInputs.declareArrayOfReal);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareArrayOfString ()
		{
			InitParser (ParserTestInputs.declareArrayOfString);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareArrayOfBoolean ()
		{
			InitParser (ParserTestInputs.declareArrayOfBoolean);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}
	}
}

