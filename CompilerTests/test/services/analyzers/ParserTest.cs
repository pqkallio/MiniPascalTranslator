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

		private void CheckScopeContainsVariables (Scope scope, params string[] ids)
		{
			foreach (string id in ids) {
				Assert.IsTrue (scope.ContainsKey (id));
			}
		}

		[Test]
		public void TestEmptyProgram ()
		{
			InitParser (ParserTestInputs.emptyProgram);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
			Assert.AreEqual (tree.ProgramName, "prog");
			Assert.AreEqual (tree.RootScope.Children.Count, 1);
		}

		[Test]
		public void TestOneFunction ()
		{
			InitParser (ParserTestInputs.oneFunction);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
			Assert.IsTrue (tree.Root.Functions.ContainsKey ("func"));
			Assert.AreEqual (tree.Root.Functions.Count, 1);
			Assert.AreEqual (tree.RootScope.Children.Count, 2);
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
			CheckScopeContainsVariables (tree.Root.Functions ["func"].Scope, "i", "ii", "str", "ary");
		}

		[Test]
		public void TestProcedureWithParams ()
		{
			InitParser (ParserTestInputs.procedureWithParams);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
			CheckScopeContainsVariables (tree.Root.Functions ["proc"].Scope, "i", "ii", "str", "ary");
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

		[Test]
		public void TestDeclareMultipleInteger ()
		{
			InitParser (ParserTestInputs.declareMultipleInteger);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareMultipleReal ()
		{
			InitParser (ParserTestInputs.declareMultipleReal);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareMultipleString ()
		{
			InitParser (ParserTestInputs.declareMultipleString);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareMultipleBoolean ()
		{
			InitParser (ParserTestInputs.declareMultipleBoolean);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareMultipleArrayOfInteger ()
		{
			InitParser (ParserTestInputs.declareMultipleArrayOfInteger);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareMultipleArrayOfReal ()
		{
			InitParser (ParserTestInputs.declareMultipleArrayOfReal);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareMultipleArrayOfString ()
		{
			InitParser (ParserTestInputs.declareMultipleArrayOfString);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareMultipleArrayOfBoolean ()
		{
			InitParser (ParserTestInputs.declareMultipleArrayOfBoolean);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareAndAssignInteger ()
		{
			InitParser (ParserTestInputs.declareAndAssignInteger);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareAndAssignReal ()
		{
			InitParser (ParserTestInputs.declareAndAssignReal);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareAndAssignString ()
		{
			InitParser (ParserTestInputs.declareAndAssignString);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareAndAssignBoolean ()
		{
			InitParser (ParserTestInputs.declareAndAssignBoolean);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareAndAssignArrayOfInteger ()
		{
			InitParser (ParserTestInputs.declareAndAssignArrayOfInteger);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareAndAssignArrayOfReal ()
		{
			InitParser (ParserTestInputs.declareAndAssignArrayOfReal);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareAndAssignArrayOfString ()
		{
			InitParser (ParserTestInputs.declareAndAssignArrayOfString);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareAndAssignArrayOfBoolean ()
		{
			InitParser (ParserTestInputs.declareAndAssignArrayOfBoolean);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareAndAssignFunctionCall1 ()
		{
			InitParser (ParserTestInputs.declareAndAssignFunctionCall1);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareAndAssignFunctionCall2 ()
		{
			InitParser (ParserTestInputs.declareAndAssignFunctionCall2);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestReturnStatement1 ()
		{
			InitParser (ParserTestInputs.returnStatement1);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestReturnStatement2 ()
		{
			InitParser (ParserTestInputs.returnStatement2);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestReadStatement ()
		{
			InitParser (ParserTestInputs.readStatement);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestWriteStatement ()
		{
			InitParser (ParserTestInputs.writeStatement);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestAssertStatement ()
		{
			InitParser (ParserTestInputs.assertStatement);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestBlockInABlockStatement1 ()
		{
			InitParser (ParserTestInputs.blockInABlockStatement1);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestBlockInABlockStatement2 ()
		{
			InitParser (ParserTestInputs.blockInABlockStatement2);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestIfThenStatement ()
		{
			InitParser (ParserTestInputs.ifThenStatement);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestIfThenElseStatement ()
		{
			InitParser (ParserTestInputs.ifThenElseStatement);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestWhileStatement ()
		{
			InitParser (ParserTestInputs.whileStatement);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestAccessArraySize ()
		{
			InitParser (ParserTestInputs.accessArraySize);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestProcedureCall1 ()
		{
			InitParser (ParserTestInputs.procedureCall1);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestProcedureCall2 ()
		{
			InitParser (ParserTestInputs.procedureCall2);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}
	}
}

