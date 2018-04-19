using System;
using System.Collections.Generic;
using Compiler;

namespace CompilerTests
{
	public class ScannerTestInputs
	{
		public static readonly string[] empty = {
		};

		public static readonly Dictionary<string[], TokenType> singleTokens = new Dictionary<string[], TokenType> ()
		{
			{new[] {"var"}, TokenType.VAR},
			{new[] {"false"}, TokenType.BOOLEAN_VAL_FALSE},
			{new[] {"true"}, TokenType.BOOLEAN_VAL_TRUE},
			{new[] {"end"}, TokenType.END},
			{new[] {"integer"}, TokenType.TYPE_INTEGER},
			{new[] {"do"}, TokenType.DO_WHILE},
			{new[] {"read"}, TokenType.READ},
			{new[] {"writeln"}, TokenType.WRITELN},
			{new[] {"string"}, TokenType.TYPE_STRING},
			{new[] {"Boolean"}, TokenType.TYPE_BOOLEAN},
			{new[] {"assert"}, TokenType.ASSERT},
			{new[] {"size"}, TokenType.SIZE},
			{new[] {"array"}, TokenType.TYPE_ARRAY},
			{new[] {"real"}, TokenType.TYPE_REAL},
			{new[] {"or"}, TokenType.BINARY_OP_LOG_OR},
			{new[] {"and"}, TokenType.BINARY_OP_LOG_AND},
			{new[] {"not"}, TokenType.UNARY_OP_LOG_NEG},
			{new[] {"program"}, TokenType.PROGRAM},
			{new[] {"procedure"}, TokenType.PROCEDURE},
			{new[] {"function"}, TokenType.FUNCTION},
			{new[] {"of"}, TokenType.OF},
			{new[] {"begin"}, TokenType.BEGIN},
			{new[] {"return"}, TokenType.RETURN},
			{new[] {"if"}, TokenType.IF},
			{new[] {"then"}, TokenType.THEN},
			{new[] {"else"}, TokenType.ELSE},
			{new[] {"while"}, TokenType.WHILE_LOOP},
			{new[] {"="}, TokenType.BINARY_OP_LOG_EQ},
			{new[] {";"}, TokenType.END_STATEMENT},
			{new[] {","}, TokenType.COMMA},
			{new[] {"."}, TokenType.DOT},
			{new[] {"("}, TokenType.PARENTHESIS_LEFT},
			{new[] {")"}, TokenType.PARENTHESIS_RIGHT},
			{new[] {"["}, TokenType.BRACKET_LEFT},
			{new[] {"]"}, TokenType.BRACKET_RIGHT},
			{new[] {"*"}, TokenType.BINARY_OP_MUL},
			{new[] {"/"}, TokenType.BINARY_OP_DIV},
			{new[] {"+"}, TokenType.SIGN_PLUS},
			{new[] {"-"}, TokenType.SIGN_MINUS},
			{new[] {"%"}, TokenType.BINARY_OP_MOD},
			{new[] {":"}, TokenType.SET_TYPE},
			{new[] {":="}, TokenType.ASSIGN},
			{new[] {"<"}, TokenType.BINARY_OP_LOG_LT},
			{new[] {"<="}, TokenType.BINARY_OP_LOG_LTE},
			{new[] {"<>"}, TokenType.BINARY_OP_LOG_NEQ},
			{new[] {">"}, TokenType.BINARY_OP_LOG_GT},
			{new[] {">="}, TokenType.BINARY_OP_LOG_GTE}
		};

		public static readonly string[] emptyString = {
			"\"\""
		};

		public static readonly string[] validString = {
			"\"bamboozle\""
		};

		public static readonly string[] escapeCharsInString = {
			"\"b\\ra\\nm\\0b\\\"ooz\\tle\""
		};

		public static readonly string[] invalidStringSpansMultipleLines = {
			"\"bam\nboozle\""
		};

		public static readonly string[] invalidStringNeverEnds = {
			"\"bamboozle"
		};

		public static readonly string[] validInteger1 = {
			"0"
		};

		public static readonly string[] validInteger2 = {
			"123"
		};

		public static readonly string[] validInteger3 = {
			"00000000123"
		};

		public static readonly string[] signAndInteger1 = {
			"+123"
		};

		public static readonly string[] signAndInteger2 = {
			"-123"
		};

		public static readonly string[] validReal1 = {
			"0.0"
		};

		public static readonly string[] validReal2 = {
			"001.05"
		};

		public static readonly string[] validReal3 = {
			"0.00001e0"
		};

