using System;
using System.Collections.Generic;

namespace Compiler
{
	/// <summary>
	/// Constants used when reporting error's
	/// </summary>
	public class ErrorConstants
	{
		private static readonly Dictionary<TokenType, string> TOKEN_TYPE_STRINGS = StringFormattingConstants.TOKEN_TYPE_STRINGS;

		public static readonly string SCANNER_ERROR_TITLE = "Scanner error";
		public static readonly string SYNTAX_ERROR_TITLE = "Syntax error";
		public static readonly string SEMANTIC_ERROR_TITLE = "Semantic error";
		public static readonly string INTEGER_OVERFLOW_ERROR_TITLE = "Integer overflow error";
		public static readonly string RUNTIME_EXCEPTION_TITLE = "Runtime exception";
		public static readonly string DIVISION_BY_ZERO_TITLE = "Division by zero";
		public static readonly string INVALID_RETURN_VALUE_TITLE = "Invalid return value";

		public static readonly string STRING_LITERAL_ERROR_MESSAGE = "error while scanning string literal";
		public static readonly string TOKEN_ERROR_MESSAGE = "error while scanning token";
		public static readonly string SYNTAX_ERROR_MESSAGE = "error while parsing token";
		public static readonly string SEMANTIC_ERROR_MESSAGE = "undefined semantic error";
		public static readonly string UNINITIALIZED_VARIABLE_ERROR_MESSAGE = "Uninitialized variable";
		public static readonly string ILLEGAL_TYPE_ERROR_MESSAGE = "the type is not supported by this expression";
		public static readonly string ILLEGAL_ASSIGNMENT_ERROR_MESSAGE = "the variable's and the expression's types don't match";
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
		public static readonly string ALL_CODE_PATHS_DONT_RETURN_MSG = "some of the functions execution paths don't return a value";
		public static readonly string FUNCTION_DOESNT_RETURN_MSG = "the function doesn't return a value";
		public static readonly string ILLEGAL_ARRAY_ACCESS_ERROR_MSG = "the variable is not an array";
		public static readonly string ILLEGAL_ARRAY_INDEX_ERROR_MSG = "the array-indexing expression must evaluate to an integer";
		public static readonly string NOT_A_VALID_FUNCTION_ERROR_MSG = "the identifier doesn't point to a valid function";
		public static readonly string INVALID_ARG_COUNT_ERROR_MSG = "the amount of arguments given doesn't match with the amount requested";
		public static readonly string UNDECLARED_VARIABLE_ERROR_MSG = "the variable has not been declared yet";
		public static readonly string NO_IDS_TO_DECLARE_ERROR_MSG = "the 'var' keyword must be followed by a comma separated list of one or more identifiers";
		public static readonly string DANGLING_COMMA_ERROR_MSG = "a comma should must be followed by an identifier";

		public static string INVALID_RETURN_VALUE_MSG(ReturnStatement node, TokenType requiredType)
		{
			return String.Format ("the required return value type is {0}, but instead {1} was given", TOKEN_TYPE_STRINGS[requiredType], TOKEN_TYPE_STRINGS[node.EvaluationType]);
		}

		public static string INVALID_ARGUMENT_ERROR_MSG (int position, TokenType parameterEvaluation, TokenType argumentEvaluation)
		{
			if (parameterEvaluation == TokenType.UNDEFINED || argumentEvaluation == TokenType.UNDEFINED) {
				return String.Format ("argument {0} needs to be a variable reference", position);
			}

			return String.Format ("argument {0} is expected to be {1}, but instead a {2} was given", position, TOKEN_TYPE_STRINGS[parameterEvaluation], TOKEN_TYPE_STRINGS[argumentEvaluation]);
		}
	}
}

