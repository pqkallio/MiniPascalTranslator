using System;
using System.Text;
using System.Collections.Generic;

namespace Compiler
{
	public static class CTranslatorConstants
	{
		public static readonly string PREPROCESSOR_DIRECTIVE = "#";
		public static readonly string INCLUSION = PREPROCESSOR_DIRECTIVE + "include";
		public static readonly Tuple<string, string> LIBRARY_INCLUSION_DELIMITERS = Tuple.Create("<", ">");
		public static readonly Tuple<string, string> CALL_TAIL_DELIMITERS = Tuple.Create("(", ")");
		public static readonly Tuple<string, string> BLOCK_DELIMITERS = Tuple.Create("{", "}");
		public static readonly Tuple<string, string> ARRAY_INDEX_DELIMITERS = Tuple.Create("[", "]");
		public static readonly Tuple<string, string> COMMENT_DELIMITERS = Tuple.Create("/*", "*/");
		public static readonly string[] LIBRARIES = new string[] {"stdio.h", "stdlib.h"};
		public static readonly string MEM_ALLOCATION = "malloc";
		public static readonly string MEM_RELEASE = "free";
		public static readonly string MEM_POINTER = "*";
		public static readonly string MEM_ADDRESS = "&";
		public static readonly string SIZE_OF = "sizeof";
		public static readonly string ASSIGNMENT = "=";
		public static readonly string STRING_DELIMITER = "\"";

		public static readonly Dictionary<TokenType, string> SIMPLE_TYPE_NAMES = new Dictionary<TokenType, string>()
		{
			{TokenType.INTEGER_VAL, "int"},
			{TokenType.REAL_VAL, "float"},
			{TokenType.STRING_VAL, "char*"},
			{TokenType.BOOLEAN_VAL, "int"},
			{TokenType.TYPE_INTEGER, "int"},
			{TokenType.TYPE_REAL, "float"},
			{TokenType.TYPE_STRING, "char*"},
			{TokenType.TYPE_BOOLEAN, "int"},
			{TokenType.VOID, "void"}
		};

		public static readonly Dictionary<TokenType, string> PARAM_SIMPLE_TYPE_NAMES = new Dictionary<TokenType, string>()
		{
			{TokenType.INTEGER_VAL, "int"},
			{TokenType.REAL_VAL, "float"},
			{TokenType.STRING_VAL, "char"},
			{TokenType.BOOLEAN_VAL, "int"},
			{TokenType.TYPE_INTEGER, "int"},
			{TokenType.TYPE_REAL, "float"},
			{TokenType.TYPE_STRING, "char"},
			{TokenType.TYPE_BOOLEAN, "int"},
		};

		public static readonly Dictionary<TokenType, string> OPERATION_STRINGS = new Dictionary<TokenType, string> ()
		{
			{TokenType.BINARY_OP_ADD, "+"},
			{TokenType.BINARY_OP_DIV, "/"},
			{TokenType.BINARY_OP_LOG_AND, "&&"},
			{TokenType.BINARY_OP_LOG_EQ, "=="},
			{TokenType.BINARY_OP_LOG_GT, ">"},
			{TokenType.BINARY_OP_LOG_GTE, ">="},
			{TokenType.BINARY_OP_LOG_LT, "<"},
			{TokenType.BINARY_OP_LOG_LTE, "<="},
			{TokenType.BINARY_OP_LOG_NEQ, "!="},
			{TokenType.BINARY_OP_LOG_OR, "||"},
			{TokenType.BINARY_OP_MOD, "%"},
			{TokenType.BINARY_OP_MUL, "*"},
			{TokenType.BINARY_OP_SUB, "-"},
			{TokenType.UNARY_OP_LOG_NEG, "!"},
		};

		public static readonly string[] HELPER_FUNCTION_DECLARATIONS = new string[]
		{
			"unsigned int get_string_length(char* string)",
			"int load_from_int_array(int* array, unsigned int index, int* temp)",
			"int load_from_float_array(float* array, unsigned int index, float* temp)",
			"int load_from_string_array(char** array, unsigned int index, char** temp)",
			"int insert_to_int_array(int* array, unsigned int index, int temp)",
			"int insert_to_float_array(float* array, unsigned int index, float temp)",
			"int insert_to_string_array(char** array, unsigned int index, char* temp)",
			"char* string_concatenation(char* first, char* second)"
		};

