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
			TOKEN_TYPE_STRINGS[TokenType.WHILE_LOOP],
			TOKEN_TYPE_STRINGS[TokenType.READ],
			TOKEN_TYPE_STRINGS[TokenType.WRITELN],
			TOKEN_TYPE_STRINGS[TokenType.ASSERT],
			IDENTIFIER_STR,
			TOKEN_TYPE_STRINGS[TokenType.END_OF_FILE]
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
	}
}

