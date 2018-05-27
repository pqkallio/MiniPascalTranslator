using System;

namespace CompilerTests
{
	public static class SemanticAnalyzerTestInputs
	{
		public static readonly string[] parameterIdentifiersAreUnique = new string[] {
			"program a;\n",
			"function b(i : integer, r : real) : integer;\n",
			"begin\n",
			"return 0;\n",
			"end;\n",
			"begin\n",
			"end .\n"
		};

		public static readonly string[] parameterIdentifiersAreNotUnique = new string[] {
			"program a;\n",
			"function b(i : integer, i : real) : integer;\n",
			"begin\n",
			"return 0;\n",
			"end;\n",
			"begin\n",
			"end .\n"
		};

		public static readonly string[] parametersStringParamIsNotReference = new string[] {
			"program a;\n",
			"function b(x : string) : integer;\n",
			"begin\n",
			"return 0;\n",
			"end;\n",
			"begin\n",
			"end .\n"
		};

		public static readonly string[] parametersStringParamIsReference = new string[] {
			"program a;\n",
			"function b(var x : string) : integer;\n",
			"begin\n",
			"return 0;\n",
			"end;\n",
			"begin\n",
			"end .\n"
		};

		public static readonly string[] parametersArrayParamIsNotReference = new string[] {
			"program a;\n",
			"function b(x : array[] of boolean) : integer;\n",
			"begin\n",
			"return 0;\n",
			"end;\n",
			"begin\n",
			"end .\n"
		};

		public static readonly string[] parametersArrayParamIsReference = new string[] {
			"program a;\n",
			"function b(var x : array[] of boolean) : integer;\n",
			"begin\n",
			"return 0;\n",
			"end;\n",
			"begin\n",
			"end .\n"
		};

		public static readonly string[] variableDeclarationsAreNotUniqueWithinAScope1 = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"var i : integer;\n",
			"end .\n"
		};