		public static readonly string[] getStringLengthFunction = new string[]
		{
			HELPER_FUNCTION_DECLARATIONS[0],
			"{",
			"	int n = 0;",
			"	char c = string[n];",
			"",
			"	goto check_condition;",
			"",
			"	loop:",
			"	n = n + 1;",
			"	c = string[n];",
			"",
			"	check_condition:",
			"	if (c != '\\0') goto loop;",
			"",
			"	return n;",
			"}"
		};

		public static readonly string[] loadFromIntArrayFunction = new string[]
		{
			HELPER_FUNCTION_DECLARATIONS[1],
			"{",
			"	int size_element = array[0];",
			"	unsigned int array_size = (unsigned int)size_element;",
			"",
			"	if (index < array_size) goto load;",
			"",
			"	return 0;",
			"",
			"	load:",
			"	*temp = array[index + 1];",
			"	return 1;",
			"}"
		};

		public static readonly string[] loadFromFloatArray = new string[]
		{
			HELPER_FUNCTION_DECLARATIONS[2],
			"{",
			"	float size_element = array[0];",
			"	unsigned int array_size = (unsigned int)size_element;",
			"",
			"	if (index < array_size) goto load;",
			"",
			"	return 0;",
			"",
			"	load:",
			"	*temp = array[index + 1];",
			"	return 1;",
			"}"
		};

		public static readonly string[] loadFromStringArray = new string[]
		{
			HELPER_FUNCTION_DECLARATIONS[3],
			"{",
			"	char* size_element = array[0];",
			"	unsigned int array_size = (unsigned int)size_element;",
			"",
			"	if (index < array_size) goto load;",
			"",
			"	return 0;",
			"",
			"	load:",
			"	*temp = array[index + 1];",
			"	return 1;",
			"}"
		};


		public static readonly string[] insertToIntArray = new string[]
		{
			HELPER_FUNCTION_DECLARATIONS[4],
			"{",
			"	if (index < (unsigned int)array[0]) goto insert;",
			"",
			"	return 0;",
			"",
			"	insert:",
			"	array[index + 1] = temp;",
			"	return 1;",
			"}"
		};

		public static readonly string[] insertToFloatArray = new string[]
		{
			HELPER_FUNCTION_DECLARATIONS[5],
			"{",
			"	if (index < (unsigned int)array[0]) goto insert;",
			"",
			"	return 0;",
			"",
			"	insert:",
			"	array[index + 1] = temp;",
			"	return 1;",
			"}"
		};

		public static readonly string[] insertToStringArray = new string[]
		{
			HELPER_FUNCTION_DECLARATIONS[6],
			"{",
			"	if (index < (unsigned int)array[0]) goto insert;",
			"",
			"	return 0;",
			"",
			"	insert:",
			"	array[index + 1] = temp;",
			"	return 1;",
			"}"
		};

		public static readonly string[] stringConcatenation = new string[]
		{
			HELPER_FUNCTION_DECLARATIONS[7],
			"{",
			"	unsigned int first_size = get_string_length(first);",
			"	unsigned int second_size = get_string_length(second);",
			"",
			"	unsigned int total_size = first_size + second_size;",
			"	total_size = total_size + 1;",
			"",
			"	char* concatenation = malloc(sizeof(char) * total_size);",
			"",
			"	int i = 0;",
			"	int n = 0;",
			"",
			"	goto check_first_loop_condition;",
			"",
			"	first_loop:",
			"	concatenation[n] = first[i];",
			"	n = n + 1;",
			"	i = i + 1;",
			"",
			"	check_first_loop_condition:",
			"	if (i < first_size) goto first_loop;",
			"",
			"	i = 0;",
			"",
			"	goto check_second_loop_condition;",
			"",
			"	second_loop:",
			"	concatenation[n] = second[i];",
			"	n = n + 1;",
			"	i = i + 1;",
			"",
			"	check_second_loop_condition:",
			"	if (i < second_size) goto second_loop;",
			"",
			"	concatenation[n] = '\\0';",
			"",
			"	return concatenation;",
			"}"
		};

		public static readonly string[][] HELPER_FUNCTIONS = new string[][] {
			getStringLengthFunction,
			loadFromFloatArray,
			loadFromIntArrayFunction,
			loadFromStringArray,
			insertToFloatArray,
			insertToIntArray,
			insertToStringArray,
			stringConcatenation
		};
	}
}

