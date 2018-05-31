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
			
		public static readonly string[] readStatementOk = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"var r : real;\n",
			"var s : string;\n",
			"var a : array[6] of integer;\n",
			"read(i, r, s, a[2]);\n",
			"end .\n"
		};

		public static readonly string[] readStatementBooleanArgument = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"read(b);\n",
			"end .\n"
		};

		public static readonly string[] readStatementArrayArgument = new string[] {
			"program a;\n",
			"begin\n",
			"var a : array[5] of integer;\n",
			"read(a);\n",
			"end .\n"
		};

		public static readonly string[] assertStatementOk = new string[] {
			"program a;\n",
			"begin\n",
			"var a : array[5] of boolean;\n",
			"assert (a[0]);\n",
			"end .\n"
		};

		public static readonly string[] assertStatementNotBooleanCheck = new string[] {
			"program a;\n",
			"begin\n",
			"var a : array[5] of integer;\n",
			"assert (a[0]);\n",
			"end .\n"
		};

		public static readonly string[] ifStatementNotBooleanCheck = new string[] {
			"program a;\n",
			"begin\n",
			"var a : array[5] of integer;\n",
			"if (a[3]) then a[2] := 4;\n",
			"end .\n"
		};

		public static readonly string[] ifStatementBooleanCheck = new string[] {
			"program a;\n",
			"begin\n",
			"var a : array[5] of BoOlEaN;\n",
			"if (a[3]) then a[2] := FaLsE;\n",
			"end .\n"
		};

		public static readonly string[] whileStatementNotBooleanCheck = new string[] {
			"program a;\n",
			"begin\n",
			"var a : array[5] of integer;\n",
			"while (a[3]) do a[2] := 4;\n",
			"end .\n"
		};

		public static readonly string[] whileStatementBooleanCheck = new string[] {
			"program a;\n",
			"begin\n",
			"var a : array[5] of BoOlEaN;\n",
			"while (a[3]) do a[2] := FaLsE;\n",
			"end .\n"
		};

		public static readonly string[] stringExpressionConcatValid = new string[] {
			"program a;\n",
			"begin\n",
			"var s : string;\n",
			"s := \"mamaaaa, \";\n",
			"s := s + \"oo-o-o-ooooo...\";\n",
			"end .\n"
		};

		public static readonly string[] stringExpressionSubInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var s : string;\n",
			"s := \"mamaaaa, \";\n",
			"s := s - \"oo-o-o-ooooo...\";\n",
			"end .\n"
		};

		public static readonly string[] stringExpressionMultiplicationInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var s : string;\n",
			"s := \"mamaaaa, \";\n",
			"s := s * \"oo-o-o-ooooo...\";\n",
			"end .\n"
		};

		public static readonly string[] stringExpressionDivisionInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var s : string;\n",
			"s := \"mamaaaa, \";\n",
			"s := s / \"oo-o-o-ooooo...\";\n",
			"end .\n"
		};

		public static readonly string[] stringExpressionModuloInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var s : string;\n",
			"s := \"mamaaaa, \";\n",
			"s := s % \"oo-o-o-ooooo...\";\n",
			"end .\n"
		};

		public static readonly string[] stringExpressionSignedInvalid1 = new string[] {
			"program a;\n",
			"begin\n",
			"var s : string;\n",
			"s := \"mamaaaa, \";\n",
			"s := +s;\n",
			"end .\n"
		};

		public static readonly string[] stringExpressionSignedInvalid2 = new string[] {
			"program a;\n",
			"begin\n",
			"var s : string;\n",
			"s := -\"mamaaaa, \";\n",
			"end .\n"
		};

		public static readonly string[] stringExpressionNegationInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var s : string;\n",
			"var b : boolean;\n",
			"s := \"mamaaaa, \";\n",
			"b := not s;\n",
			"end .\n"
		};

		public static readonly string[] stringExpressionBooleanAndInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var s : string;\n",
			"var b : boolean;\n",
			"s := \"mamaaaa, \";\n",
			"b := s and s;\n",
			"end .\n"
		};

		public static readonly string[] stringExpressionBooleanOrInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var s : string;\n",
			"var b : boolean;\n",
			"s := \"mamaaaa, \";\n",
			"b := s or s;\n",
			"end .\n"
		};

		public static readonly string[] stringExpressionEqValid = new string[] {
			"program a;\n",
			"begin\n",
			"var s : string;\n",
			"var b : boolean;\n",
			"s := \"mamaaaa, \";\n",
			"b := s = s;\n",
			"end .\n"
		};

		public static readonly string[] stringExpressionNeqValid = new string[] {
			"program a;\n",
			"begin\n",
			"var s : string;\n",
			"var b : boolean;\n",
			"s := \"mamaaaa, \";\n",
			"b := s <> s;\n",
			"end .\n"
		};

		public static readonly string[] stringExpressionLtValid = new string[] {
			"program a;\n",
			"begin\n",
			"var s : string;\n",
			"var b : boolean;\n",
			"s := \"mamaaaa, \";\n",
			"b := s < s;\n",
			"end .\n"
		};

		public static readonly string[] stringExpressionLteValid = new string[] {
			"program a;\n",
			"begin\n",
			"var s : string;\n",
			"var b : boolean;\n",
			"s := \"mamaaaa, \";\n",
			"b := s <= s;\n",
			"end .\n"
		};

		public static readonly string[] stringExpressionGtValid = new string[] {
			"program a;\n",
			"begin\n",
			"var s : string;\n",
			"var b : boolean;\n",
			"s := \"mamaaaa, \";\n",
			"b := s > s;\n",
			"end .\n"
		};

		public static readonly string[] stringExpressionGteValid = new string[] {
			"program a;\n",
			"begin\n",
			"var s : string;\n",
			"var b : boolean;\n",
			"s := \"mamaaaa, \";\n",
			"b := s >= s;\n",
			"end .\n"
		};

		/////////////
		/// ////////
		/// //////////
		/// ////////////////
		/// 
		/// 

		public static readonly string[] integerExpressionAddValid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"var a : array[3] of integer;\n",
			"i := 15;\n",
			"i := i + a[7];\n",
			"end .\n"
		};

		public static readonly string[] integerExpressionSubValid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"var a : array[3] of integer;\n",
			"i := 15;\n",
			"i := i - a[7];\n",
			"end .\n"
		};

		public static readonly string[] integerExpressionMultiplicationValid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"var a : array[3] of integer;\n",
			"i := 15;\n",
			"i := i * a[7];\n",
			"end .\n"
		};

		public static readonly string[] integerExpressionDivisionValid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"var a : array[3] of integer;\n",
			"i := 15;\n",
			"i := i / a[7];\n",
			"end .\n"
		};

		public static readonly string[] integerExpressionModuloValid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"var a : array[3] of integer;\n",
			"i := 15;\n",
			"i := i % a[7];\n",
			"end .\n"
		};

		public static readonly string[] integerExpressionSignedValid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"var a : array[3] of integer;\n",
			"i := 15;\n",
			"i := -i + a[7];\n",
			"end .\n"
		};

		public static readonly string[] integerExpressionSignedValid2 = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"var a : array[3] of integer;\n",
			"i := 15;\n",
			"i := +i + a[7];\n",
			"end .\n"
		};

		public static readonly string[] integerExpressionNegationInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : integer;\n",
			"var a : array[3] of integer;\n",
			"i := 15;\n",
			"i := i + a[7];\n",
			"b := not i;\n",
			"end .\n"
		};

		public static readonly string[] integerExpressionBooleanAndInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : integer;\n",
			"var a : array[3] of integer;\n",
			"i := 15;\n",
			"b := i and a[1];\n",
			"end .\n"
		};

		public static readonly string[] integerExpressionBooleanOrInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : integer;\n",
			"var a : array[3] of integer;\n",
			"i := 15;\n",
			"b := i or a[1];\n",
			"end .\n"
		};

		public static readonly string[] integerExpressionEqValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : integer;\n",
			"var a : array[3] of integer;\n",
			"i := 15;\n",
			"b := i = a[1];\n",
			"end .\n"
		};

		public static readonly string[] integerExpressionNeqValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : integer;\n",
			"var a : array[3] of integer;\n",
			"i := 15;\n",
			"b := i <> a[1];\n",
			"end .\n"
		};

		public static readonly string[] integerExpressionLtValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : integer;\n",
			"var a : array[3] of integer;\n",
			"i := 15;\n",
			"b := i < a[1];\n",
			"end .\n"
		};

		public static readonly string[] integerExpressionLteValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : integer;\n",
			"var a : array[3] of integer;\n",
			"i := 15;\n",
			"b := i <= a[1];\n",
			"end .\n"
		};

		public static readonly string[] integerExpressionGtValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : integer;\n",
			"var a : array[3] of integer;\n",
			"i := 15;\n",
			"b := i > a[1];\n",
			"end .\n"
		};

		public static readonly string[] integerExpressionGteValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : integer;\n",
			"var a : array[3] of integer;\n",
			"i := 15;\n",
			"b := i >= a[1];\n",
			"end .\n"
		};

		/////////////
		/// ////////
		/// //////////
		/// ////////////////
		/// 
		/// 

		public static readonly string[] realExpressionAddValid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : real;\n",
			"var a : array[3] of real;\n",
			"i := 15.7;\n",
			"i := i + a[7];\n",
			"end .\n"
		};

		public static readonly string[] realExpressionSubValid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : real;\n",
			"var a : array[3] of real;\n",
			"i := 15.7;\n",
			"i := i - a[7];\n",
			"end .\n"
		};

		public static readonly string[] realExpressionMultiplicationValid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : real;\n",
			"var a : array[3] of real;\n",
			"i := 15.7;\n",
			"i := i * a[7];\n",
			"end .\n"
		};

		public static readonly string[] realExpressionDivisionValid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : real;\n",
			"var a : array[3] of real;\n",
			"i := 15.7;\n",
			"i := i / a[7];\n",
			"end .\n"
		};

		public static readonly string[] realExpressionModuloInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : real;\n",
			"var a : array[3] of real;\n",
			"i := 15.7;\n",
			"i := i % a[7];\n",
			"end .\n"
		};

		public static readonly string[] realExpressionSignedValid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : real;\n",
			"var a : array[3] of real;\n",
			"i := 15.7;\n",
			"i := -i + a[7];\n",
			"end .\n"
		};

		public static readonly string[] realExpressionSignedValid2 = new string[] {
			"program a;\n",
			"begin\n",
			"var i : real;\n",
			"var a : array[3] of real;\n",
			"i := 15.7;\n",
			"i := +i + a[7];\n",
			"end .\n"
		};

		public static readonly string[] realExpressionNegationInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : real;\n",
			"var a : array[3] of real;\n",
			"i := 15.7;\n",
			"i := i + a[7];\n",
			"b := not i;\n",
			"end .\n"
		};

		public static readonly string[] realExpressionBooleanAndInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : real;\n",
			"var a : array[3] of real;\n",
			"i := 15.7;\n",
			"b := i and a[1];\n",
			"end .\n"
		};

		public static readonly string[] realExpressionBooleanOrInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : real;\n",
			"var a : array[3] of real;\n",
			"i := 15.7;\n",
			"b := i or a[1];\n",
			"end .\n"
		};

		public static readonly string[] realExpressionEqValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : real;\n",
			"var a : array[3] of real;\n",
			"i := 15.7;\n",
			"b := i = a[1];\n",
			"end .\n"
		};

		public static readonly string[] realExpressionNeqValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : real;\n",
			"var a : array[3] of real;\n",
			"i := 15.7;\n",
			"b := i <> a[1];\n",
			"end .\n"
		};

		public static readonly string[] realExpressionLtValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : real;\n",
			"var a : array[3] of real;\n",
			"i := 15.7;\n",
			"b := i < a[1];\n",
			"end .\n"
		};

		public static readonly string[] realExpressionLteValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : real;\n",
			"var a : array[3] of real;\n",
			"i := 15.7;\n",
			"b := i <= a[1];\n",
			"end .\n"
		};

		public static readonly string[] realExpressionGtValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : real;\n",
			"var a : array[3] of real;\n",
			"i := 15.7;\n",
			"b := i > a[1];\n",
			"end .\n"
		};

		public static readonly string[] realExpressionGteValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : real;\n",
			"var a : array[3] of real;\n",
			"i := 15.7;\n",
			"b := i >= a[1];\n",
			"end .\n"
		};

		/////////////
		/// ////////
		/// //////////
		/// ////////////////
		/// 
		/// 

		public static readonly string[] booleanExpressionAddInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : boolean;\n",
			"var a : array[3] of boolean;\n",
			"i := true;\n",
			"i := i + a[7];\n",
			"end .\n"
		};

		public static readonly string[] booleanExpressionSubInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : boolean;\n",
			"var a : array[3] of boolean;\n",
			"i := true;\n",
			"i := i - a[7];\n",
			"end .\n"
		};

		public static readonly string[] booleanExpressionMultiplicationInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : boolean;\n",
			"var a : array[3] of boolean;\n",
			"i := true;\n",
			"i := i * a[7];\n",
			"end .\n"
		};

		public static readonly string[] booleanExpressionDivisionInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : boolean;\n",
			"var a : array[3] of boolean;\n",
			"i := true;\n",
			"i := i / a[7];\n",
			"end .\n"
		};

		public static readonly string[] booleanExpressionModuloInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : boolean;\n",
			"var a : array[3] of boolean;\n",
			"i := true;\n",
			"i := i % a[7];\n",
			"end .\n"
		};

		public static readonly string[] booleanExpressionSignedInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : boolean;\n",
			"var a : array[3] of boolean;\n",
			"i := true;\n",
			"i := -i;\n",
			"end .\n"
		};

		public static readonly string[] booleanExpressionSignedInvalid2 = new string[] {
			"program a;\n",
			"begin\n",
			"var i : boolean;\n",
			"var a : array[3] of boolean;\n",
			"i := true;\n",
			"i := +i;\n",
			"end .\n"
		};

		public static readonly string[] booleanExpressionNegationValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : boolean;\n",
			"var a : array[3] of boolean;\n",
			"i := true;\n",
			"b := not i;\n",
			"end .\n"
		};

		public static readonly string[] booleanExpressionBooleanAndValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : boolean;\n",
			"var a : array[3] of boolean;\n",
			"i := true;\n",
			"b := i and a[1];\n",
			"end .\n"
		};

		public static readonly string[] booleanExpressionBooleanOrValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : boolean;\n",
			"var a : array[3] of boolean;\n",
			"i := true;\n",
			"b := i or a[1];\n",
			"end .\n"
		};

		public static readonly string[] booleanExpressionEqValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : boolean;\n",
			"var a : array[3] of boolean;\n",
			"i := true;\n",
			"b := i = a[1];\n",
			"end .\n"
		};

		public static readonly string[] booleanExpressionNeqValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : boolean;\n",
			"var a : array[3] of boolean;\n",
			"i := true;\n",
			"b := i <> a[1];\n",
			"end .\n"
		};

		public static readonly string[] booleanExpressionLtValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : boolean;\n",
			"var a : array[3] of boolean;\n",
			"i := true;\n",
			"b := i < a[1];\n",
			"end .\n"
		};

		public static readonly string[] booleanExpressionLteValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : boolean;\n",
			"var a : array[3] of boolean;\n",
			"i := true;\n",
			"b := i <= a[1];\n",
			"end .\n"
		};

		public static readonly string[] booleanExpressionGtValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : boolean;\n",
			"var a : array[3] of boolean;\n",
			"i := true;\n",
			"b := i > a[1];\n",
			"end .\n"
		};

		public static readonly string[] booleanExpressionGteValid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : boolean;\n",
			"var a : array[3] of boolean;\n",
			"i := true;\n",
			"b := i >= a[1];\n",
			"end .\n"
		};

		/////////////
		/// ////////
		/// //////////
		/// ////////////////
		/// 
		/// 

		public static readonly string[] integerAndRealExpressionAddValid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"var a : array[3] of real;\n",
			"i := 15;\n",
			"a[2] := i + a[7];\n",
			"end .\n"
		};

		public static readonly string[] integerAndRealExpressionSubValid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"var a : array[3] of real;\n",
			"i := 15;\n",
			"a[2] := i - a[7];\n",
			"end .\n"
		};

		public static readonly string[] integerAndRealExpressionMultiplicationValid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"var a : array[3] of real;\n",
			"i := 15;\n",
			"a[2] := i * a[7];\n",
			"end .\n"
		};

		public static readonly string[] integerAndRealExpressionDivisionValid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"var a : array[3] of real;\n",
			"i := 15;\n",
			"a[2] := i / a[7];\n",
			"end .\n"
		};

		public static readonly string[] integerAndRealExpressionModuloInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"var a : array[3] of real;\n",
			"i := 15;\n",
			"a[2] := i % a[7];\n",
			"end .\n"
		};

		public static readonly string[] integerAndRealExpressionEqInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : integer;\n",
			"var a : array[3] of real;\n",
			"i := 15;\n",
			"b := i = a[1];\n",
			"end .\n"
		};

		public static readonly string[] integerAndRealExpressionNeqInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : integer;\n",
			"var a : array[3] of real;\n",
			"i := 15;\n",
			"b := i <> a[1];\n",
			"end .\n"
		};

		public static readonly string[] integerAndRealExpressionLtInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : integer;\n",
			"var a : array[3] of real;\n",
			"i := 15;\n",
			"b := i < a[1];\n",
			"end .\n"
		};

		public static readonly string[] integerAndRealExpressionLteInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : integer;\n",
			"var a : array[3] of real;\n",
			"i := 15;\n",
			"b := i <= a[1];\n",
			"end .\n"
		};

		public static readonly string[] integerAndRealExpressionGtInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : integer;\n",
			"var a : array[3] of real;\n",
			"i := 15;\n",
			"b := i > a[1];\n",
			"end .\n"
		};

		public static readonly string[] integerAndRealExpressionGteInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var b : boolean;\n",
			"var i : integer;\n",
			"var a : array[3] of real;\n",
			"i := 15;\n",
			"b := i >= a[1];\n",
			"end .\n"
		};

		/////////////
		/// ////////
		/// //////////
		/// ////////////////
		/// 
		/// 

		public static readonly string[] assignIntegerAndRealExpressionToIntegerVarInvalid = new string[] {
			"program a;\n",
			"begin\n",
			"var i : integer;\n",
			"var a : array[3] of real;\n",
			"var x : integer;\n",
			"i := 15;\n",
			"a[2] := -5.7;\n",
			"x := i + a[2];\n",
			"end .\n"
		};
	}
}
