using System;

namespace CompilerTests
{
	public class ParserTestInputs
	{
		public static readonly string[] emptyProgram = {
			"program empty;\n",
			"begin\n",
			"end ."
		};

		public static readonly string[] oneFunction = {
			"program oneFunction;\n",
			"function func() : string;\n",
			"begin\n",
			"end;\n",
			"begin\n",
			"end ."
		};

		public static readonly string[] oneProcedure = {
			"program oneProcedure;\n",
			"procedure proc();\n",
			"begin\n",
			"end;\n",
			"begin\n",
			"end ."
		};

		public static readonly string[] functionAndProcedure = {
			"program oneProcedure;\n",
			"function func() : string;\n",
			"begin\n",
			"end;\n",
			"procedure proc();\n",
			"begin\n",
			"end;\n",
			"begin\n",
			"end ."
		};

		public static readonly string[] functionWithParams = {
			"program oneFunction;\n",
			"function func(var i : integer, ii : real, str : string, ary : array[] of Boolean) : string;\n",
			"begin\n",
			"end;\n",
			"begin\n",
			"end ."
		};

		public static readonly string[] procedureWithParams = {
			"program oneFunction;\n",
			"procedure proc(var i : integer, ii : real, str : string, ary : array[] of Boolean);\n",
			"begin\n",
			"end;\n",
			"begin\n",
			"end ."
		};

		public static readonly string[] declareInteger = {
			"program empty;\n",
			"begin\n",
			"var x : integer;\n",
			"end ."
		};

		public static readonly string[] declareReal = {
			"program empty;\n",
			"begin\n",
			"var x : real;\n",
			"end ."
		};

		public static readonly string[] declareBoolean = {
			"program empty;\n",
			"begin\n",
			"var x : Boolean;\n",
			"end ."
		};

		public static readonly string[] declareString = {
			"program empty;\n",
			"begin\n",
			"var x : string;\n",
			"end ."
		};

		public static readonly string[] declareArrayOfInteger = {
			"program empty;\n",
			"begin\n",
			"var x : array[1] of integer;\n",
			"end ."
		};

		public static readonly string[] declareArrayOfReal = {
			"program empty;\n",
			"begin\n",
			"var x : array[2] of real;\n",
			"end ."
		};

		public static readonly string[] declareArrayOfBoolean = {
			"program empty;\n",
			"begin\n",
			"var x : array[3] of Boolean;\n",
			"end ."
		};

		public static readonly string[] declareArrayOfString = {
			"program empty;\n",
			"begin\n",
			"var x : array[4] of string;\n",
			"end ."
		};

		public static readonly string[] declareMultipleInteger = {
			"program empty;\n",
			"begin\n",
			"var x, y : integer;\n",
			"end ."
		};

		public static readonly string[] declareMultipleReal = {
			"program empty;\n",
			"begin\n",
			"var x, y, z : real;\n",
			"end ."
		};

		public static readonly string[] declareMultipleBoolean = {
			"program empty;\n",
			"begin\n",
			"var x, y, z, k : Boolean;\n",
			"end ."
		};

		public static readonly string[] declareMultipleString = {
			"program empty;\n",
			"begin\n",
			"var x, y, z, k, l : string;\n",
			"end ."
		};

		public static readonly string[] declareMultipleArrayOfInteger = {
			"program empty;\n",
			"begin\n",
			"var x, y, z, k, l, m : array[1] of integer;\n",
			"end ."
		};

		public static readonly string[] declareMultipleArrayOfReal = {
			"program empty;\n",
			"begin\n",
			"var x, y, z, k, l, m, n : array[2] of real;\n",
			"end ."
		};

		public static readonly string[] declareMultipleArrayOfBoolean = {
			"program empty;\n",
			"begin\n",
			"var x, y, z, k, l, m, n, o : array[3] of Boolean;\n",
			"end ."
		};

		public static readonly string[] declareMultipleArrayOfString = {
			"program empty;\n",
			"begin\n",
			"var x, y, z, k, l, m, n, o, p : array[4] of string;\n",
			"end ."
		};

		public static readonly string[] declareAndAssignInteger = {
			"program empty;\n",
			"begin\n",
			"var x : integer;\n",
			"x := 5;\n",
			"end ."
		};

		public static readonly string[] declareAndAssignReal = {
			"program empty;\n",
			"begin\n",
			"var x : real;\n",
			"x := 5.68e-4;\n",
			"end ."
		};

