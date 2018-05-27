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

		[TearDown ()]
		public void tearDown ()
		{
			this.sc = null;
			this.parser = null;
			this.sa = null;
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
		public void ParametersStringParamIsNotReference ()
		{
			Init (SemanticAnalyzerTestInputs.parametersStringParamIsNotReference);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
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

		[Test ()]
		public void FunctionCallValidArguments ()
		{
			Init (SemanticAnalyzerTestInputs.functionCallValidArguments);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void FunctionCallTooFewArguments ()
		{
			Init (SemanticAnalyzerTestInputs.functionCallTooFewArguments);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test ()]
		public void FunctionCallTooManyArguments ()
		{
			Init (SemanticAnalyzerTestInputs.functionCallTooManyArguments);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test ()]
		public void FunctionCallInvalidArgument ()
		{
			Init (SemanticAnalyzerTestInputs.functionCallInvalidArgument);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test ()]
		public void FunctionDoesntReturn1 ()
		{
			Init (SemanticAnalyzerTestInputs.functionDoesntReturn1);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 2);
		}

		[Test ()]
		public void FunctionDoesntReturn2 ()
		{
			Init (SemanticAnalyzerTestInputs.functionDoesntReturn2);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test ()]
		public void FunctionDoesntReturn3 ()
		{
			Init (SemanticAnalyzerTestInputs.functionDoesntReturn3);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test ()]
		public void FunctionDoesntReturn4 ()
		{
			Init (SemanticAnalyzerTestInputs.functionDoesntReturn4);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test ()]
		public void FunctionReturns1 ()
		{
			Init (SemanticAnalyzerTestInputs.functionReturns1);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void FunctionReturns2 ()
		{
			Init (SemanticAnalyzerTestInputs.functionReturns2);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void FunctionsReturnTypeIsInvalid ()
		{
			Init (SemanticAnalyzerTestInputs.functionsReturnTypeIsInvalid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test ()]
		public void FunctionsReturnTypesAreInvalid ()
		{
			Init (SemanticAnalyzerTestInputs.functionsReturnTypesAreInvalid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 3);
		}

		[Test ()]
		public void ProceduredReturnTypeIsValid ()
		{
			Init (SemanticAnalyzerTestInputs.proceduresReturnTypeIsValid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void ProceduresReturnTypeIsInvalid ()
		{
			Init (SemanticAnalyzerTestInputs.proceduresReturnTypeIsInvalid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test ()]
		public void ReadStatementOk ()
		{
			Init (SemanticAnalyzerTestInputs.readStatementOk);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void ReadStatementBooleanArgument ()
		{
			Init (SemanticAnalyzerTestInputs.readStatementBooleanArgument);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test ()]
		public void ReadStatementArrayArgument ()
		{
			Init (SemanticAnalyzerTestInputs.readStatementArrayArgument);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test ()]
		public void AssertStatementOk ()
		{
			Init (SemanticAnalyzerTestInputs.assertStatementOk);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void AssertStatementNotBooleanCheck ()
		{
			Init (SemanticAnalyzerTestInputs.assertStatementNotBooleanCheck);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test ()]
		public void IfStatementNotBooleanCheck ()
		{
			Init (SemanticAnalyzerTestInputs.ifStatementNotBooleanCheck);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test ()]
		public void IfStatementBooleanCheck ()
		{
			Init (SemanticAnalyzerTestInputs.ifStatementBooleanCheck);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void WhileStatementNotBooleanCheck ()
		{
			Init (SemanticAnalyzerTestInputs.whileStatementNotBooleanCheck);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test ()]
		public void WhileStatementBooleanCheck ()
		{
			Init (SemanticAnalyzerTestInputs.whileStatementBooleanCheck);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void StringExpressionConcatValid ()
		{
			Init (SemanticAnalyzerTestInputs.stringExpressionConcatValid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void StringExpressionSubInvalid ()
		{
			Init (SemanticAnalyzerTestInputs.stringExpressionSubInvalid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 2);
		}

		[Test ()]
		public void StringExpressionMultiplicationInvalid ()
		{
			Init (SemanticAnalyzerTestInputs.stringExpressionMultiplicationInvalid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 3);
		}

		[Test ()]
		public void StringExpressionDivisionInvalid ()
		{
			Init (SemanticAnalyzerTestInputs.stringExpressionDivisionInvalid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 3);
		}

		[Test ()]
		public void StringExpressionModuloInvalid ()
		{
			Init (SemanticAnalyzerTestInputs.stringExpressionModuloInvalid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 3);
		}

		/*
		[Test ()]
		public void StringExpressionSignedInvalid1 ()
		{
			Init (SemanticAnalyzerTestInputs.stringExpressionSignedInvalid1);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 3);
		}

		[Test ()]
		public void StringExpressionSignedInvalid2 ()
		{
			Init (SemanticAnalyzerTestInputs.stringExpressionSignedInvalid1);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 3);
		}
		*/

		[Test ()]
		public void StringExpressionNegationInvalid ()
		{
			Init (SemanticAnalyzerTestInputs.stringExpressionNegationInvalid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 4);
		}

		[Test ()]
		public void StringExpressionBooleanAndInvalid ()
		{
			Init (SemanticAnalyzerTestInputs.stringExpressionBooleanAndInvalid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 3);
		}

		[Test ()]
		public void StringExpressionBooleanOrInvalid ()
		{
			Init (SemanticAnalyzerTestInputs.stringExpressionBooleanOrInvalid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 2);
		}

		[Test ()]
		public void StringExpressionEqValid ()
		{
			Init (SemanticAnalyzerTestInputs.stringExpressionEqValid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void StringExpressionNeqValid ()
		{
			Init (SemanticAnalyzerTestInputs.stringExpressionNeqValid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void StringExpressionLtValid ()
		{
			Init (SemanticAnalyzerTestInputs.stringExpressionLtValid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void StringExpressionLteValid ()
		{
			Init (SemanticAnalyzerTestInputs.stringExpressionLteValid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void StringExpressionGtValid ()
		{
			Init (SemanticAnalyzerTestInputs.stringExpressionGtValid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test ()]
		public void StringExpressionGteValid ()
		{
			Init (SemanticAnalyzerTestInputs.stringExpressionGteValid);

			checkScannerAndParserErrors (0, 0);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}
	}
}

