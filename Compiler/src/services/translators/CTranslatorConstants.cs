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
		public static readonly string[] LIBRARIES = new string[] {"stdio.h", "stdlib.h", "string.h"};
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
	}
}

