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
	}
}

