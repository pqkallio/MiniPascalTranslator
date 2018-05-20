﻿using System;
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
		public static readonly string IF = "if";
		public static readonly string GOTO = "goto";
		public static readonly string MEM_ALLOCATION = "malloc";
		public static readonly string MEM_RELEASE = "free";
		public static readonly string MEM_POINTER = "*";
		public static readonly string MEM_ADDRESS = "&";
		public static readonly string SIZE_OF = "sizeof";
		public static readonly string ASSIGNMENT = "=";
		public static readonly string ERROR_LABEL = "error";
		public static readonly string ASSERTION_FAILED_MESSAGE = "\"Assertion failed.\"";
		public static readonly string STRING_DELIMITER = "\"";
		public static readonly string ARRAY_SIZE_INDEX = "0";

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

		public static readonly Dictionary<TokenType, string> ADDRESS_PREFIXES = new Dictionary<TokenType, string> ()
		{
			{TokenType.INTEGER_VAL, MEM_ADDRESS},
			{TokenType.REAL_VAL, MEM_ADDRESS},
			{TokenType.BOOLEAN_VAL, MEM_ADDRESS},
			{TokenType.STRING_VAL, ""},
			{TokenType.TYPE_ARRAY, ""}
		};

		public static readonly Dictionary<TokenType, string> ARRAY_ACCESS_FUNCTION_CALLS = new Dictionary<TokenType, string> ()
		{
			{TokenType.BOOLEAN_VAL, "load_from_int_array"},
			{TokenType.INTEGER_VAL, "load_from_int_array"},
			{TokenType.REAL_VAL, "load_from_float_array"},
			{TokenType.STRING_VAL, "load_from_string_array"},
		};

		public static readonly Dictionary<TokenType, string> ARRAY_INSERTION_FUNCTION_CALLS = new Dictionary<TokenType, string> ()
		{
			{TokenType.BOOLEAN_VAL, "insert_to_int_array"},
			{TokenType.INTEGER_VAL, "insert_to_int_array"},
			{TokenType.REAL_VAL, "insert_to_float_array"},
			{TokenType.STRING_VAL, "insert_to_string_array"},
		};

		public static readonly Dictionary<TokenType, string> PRINTING_FUNCTION_CALLS = new Dictionary<TokenType, string> ()
		{
			{TokenType.BOOLEAN_VAL, "print_int"},
			{TokenType.INTEGER_VAL, "print_int"},
			{TokenType.REAL_VAL, "print_float"},
			{TokenType.STRING_VAL, "print_string"},
		};

		public static readonly string[] HELPER_FUNCTION_DECLARATIONS = new string[]
		{
			"unsigned int get_string_length(char* string)",
			"void load_from_int_array(int* array, unsigned int index, int* temp)",
			"void load_from_float_array(float* array, unsigned int index, float* temp)",
			"void load_from_string_array(char** array, unsigned int index, char** temp)",
			"int insert_to_int_array(int* array, unsigned int index, int temp)",
			"int insert_to_float_array(float* array, unsigned int index, float temp)",
			"int insert_to_string_array(char** array, unsigned int index, char* temp)",
			"char* string_concatenation(char* first, char* second)",
			"int compare_strings(char* lhs, char* rhs)",
			"void print_int(int integer)",
			"void print_float(float real)",
			"void print_string(char* string)",
			"void print_linebreak()"
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

		public static readonly string[] stringComparison = new string[]
		{
			HELPER_FUNCTION_DECLARATIONS[8],
			"{",
			"	unsigned int lhs_length = get_string_length(lhs);",
			"	unsigned int rhs_length = get_string_length(rhs);",
			"	int comparison;",
			"	int index;",
			"	char c_lhs;",
			"	char c_rhs;",
			"	int lhs_length_gt_index;",
			"	int rhs_length_gt_index;",
			"",
			"	goto check_condition;",
			"",
			"	compare:",
			"	c_lhs = lhs[index];",
			"	c_rhs = rhs[index];",
			"	comparison = c_lhs - c_rhs;",
			"",
			"	if (comparison < 0) goto end;",
			"	if (comparison > 0) goto end;",
			"",
			"	index = index + 1;",
			"",
			"	check_condition:",
			"	lhs_length_gt_index = lhs_length > index;",
			"	rhs_length_gt_index = rhs_length > index;",
			"	comparison = lhs_length_gt_index - rhs_length_gt_index;",
			"",
			"	if (!lhs_length_gt_index) goto end;",
			"	if (!rhs_length_gt_index) goto end;",
			"",
			"	goto compare;",
			"",
			"	end:",
			"	return comparison;",
			"}"
		};

		public static readonly string[] printInt = new string[] {
			HELPER_FUNCTION_DECLARATIONS[9],
			"{",
			"	printf(\"%d\", integer);",
			"}"
		};

		public static readonly string[] printReal = new string[] {
			HELPER_FUNCTION_DECLARATIONS[10],
			"{",
			"	printf(\"%f\", real);",
			"}"
		};

		public static readonly string[] printString = new string[] {
			HELPER_FUNCTION_DECLARATIONS[11],
			"{",
			"	printf(\"%s\", string);",
			"}"
		};

		public static readonly string[] printLinebreak = new string[] {
			HELPER_FUNCTION_DECLARATIONS[12],
			"{",
			"	printf(\"\\n\");",
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
			stringConcatenation,
			stringComparison,
			printInt,
			printReal,
			printString,
			printLinebreak
		};
	}
}
