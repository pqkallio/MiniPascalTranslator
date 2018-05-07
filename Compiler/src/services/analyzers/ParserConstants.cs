using System;
using System.Collections.Generic;

namespace Compiler
{
	public class ParserConstants
	{
		private static readonly Dictionary<TokenType, string> TOKEN_TYPE_STRINGS = StringFormattingConstants.TOKEN_TYPE_STRINGS;

		public static readonly Dictionary<TokenType, string> BINARY_OPERATIONS = new Dictionary<TokenType, string> ()
		{
			{TokenType.UNARY_OP_POSITIVE, null},
			{TokenType.BINARY_OP_DIV, null},
			{TokenType.BINARY_OP_LOG_AND, null},
			{TokenType.BINARY_OP_LOG_EQ, null},
			{TokenType.BINARY_OP_LOG_LT, null},
			{TokenType.BINARY_OP_MUL, null},
			{TokenType.UNARY_OP_NEGATIVE, null}
		};

		public static readonly Dictionary<TokenType, string> SIMPLE_TYPES = new Dictionary<TokenType, string> ()
		{
			{TokenType.TYPE_INTEGER, null},
			{TokenType.TYPE_BOOLEAN, null},
			{TokenType.TYPE_REAL, null},
			{TokenType.TYPE_STRING, null},
			{TokenType.INTEGER_VAL, null},
			{TokenType.BOOLEAN_VAL, null},
			{TokenType.REAL_VAL, null},
			{TokenType.STRING_VAL, null}
		};

		public static readonly Dictionary<TokenType, string> STATEMENT_FASTFORWARD_TO = new Dictionary<TokenType, string> () 
		{
			{TokenType.END_STATEMENT, null},
			{TokenType.END_OF_FILE, null}
		};

		public static readonly Dictionary<TokenType, string> BLOCK_DEF_FASTFORWARD_TO = new Dictionary<TokenType, string> () 
		{
			{TokenType.DO_WHILE, null},
			{TokenType.END_STATEMENT, null},
			{TokenType.END_OF_FILE, null}
		};

		public static readonly Dictionary<TokenType, string> PROGRAM_FASTFORWARD_TO = new Dictionary<TokenType, string> ()
		{
			{TokenType.DOT, null},
			{TokenType.END_OF_FILE, null}
		};

		public static readonly Dictionary<TokenType, string> FUNCTION_AND_PROCEDURE_FASTFORWARD_TO = new Dictionary<TokenType, string> ()
		{
			{TokenType.DOT, null},
			{TokenType.END_OF_FILE, null}
		};

		public static readonly Dictionary<TokenType, string> PARAMETERS_FASTFORWARD_TO = new Dictionary<TokenType, string> ()
		{
			{TokenType.PARENTHESIS_RIGHT, null},
			{TokenType.END_OF_FILE, null}
		};

		public static readonly Dictionary<TokenType, string> FUNCTION_FASTFORWARD_TO = new Dictionary<TokenType, string> ()
		{
			{TokenType.END, null},
			{TokenType.END_OF_FILE, null}
		};
			
		public static readonly string IDENTIFIER_STR = "identifier";
		public static readonly string EOF_STR = "end of file";
		public static readonly string OPERAND_STR = "operand literal";
		public static readonly string EXPRESSION_STR = "expression";
		public static readonly string END_OF_EXPR_STR = "end of expression";
		public static readonly string INT_VAL_STR = "integer";
		public static readonly string STR_VAL_STR = "string";
		public static readonly string BOOL_VAL_STR = "boolean";
		public static readonly string ID_LIST = "comma-separated list of identifiers";

		// Expectation sets
		public static readonly string[] EXPECTATION_SET_PROGRAM = new string[] 
		{
			TOKEN_TYPE_STRINGS[TokenType.VAR],
			TOKEN_TYPE_STRINGS[TokenType.WHILE_LOOP],
			TOKEN_TYPE_STRINGS[TokenType.READ],
			TOKEN_TYPE_STRINGS[TokenType.WRITELN],
			TOKEN_TYPE_STRINGS[TokenType.ASSERT],
			IDENTIFIER_STR,
			EOF_STR
		};

		public static readonly string[] EXPECTATION_SET_DECLARATION = new string[]
		{
			ID_LIST,
			TOKEN_TYPE_STRINGS[TokenType.SET_TYPE]
		};

		public static readonly string[] EXPECTATION_SET_IDS_FOR_READ = new string[]
		{
			ID_LIST
		};

		public static readonly string[] EXPECTATION_SET_FUNCTIONS_AND_PROCEDURES = new string[] 
		{
			TOKEN_TYPE_STRINGS[TokenType.FUNCTION],
			TOKEN_TYPE_STRINGS[TokenType.PROCEDURE],
			TOKEN_TYPE_STRINGS[TokenType.BEGIN]
		};

		public static readonly string[] EXPECTATION_SET_PARAMETER = new string[]
		{
			TOKEN_TYPE_STRINGS[TokenType.VAR],
			TOKEN_TYPE_STRINGS[TokenType.ID]
		};

