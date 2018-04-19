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
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			foreach (Error e in parser.getErrors()) {
				Console.WriteLine (e);
			}
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}
	}
}

