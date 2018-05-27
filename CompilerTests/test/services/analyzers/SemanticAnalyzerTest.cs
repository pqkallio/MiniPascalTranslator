using NUnit.Framework;
using System;
using Compiler;

namespace CompilerTests
{
	[TestFixture ()]
	public class SemanticAnalyzerTest
	{
		private Scanner sc;
		private Parser parser;
		private SemanticAnalyzer sa;

		private void Init(string[] input) {
			this.sc = new Scanner (input);
			this.parser = new Parser (sc);
			parser.Parse ();
			this.sa = new SemanticAnalyzer (parser.SyntaxTree);
			if (parser.getErrors ().Count == 0) {
				this.sa.Analyze ();
			}
		}

		private void checkScannerAndParserErrors (int scannerErrorCount, int parserErrorCount)
		{
			Assert.AreEqual (sc.getErrors ().Count, scannerErrorCount);
			Assert.AreEqual (parser.getErrors ().Count, parserErrorCount);
		}

		[Test ()]
		public void TestParameterIdsAreUnique ()
		{
			Init (SemanticAnalyzerTestInputs.parameterIdentifiersAreUnique);
			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void TestParameterIdsAreNotUnique ()
		{
			Init (SemanticAnalyzerTestInputs.parameterIdentifiersAreNotUnique);

			checkScannerAndParserErrors (0, 1);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void VariableDeclarationsAreNotUniqueWithinAScope1 ()
		{
			Init (SemanticAnalyzerTestInputs.variableDeclarationsAreNotUniqueWithinAScope1);

			checkScannerAndParserErrors (0, 1);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void VariableDeclarationsAreNotUniqueWithinAScope2 ()
		{
			Init (SemanticAnalyzerTestInputs.variableDeclarationsAreNotUniqueWithinAScope2);

			checkScannerAndParserErrors (0, 1);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void VariableDeclarationsAreUniqueWithinAScope1 ()
		{
			Init (SemanticAnalyzerTestInputs.variableDeclarationsAreUniqueWithinAScope1);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void VariableDeclarationsAreUniqueWithinAScope2 ()
		{
			Init (SemanticAnalyzerTestInputs.variableDeclarationsAreUniqueWithinAScope2);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void ArrayTypesDeclarationSizeEvaluatesToInteger ()
		{
			Init (SemanticAnalyzerTestInputs.arrayTypesDeclarationSizeEvaluatesToInteger);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void ArrayTypesDeclarationSizeDoesntEvaluateToInteger ()
		{
			Init (SemanticAnalyzerTestInputs.arrayTypesDeclarationSizeDoesntEvaluateToInteger);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test ()]
		public void ArrayAccessAllIsWell1 ()
		{
			Init (SemanticAnalyzerTestInputs.arrayAccessAllIsWell1);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void ArrayAccessAllIsWell2 ()
		{
			Init (SemanticAnalyzerTestInputs.arrayAccessAllIsWell2);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void ArrayAccessArrayNotDeclared ()
		{
			Init (SemanticAnalyzerTestInputs.arrayAccessArrayNotDeclared);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 2);
		}

		[Test ()]
		public void ArrayAccessNotAnArray ()
		{
			Init (SemanticAnalyzerTestInputs.arrayAccessNotAnArray);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test ()]
		public void ArrayAccessIndexNotInteger ()
		{
			Init (SemanticAnalyzerTestInputs.arrayAccessIndexNotInteger);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test ()]
		public void AssignmentAllIsWell ()
		{
			Init (SemanticAnalyzerTestInputs.assignmentAllIsWell);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void AssignmentVariableNotDeclared ()
		{
			Init (SemanticAnalyzerTestInputs.assignmentVariableNotDeclared);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 3);
		}

		[Test ()]
		public void AssignmentVariableandExpressionDontMatch ()
		{
			Init (SemanticAnalyzerTestInputs.assignmentVariableandExpressionDontMatch);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test ()]
		public void AssignmentVariableandExpressionMatch1 ()
		{
			Init (SemanticAnalyzerTestInputs.assignmentVariableandExpressionMatch1);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void AssignmentVariableandExpressionMatch2 ()
		{
			Init (SemanticAnalyzerTestInputs.assignmentVariableandExpressionMatch2);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void ProcedureCallOk ()
		{
			Init (SemanticAnalyzerTestInputs.procedureCallOk);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void ProcedureNotDeclared ()
		{
			Init (SemanticAnalyzerTestInputs.procedureNotDeclared);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 2);
		}

		[Test ()]
		public void FunctionCallNotToAFunction ()
		{
			Init (SemanticAnalyzerTestInputs.functionCallNotToAFunction);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}
	}
}

