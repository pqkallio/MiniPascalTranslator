using System;

namespace Compiler
{
	/// <summary>
	/// Constants used when reporting error's
	/// </summary>
	public class ErrorConstants
	{
		public static readonly string SCANNER_ERROR_TITLE = "Scanner error";
		public static readonly string SYNTAX_ERROR_TITLE = "Syntax error";
		public static readonly string SEMANTIC_ERROR_TITLE = "Semantic error";
		public static readonly string INTEGER_OVERFLOW_ERROR_TITLE = "Integer overflow error";
		public static readonly string RUNTIME_EXCEPTION_TITLE = "Runtime exception";
		public static readonly string DIVISION_BY_ZERO_TITLE = "Division by zero";

		public static readonly string STRING_LITERAL_ERROR_MESSAGE = "error while scanning string literal";
		public static readonly string TOKEN_ERROR_MESSAGE = "error while scanning token";
		public static readonly string SYNTAX_ERROR_MESSAGE = "error while parsing token";
		public static readonly string SEMANTIC_ERROR_MESSAGE = "undefined semantic error";
		public static readonly string UNINITIALIZED_VARIABLE_ERROR_MESSAGE = "Uninitialized variable";
		public static readonly string ILLEGAL_TYPE_ERROR_MESSAGE = "the type is not supported by this expression";
		public static readonly string ILLEGAL_ASSIGNMENT_ERROR_MESSAGE = "the for-loop's control variable cannot be reassigned inside the loop";
		public static readonly string ILLEGAL_ARRAY_ELEMENT_TYPE_ERROR_MSG = "arrays can contain only string, integer, real or boolean values";
		public static readonly string DECLARATION_ERROR_MESSAGE = "the variable has already been declared";
		public static readonly string INTEGER_OVERFLOW_ERROR_MESSAGE = "integer values must be within a range of -2147483648 to 2147483647 (32-bit integer)";
		public static readonly string NULL_POINTER_ERROR_MESSAGE = "null pointer encountered";
		public static readonly string LINEBREAK_IN_STR_LITERAL_MESSAGE = "string literal mustn't span multiple lines";
		public static readonly string EOF_WHILE_SCANNING_MESSAGE = "EOF encountered while parsing string literal";
		public static readonly string NOT_AN_INTEGER_MESSAGE = "the read value is not an integer";
		public static readonly string RUNTIME_ERROR_MESSAGE = "unexpected runtime error";
		public static readonly string DIVISION_BY_ZERO_MESSAGE = "division by zero";
		public static readonly string INVALID_IDENTIFIER_MESSAGE = "the identifiers must begin with a letter followed by zero or more numbers, letters and underscores";
		public static readonly string INVALID_ID_MESSAGE = "reserved keyword used as variable identifier";
		public static readonly string INVALID_REAL_NUM_MESSAGE = "a real number's exponent part must be one of the following forms: e<digits>, e+<digits> or e-<digits>";
	}
}