		public static readonly string[] EXPECTATION_SET_PARAMETER_TAIL = new string[]
		{
			TOKEN_TYPE_STRINGS[TokenType.COMMA],
			TOKEN_TYPE_STRINGS[TokenType.PARENTHESIS_RIGHT]
		};

		public static readonly string[] EXPECTATION_SET_PARAMETERS = new string[]
		{
			TOKEN_TYPE_STRINGS[TokenType.VAR],
			TOKEN_TYPE_STRINGS[TokenType.ID],
			TOKEN_TYPE_STRINGS[TokenType.PARENTHESIS_RIGHT]
		};

		public static readonly string[] EXPECTATION_SET_STATEMENTS = new string[] 
		{
			TOKEN_TYPE_STRINGS[TokenType.VAR],
			TOKEN_TYPE_STRINGS[TokenType.RETURN],
			TOKEN_TYPE_STRINGS[TokenType.BEGIN],
			TOKEN_TYPE_STRINGS[TokenType.IF],
			TOKEN_TYPE_STRINGS[TokenType.WHILE_LOOP],
			TOKEN_TYPE_STRINGS[TokenType.READ],
			TOKEN_TYPE_STRINGS[TokenType.WRITELN],
			TOKEN_TYPE_STRINGS[TokenType.ASSERT],
			IDENTIFIER_STR,
			TOKEN_TYPE_STRINGS[TokenType.END_OF_FILE]
		};

		public static readonly string[] EXPECTATION_SET_ID_STATEMENT = new string[]
		{
			TOKEN_TYPE_STRINGS[TokenType.BRACKET_LEFT],
			TOKEN_TYPE_STRINGS[TokenType.ASSIGN],
			TOKEN_TYPE_STRINGS[TokenType.PARENTHESIS_LEFT]
		};

		public static readonly string[] EXPECTATION_SET_STATEMENT = new string[] 
		{
			TOKEN_TYPE_STRINGS[TokenType.VAR],
			TOKEN_TYPE_STRINGS[TokenType.WHILE_LOOP],
			TOKEN_TYPE_STRINGS[TokenType.READ],
			TOKEN_TYPE_STRINGS[TokenType.WRITELN],
			TOKEN_TYPE_STRINGS[TokenType.ASSERT],
			IDENTIFIER_STR
		};

		public static readonly string[] EXPECTATION_SET_EOF = new string[] 
		{
			TOKEN_TYPE_STRINGS[TokenType.END_OF_FILE]
		};

		public static readonly string[] EXPECTATION_SET_DECLARATION_TYPE = new string[] 
		{
			TOKEN_TYPE_STRINGS[TokenType.TYPE_STRING],
			TOKEN_TYPE_STRINGS[TokenType.TYPE_INTEGER],
			TOKEN_TYPE_STRINGS[TokenType.TYPE_BOOLEAN]
		};

		public static readonly string[] EXPECTATION_SET_ASSIGN = new string[] 
		{
			TOKEN_TYPE_STRINGS[TokenType.ASSIGN],
			TOKEN_TYPE_STRINGS[TokenType.END_STATEMENT]
		};

		public static readonly string[] EXPECTATION_SET_EXPRESSION = new string[] 
		{
			EXPRESSION_STR
		};

		public static readonly string[] EXPECTATION_SET_BINOP = new string[]
		{
			OPERAND_STR,
			END_OF_EXPR_STR
		};

		public static readonly string[] EXPECTATION_SET_ID_VAL = new string[]
		{
			INT_VAL_STR,
			STR_VAL_STR,
			BOOL_VAL_STR
		};

		public static readonly string[] EXPECTATION_SET_ARGUMENTS = new string[]
		{
			EXPRESSION_STR,
			TOKEN_TYPE_STRINGS[TokenType.COMMA],
			TOKEN_TYPE_STRINGS[TokenType.PARENTHESIS_RIGHT]
		};

		public static readonly string[] EXPECTATION_SET_LITERAL = new string[]
		{
			TOKEN_TYPE_STRINGS[TokenType.STRING_VAL],
			TOKEN_TYPE_STRINGS[TokenType.INTEGER_VAL],
			TOKEN_TYPE_STRINGS[TokenType.REAL_VAL],
			TOKEN_TYPE_STRINGS[TokenType.BOOLEAN_VAL]
		};

		public static readonly string[] EXPECTATION_SET_RETURN_STATEMENT = new string[]
		{
			EXPRESSION_STR,
			TOKEN_TYPE_STRINGS[TokenType.END_STATEMENT],
			TOKEN_TYPE_STRINGS[TokenType.END],
			TOKEN_TYPE_STRINGS[TokenType.ELSE]
		};

		public static readonly string[] EXPECTATION_SET_TYPE = new string[]
		{
			TOKEN_TYPE_STRINGS[TokenType.TYPE_STRING],
			TOKEN_TYPE_STRINGS[TokenType.TYPE_INTEGER],
			TOKEN_TYPE_STRINGS[TokenType.TYPE_REAL],
			TOKEN_TYPE_STRINGS[TokenType.TYPE_BOOLEAN],
			TOKEN_TYPE_STRINGS[TokenType.TYPE_ARRAY]
		};
	}
}