		public static readonly string[] variableDeclarationsAreNotUniqueWithinAScope2 = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"var i : real;\n",
			"end .\n"
		};

		public static readonly string[] variableDeclarationsAreUniqueWithinAScope1 = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"begin\n",
			"var i : integer;\n",
			"end\n",
			"end .\n"
		};

		public static readonly string[] variableDeclarationsAreUniqueWithinAScope2 = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"begin\n",
			"var i : integer;\n",
			"end\n",
			"end .\n"
		};

		public static readonly string[] arrayTypesDeclarationSizeEvaluatesToInteger = new string[] {
			"program a;\n",
			"begin\n",
			"var x : array[57 - 8] of string;\n",
			"end .\n"
		};

		public static readonly string[] arrayTypesDeclarationSizeDoesntEvaluateToInteger = new string[] {
			"program a;\n",
			"begin\n",
			"var x : array[5.7] of string;\n",
			"end .\n"
		};

		public static readonly string[] arrayAccessAllIsWell1 = new string[] {
			"program a;\n",
			"begin\n",
			"var x : array[5] of string;\n",
			"x[0] := \"moi!\";\n",
			"end .\n"
		};

		public static readonly string[] arrayAccessAllIsWell2 = new string[] {
			"program a;\n",
			"begin\n",
			"var y, z :integer;\n",
			"y := 4;\n",
			"z := y - 2;\n",
			"var x : array[5] of string;\n",
			"x[z] := \"moi!\";\n",
			"end .\n"
		};

		public static readonly string[] arrayAccessArrayNotDeclared = new string[] {
			"program a;\n",
			"begin\n",
			"x[0] := \"moi!\";\n",
			"end .\n"
		};

		public static readonly string[] arrayAccessNotAnArray = new string[] {
			"program a;\n",
			"begin\n",
			"var x : integer;\n",
			"x[0] := 2;\n",
			"end .\n"
		};

		public static readonly string[] arrayAccessIndexNotInteger = new string[] {
			"program a;\n",
			"begin\n",
			"var x : integer;\n",
			"x[true] := 2;\n",
			"end .\n"
		};

		public static readonly string[] assignmentAllIsWell = new string[] {
			"program a;\n",
			"begin\n",
			"var x : integer;\n",
			"x := 2;\n",
			"end .\n"
		};

		public static readonly string[] assignmentVariableNotDeclared = new string[] {
			"program a;\n",
			"begin\n",
			"x := 2;\n",
			"end .\n"
		};

		public static readonly string[] assignmentVariableandExpressionDontMatch = new string[] {
			"program a;\n",
			"begin\n",
			"var x : integer;\n",
			"x := 2.5;\n",
			"end .\n"
		};

		public static readonly string[] assignmentVariableandExpressionMatch1 = new string[] {
			"program a;\n",
			"begin\n",
			"var x : real;\n",
			"x := 2.5;\n",
			"end .\n"
		};

		public static readonly string[] assignmentVariableandExpressionMatch2 = new string[] {
			"program a;\n",
			"begin\n",
			"var x : real;\n",
			"x := 2;\n",
			"end .\n"
		};

		public static readonly string[] procedureCallOk = new string[] {
			"program a;\n",
			"procedure b();\n",
			"begin\n",
			"end;\n",
			"begin\n",
			"b();\n",
			"end .\n"
		};

		public static readonly string[] procedureNotDeclared = new string[] {
			"program a;\n",
			"begin\n",
			"b();\n",
			"end .\n"
		};

		public static readonly string[] functionCallNotToAFunction = new string[] {
			"program a;\n",
			"function b(i : integer, r : real, var s : string) : integer;\n",
			"begin\n",
			"return 0;\n",
			"end;\n",
			"begin\n",
			"var x : integer;\n",
			"x();\n",
			"end .\n"
		};

		public static readonly string[] functionCallValidArguments = new string[] {
			"program a;\n",
			"function b(i : integer, r : real, var s : string) : integer;\n",
			"begin\n",
			"return 0;\n",
			"end;\n",
			"begin\n",
			"var s : string;\n",
			"s := \"marque\";\n",
			"b(5, 0, s);\n",
			"end .\n"
		};

		public static readonly string[] functionCallTooFewArguments = new string[] {
			"program a;\n",
			"function b(i : integer, r : real, var s : string) : integer;\n",
			"begin\n",
			"return 0;\n",
			"end;\n",
			"begin\n",
			"var s : string;\n",
			"s := \"marque\";\n",
			"b(5, 0);\n",
			"end .\n"
		};

		public static readonly string[] functionCallTooManyArguments = new string[] {
			"program a;\n",
			"function b(i : integer, r : real, var s : string) : integer;\n",
			"begin\n",
			"return 0;\n",
			"end;\n",
			"begin\n",
			"var s : string;\n",
			"s := \"marque\";\n",
			"b(5, 0, s, true);\n",
			"end .\n"
		};

		public static readonly string[] functionCallInvalidArgument = new string[] {
			"program a;\n",
			"function b(i : integer, r : integer) : integer;\n",
			"begin\n",
			"return 0;\n",
			"end;\n",
			"begin\n",
			"var s : string;\n",
			"s := \"marque\";\n",
			"b(5, 0.5);\n",
			"end .\n"
		};

		public static readonly string[] functionDoesntReturn1 = new string[] {
			"program a;\n",
			"function b(i : integer, r : integer) : integer;\n",
			"begin\n",
			"end;\n",
			"begin\n",
			"end .\n"
		};

		public static readonly string[] functionDoesntReturn2 = new string[] {
			"program a;\n",
			"function b(i : integer, r : integer) : integer;\n",
			"begin\n",
			"return 0;\n",
			"b(0, 1);\n",
			"end;\n",
			"begin\n",
			"end .\n"
		};

		public static readonly string[] functionDoesntReturn3 = new string[] {
			"program a;\n",
			"function b(i : integer, r : integer) : integer;\n",
			"begin\n",
			"if (true) then\n",
			"return 0;\n",
			"end;\n",
			"begin\n",
			"end .\n"
		};

		public static readonly string[] functionDoesntReturn4 = new string[] {
			"program a;\n",
			"function b(i : integer, r : integer) : integer;\n",
			"begin\n",
			"while (false) do\n",
			"return 0;\n",
			"end;\n",
			"begin\n",
			"end .\n"
		};

		public static readonly string[] functionReturns1 = new string[] {
			"program a;\n",
			"function b(i : integer, r : integer) : integer;\n",
			"begin\n",
			"if (true) then\n",
			"return 0\n",
			"else return 5;\n",
			"end;\n",
			"begin\n",
			"end .\n"
		};

		public static readonly string[] functionReturns2 = new string[] {
			"program a;\n",
			"function b(i : integer, r : integer) : integer;\n",
			"begin\n",
			"if (true) then\n",
			"return 0\n",
			"else if (false) then return 5\n",
			"else return 57;\n",
			"end;\n",
			"begin\n",
			"end .\n"
		};

		public static readonly string[] functionsReturnTypeIsInvalid = new string[] {
			"program a;\n",
			"function b(i : integer, r : integer) : integer;\n",
			"begin\n",
			"return;\n",
			"end;\n",
			"begin\n",
			"end .\n"
		};

		public static readonly string[] functionsReturnTypesAreInvalid = new string[] {
			"program a;\n",
			"function b(i : integer, r : integer) : integer;\n",
			"begin\n",
			"if (true) then\n",
			"return \"bonus\"\n",
			"else if (false) then return -5.7\n",
			"else return true;\n",
			"end;\n",
			"begin\n",
			"end .\n"
		};

		public static readonly string[] proceduresReturnTypeIsValid = new string[] {
			"program a;\n",
			"procedure b(i : integer, r : integer);\n",
			"begin\n",
			"return;\n",
			"end;\n",
			"begin\n",
			"end .\n"
		};

		public static readonly string[] proceduresReturnTypeIsInvalid = new string[] {
			"program a;\n",
			"procedure b(i : integer, r : integer);\n",
			"begin\n",
			"return false;\n",
			"end;\n",
			"begin\n",
			"end .\n"
		};
	}
}