		public static readonly string[] validReal4 = {
			"001.05e-001"
		};

		public static readonly string[] validReal5 = {
			"678.05e+016001"
		};

		public static readonly string[] invalidReal1 = {
			"0.0e"
		};

		public static readonly string[] invalidReal2 = {
			"001.05e+"
		};

		public static readonly string[] invalidReal3 = {
			"0.00001e-"
		};

		public static readonly string[] signAndReal1 = {
			"+123.05"
		};

		public static readonly string[] signAndReal2 = {
			"-123.432e-45"
		};

		public static readonly string[] validId1 = {
			"a"
		};

		public static readonly string[] validId2 = {
			"a4_RTY_ZzA___190288764"
		};

		public static readonly string[] invalidId = {
			"_i_is_n0t_v4l1d"
		};

		public static readonly string[] errorToken = {
			"?var"
		};

		public static readonly string[] size = {
			".size"
		};

		public static readonly string[] dot1 = {
			"."
		};

		public static readonly string[] dot2 = {
			".nice"
		};

		public static readonly string[] gcd = {
			"program gcd;\n",
			"\n",
			"begin\n",
			"var p, q : integer;\n",
			"read (p, q);\n",
			"\n",
			"while p <> q do\n",
			"	if p > q\n",
			"		then p := p - q\n",
			"	else q := q - p;\n",
			"\n",
			"writeln(p);\n",
			"end .\n"
		};

		public static readonly string[] wtf = {
			"program wtf;\n",
			"\n",
			"function halve(n : integer, x : real) : integer;\n",
			"begin\n",
			"x := x / 2;\n",
			"n := n + 1;\n",
			"\n",
			"if x < 1\n",
			"	then return n\n",
			"else return add_one(n, x);\n",
			"end;\n",
			"\n",
			"function add_one(n : integer, x : real) : integer;\n",
			"begin\n",
			"x := x + 1;\n",
			"return halve(n, x);\n",
			"end;\n",
			"\n",
			"begin\n",
			"var x : real;\n",
			"var n : integer;\n",
			"\n",
			"n := 0;\n",
			"read (x);\n",
			"\n",
			"{* This is to avoid division by zero: *}\n",
			"assert(x <> -1.0);\n",
			"\n",
			"n := add_one(n, x);\n",
			"\n",
			"writeln(\"Added 1 and halved \", n, \" times.\");\n",
			"end .\n"
		};

		public static readonly string[] invertedArrays = {
			"program inverted_arrays;\n",
			"\n",
			"procedure check_gt(smaller : integer, bigger : integer)\n",
			"begin\n",
			"assert(smaller < bigger);\n",
			"end;\n",
			"\n",
			"procedure array_magic(var i : integer,\n",
			"	var m : integer, \n",
			"	var a : array[] of integer, \n",
			"	var b : array[] of integer);\n",
			"begin\n",
			"check_gt(-1, i);\n",
			"check_gt(-1, m);\n",
			"check_gt(i, a.size);\n",
			"check_gt(i, b.size);\n",
			"check_gt(m, a.size);\n",
			"check_gt(m, b.size);\n",
			"\n",
			"a[i] := i;\n",
			"b[m] := i;\n",
			"\n",
			"i := i + 1;\n",
			"m := m - 1;\n",
			"end;\n",
			"\n",
			"procedure check_array_elements(var i : integer, \n",
			"	var m : integer, \n",
			"	var a : array[] of integer,\n",
			"	var b : array[] of integer);\n",
			"begin\n",
			"check_gt(-1, i);\n",
			"check_gt(-1, m);\n",
			"check_gt(i, a.size);\n",
			"check_gt(i, b.size);\n",
			"check_gt(m, a.size);\n",
			"check_gt(m, b.size);\n",
			"\n",
			"assert(a[i] = b[m]);\n",
			"\n",
			"i := i + 1;\n",
			"m := m - 1;\n",
			"end;\n",
			"\n",
			"begin\n",
			"var n : integer;\n",
			"var m : integer;\n",
			"read(n);\n",
			"m := n - 1;\n",
			"\n",
			"var a, b : array[n] of integer;\n",
			"\n",
			"assert(a.size = n);\n",
			"assert(b.size = n);\n",
			"\n",
			"var i : integer;\n",
			"i := 0;\n",
			"\n",
			"while i < n do\n",
			"	array_magic(i, m, a, b);\n",
			"\n",
			"i := 0;\n",
			"m := n;\n",
			"\n",
			"while i < n do\n",
			"	check_array_elements(i, m, a, b);\n",
			"end .\n"
		};
	}
}