		public static readonly string[] declareAndAssignBoolean = {
			"program empty;\n",
			"begin\n",
			"var x : Boolean;\n",
			"x := true;\n",
			"end ."
		};

		public static readonly string[] declareAndAssignString = {
			"program empty;\n",
			"begin\n",
			"var x : string;\n",
			"x := \"supadupa\";\n",
			"end ."
		};

		public static readonly string[] declareAndAssignArrayOfInteger = {
			"program empty;\n",
			"begin\n",
			"var x : array[1] of integer;\n",
			"x[0] := 5;\n",
			"end ."
		};

		public static readonly string[] declareAndAssignArrayOfReal = {
			"program empty;\n",
			"begin\n",
			"var x : array[2] of real;\n",
			"x[1] := 0.68;\n",
			"end ."
		};

		public static readonly string[] declareAndAssignArrayOfBoolean = {
			"program empty;\n",
			"begin\n",
			"var x : array[3] of Boolean;\n",
			"x[0] := true;\n",
			"x[1] := false;\n",
			"end ."
		};

		public static readonly string[] declareAndAssignArrayOfString = {
			"program empty;\n",
			"begin\n",
			"var x : array[4] of string;\n",
			"x[3] := \"supadupa\";\n",
			"end ."
		};

		public static readonly string[] declareAndAssignFunctionCall1 = {
			"program empty;\n",
			"begin\n",
			"var x : string;\n",
			"x := func();\n",
			"end ."
		};

		public static readonly string[] declareAndAssignFunctionCall2 = {
			"program empty;\n",
			"begin\n",
			"var x : string;\n",
			"x := func(3, 5 + 8, true);\n",
			"end ."
		};

		public static readonly string[] returnStatement1 = {
			"program empty;\n",
			"begin\n",
			"var x : string;\n",
			"return x;\n",
			"end ."
		};

		public static readonly string[] returnStatement2 = {
			"program empty;\n",
			"begin\n",
			"return;\n",
			"end ."
		};

		public static readonly string[] readStatement = {
			"program empty;\n",
			"begin\n",
			"var x : string;\n",
			"read(x, y, zeta);\n",
			"end ."
		};

		public static readonly string[] writeStatement = {
			"program empty;\n",
			"begin\n",
			"var x : string;\n",
			"writeln(x, 6 + z[0], -9, true);\n",
			"end ."
		};

		public static readonly string[] assertStatement = {
			"program empty;\n",
			"begin\n",
			"var x : string;\n",
			"assert(x[9] % 9 < 6 / 5);\n",
			"end ."
		};

		public static readonly string[] blockInABlockStatement1 = {
			"program empty;\n",
			"begin\n",
			"var x : string;\n",
			"assert(x[9] % 9 < 6 / 5);\n",
			"begin\n",
			"  writeln(\"is funny\");",
			"  writeln(x)\n",
			"end;\n",
			"end ."
		};

		public static readonly string[] blockInABlockStatement2 = {
			"program empty;\n",
			"begin\n",
			"var x : string;\n",
			"assert(x[9] % 9 < 6 / 5);\n",
			"begin\n",
			"  writeln(\"is funny\");",
			"  writeln(x);\n",
			"end;\n",
			"end ."
		};

		public static readonly string[] ifThenStatement = {
			"program empty;\n",
			"begin\n",
			"if x > 5 then\n",
			"x := 4;\n",
			"end ."
		};

		public static readonly string[] ifThenElseStatement = {
			"program empty;\n",
			"begin\n",
			"if x > 5 then\n",
			"x := 4\n",
			"else x := x - 1;\n",
			"writeln(x)\n",
			"end ."
		};

		public static readonly string[] whileStatement = {
			"program empty;\n",
			"begin\n",
				"  var x : string;\n",
			    "  x := 0;\n",
				"  while x <= 10 do\n",
				"    begin\n",
				"      x := x + 1;\n",
				"      writeln(\"x is now \", x)\n",
				"    end\n",
			"end ."
		};

		public static readonly string[] accessArraySize = {
			"program empty;\n",
			"begin\n",
			"var x : array[5] of integer;\n",
			"var y : integer;\n",
			"y := x.size\n",
			"end ."
		};

		public static readonly string[] procedureCall = {
			"program empty;\n",
			"begin\n",
			"proc(\"supadupa\")\n",
			"end ."
		};
	}
}

