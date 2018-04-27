﻿using System;
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

		private void CheckScopeContainsVariables (Scope scope, bool expected, params string[] ids)
		{
			foreach (string id in ids) {
				Assert.IsTrue (scope.ContainsKey (id) == expected);
			}
		}

		[Test]
		public void testProgramNameIsSet ()
		{
			InitParser (ParserTestInputs.emptyProgram);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (tree.ProgramName, "prog");
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
		public void TestRootScopeHasOneBlockScopeAsChild ()
		{
			InitParser (ParserTestInputs.emptyProgram);
			SyntaxTree tree = parser.Parse ();
			Assert.IsNotNull (tree.RootScope);
			Assert.AreEqual (tree.RootScope.Children.Count, 1);
		}

		[Test]
		public void TestOneFunction ()
		{
			InitParser (ParserTestInputs.oneFunction);
			parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestRootHasOneFunction ()
		{
			InitParser (ParserTestInputs.oneFunction);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (tree.Root.Functions.Count, 1);
			Assert.IsTrue (tree.Root.Functions.ContainsKey ("func"));
			Assert.AreEqual (tree.Root.Functions ["func"].ReturnType, TokenType.STRING_VAL);
		}

		[Test]
		public void TestRootScopeHasBlockAndFunctionScopesAsChild ()
		{
			InitParser (ParserTestInputs.oneFunction);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (tree.RootScope.Children.Count, 2);
		}

		[Test]
		public void TestOneProcedure ()
		{
			InitParser (ParserTestInputs.oneProcedure);
			parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestRootHasOneProcedure ()
		{
			InitParser (ParserTestInputs.oneProcedure);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (tree.Root.Functions.Count, 1);
			Assert.IsTrue (tree.Root.Functions.ContainsKey ("proc"));
			Assert.AreEqual (tree.Root.Functions ["proc"].ReturnType, TokenType.VOID);
		}

		[Test]
		public void TestRootScopeHasBlockAndProcedureScopesAsChild ()
		{
			InitParser (ParserTestInputs.oneProcedure);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (tree.RootScope.Children.Count, 2);
		}

		[Test]
		public void TestFunctionAndProcedure ()
		{
			InitParser (ParserTestInputs.functionAndProcedure);
			parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestRootHasOneFunctionAndOneProcedure ()
		{
			InitParser (ParserTestInputs.functionAndProcedure);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (tree.Root.Functions.Count, 2);
			Assert.IsTrue (tree.Root.Functions.ContainsKey ("proc"));
			Assert.AreEqual (tree.Root.Functions ["proc"].ReturnType, TokenType.VOID);
			Assert.IsTrue (tree.Root.Functions.ContainsKey ("func"));
			Assert.AreEqual (tree.Root.Functions ["func"].ReturnType, TokenType.REAL_VAL);
		}

		[Test]
		public void TestRootScopeHasBlockAndFunctionAndProcedureScopesAsChild ()
		{
			InitParser (ParserTestInputs.functionAndProcedure);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (tree.RootScope.Children.Count, 3);
		}

		[Test]
		public void TestFunctionsAndProceduresSeeEachOther ()
		{
			InitParser (ParserTestInputs.functionAndProcedure);
			SyntaxTree tree = parser.Parse ();
			CheckScopeContainsVariables (tree.Root.Functions ["proc"].Scope, true, "func");
			CheckScopeContainsVariables (tree.Root.Functions ["func"].Scope, true, "proc");
		}

		[Test]
		public void TestFunctionWithParams ()
		{
			InitParser (ParserTestInputs.functionWithParams);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
			CheckScopeContainsVariables (tree.Root.Functions ["func"].Scope, true, "i", "ii", "str", "ary");
			CheckScopeContainsVariables (tree.Root.Functions ["func"].Scope.Children[0], true, "i", "ii", "str", "ary", "x");
			CheckScopeContainsVariables (tree.Root.Functions ["func"].Scope, false, "y", "x");
		}

		[Test]
		public void TestFunctionAndBlockScopesNotVisibleToRoot ()
		{
			InitParser (ParserTestInputs.functionWithParams);
			SyntaxTree tree = parser.Parse ();
			CheckScopeContainsVariables (tree.RootScope, false, "i", "ii", "str", "ary", "x");
			CheckScopeContainsVariables (tree.RootScope, false, "y");
		}

		[Test]
		public void TestProcedureWithParams ()
		{
			InitParser (ParserTestInputs.procedureWithParams);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
			CheckScopeContainsVariables (tree.Root.Functions ["proc"].Scope, true, "i", "ii", "str", "ary");
		}

		[Test]
		public void TestDeclareInteger ()
		{
			InitParser (ParserTestInputs.declareInteger);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsTrue (prop.Assigned == false);
			Assert.IsTrue (prop.GetTokenType () == TokenType.INTEGER_VAL);
		}

		[Test]
		public void TestDeclareReal ()
		{
			InitParser (ParserTestInputs.declareReal);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsTrue (prop.Assigned == false);
			Assert.IsTrue (prop.GetTokenType () == TokenType.REAL_VAL);
		}

		[Test]
		public void TestDeclareString ()
		{
			InitParser (ParserTestInputs.declareString);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsTrue (prop.Assigned == false);
			Assert.IsTrue (prop.GetTokenType () == TokenType.STRING_VAL);
		}

		[Test]
		public void TestDeclareBoolean ()
		{
			InitParser (ParserTestInputs.declareBoolean);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsTrue (prop.Assigned == false);
			Assert.IsTrue (prop.GetTokenType () == TokenType.BOOLEAN_VAL);
		}

		[Test]
		public void TestDeclareArrayOfInteger ()
		{
			InitParser (ParserTestInputs.declareArrayOfInteger);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsTrue (prop.Assigned == true);
			Assert.IsTrue (prop.GetTokenType () == TokenType.TYPE_ARRAY);
			Assert.AreEqual (((ArrayProperty)prop).ArrayElementType, TokenType.TYPE_INTEGER);
		}

		[Test]
		public void TestDeclareArrayOfReal ()
		{
			InitParser (ParserTestInputs.declareArrayOfReal);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsTrue (prop.Assigned == true);
			Assert.IsTrue (prop.GetTokenType () == TokenType.TYPE_ARRAY);
			Assert.AreEqual (((ArrayProperty)prop).ArrayElementType, TokenType.TYPE_REAL);
		}

		[Test]
		public void TestDeclareArrayOfString ()
		{
			InitParser (ParserTestInputs.declareArrayOfString);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsTrue (prop.Assigned == true);
			Assert.IsTrue (prop.GetTokenType () == TokenType.TYPE_ARRAY);
			Assert.AreEqual (((ArrayProperty)prop).ArrayElementType, TokenType.TYPE_STRING);
		}

		[Test]
		public void TestDeclareArrayOfBoolean ()
		{
			InitParser (ParserTestInputs.declareArrayOfBoolean);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsTrue (prop.Assigned == true);
			Assert.IsTrue (prop.GetTokenType () == TokenType.TYPE_ARRAY);
			Assert.AreEqual (((ArrayProperty)prop).ArrayElementType, TokenType.TYPE_BOOLEAN);
		}

		[Test]
		public void TestDeclareMultipleInteger ()
		{
			InitParser (ParserTestInputs.declareMultipleInteger);
			parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareMultipleReal ()
		{
			InitParser (ParserTestInputs.declareMultipleReal);
			parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareMultipleString ()
		{
			InitParser (ParserTestInputs.declareMultipleString);
			parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareMultipleBoolean ()
		{
			InitParser (ParserTestInputs.declareMultipleBoolean);
			parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareMultipleArrayOfInteger ()
		{
			InitParser (ParserTestInputs.declareMultipleArrayOfInteger);
			parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareMultipleArrayOfReal ()
		{
			InitParser (ParserTestInputs.declareMultipleArrayOfReal);
			parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareMultipleArrayOfString ()
		{
			InitParser (ParserTestInputs.declareMultipleArrayOfString);
			parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestDeclareMultipleArrayOfBoolean ()
		{
			InitParser (ParserTestInputs.declareMultipleArrayOfBoolean);
			parser.Parse ();
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

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsFalse (prop.Assigned);
		}

		[Test]
		public void TestDeclareAndAssignReal ()
		{
			InitParser (ParserTestInputs.declareAndAssignReal);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsFalse (prop.Assigned);
		}

		[Test]
		public void TestDeclareAndAssignString ()
		{
			InitParser (ParserTestInputs.declareAndAssignString);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsFalse (prop.Assigned);
		}

		[Test]
		public void TestDeclareAndAssignBoolean ()
		{
			InitParser (ParserTestInputs.declareAndAssignBoolean);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsFalse (prop.Assigned);
		}

		[Test]
		public void TestDeclareAndAssignArrayOfInteger ()
		{
			InitParser (ParserTestInputs.declareAndAssignArrayOfInteger);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsTrue (prop.Assigned);
		}

		[Test]
		public void TestDeclareAndAssignArrayOfReal ()
		{
			InitParser (ParserTestInputs.declareAndAssignArrayOfReal);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsTrue (prop.Assigned);
		}

		[Test]
		public void TestDeclareAndAssignArrayOfString ()
		{
			InitParser (ParserTestInputs.declareAndAssignArrayOfString);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsTrue (prop.Assigned);
		}

		[Test]
		public void TestDeclareAndAssignArrayOfBoolean ()
		{
			InitParser (ParserTestInputs.declareAndAssignArrayOfBoolean);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsTrue (prop.Assigned);
		}

		[Test]
		public void TestDeclareAndAssignFunctionCall1 ()
		{
			InitParser (ParserTestInputs.declareAndAssignFunctionCall1);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsFalse (prop.Assigned);
		}

		[Test]
		public void TestDeclareAndAssignFunctionCall2 ()
		{
			InitParser (ParserTestInputs.declareAndAssignFunctionCall2);
			SyntaxTree tree = parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);

			Property prop = tree.Root.MainBlock.Scope.GetProperty ("x", false);
			Assert.IsFalse (prop.Assigned);
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

		[Test]
		public void TestValidIdDeclarations ()
		{
			InitParser (ParserTestInputs.validIdDeclarations);
			parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}

		[Test]
		public void TestInvalidIdDeclarations ()
		{
			InitParser (ParserTestInputs.invalidIdDeclarations);
			parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, ScannerConstants.KEYWORDS.Count);
		}

		[Test]
		public void TestDoubleDeclarationWithinScope ()
		{
			InitParser (ParserTestInputs.doubleDeclarationWithinScope);
			parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 1);
		}

		[Test]
		public void TestDoubleDeclarationNestedScopes ()
		{
			InitParser (ParserTestInputs.doubleDeclarationNestedScopes);
			parser.Parse ();
			Assert.AreEqual (scanner.getErrors ().Count, 0);
			Assert.AreEqual (parser.getErrors ().Count, 0);
		}
	}
}

