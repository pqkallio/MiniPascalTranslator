using System;
using System.Collections.Generic;

namespace Compiler
{
	public class StringFormattingConstants
	{
		public static readonly bool UNIX = Environment.OSVersion.ToString ().ToLower ().StartsWith ("unix"); 

		public static readonly Dictionary<TokenType, string> TOKEN_TYPE_STRINGS = new Dictionary<TokenType, string> () 
		{
			{TokenType.VAR, "var"},
			{TokenType.WHILE_LOOP, "for"},
			{TokenType.END, "end"},
			{TokenType.TYPE_INTEGER, "int"},
			{TokenType.DO_WHILE, "do"},
			{TokenType.READ, "read"},
			{TokenType.WRITELN, "print"},
			{TokenType.TYPE_STRING, "string"},
			{TokenType.TYPE_BOOLEAN, "bool"},
			{TokenType.ASSERT, "assert"},
			{TokenType.BINARY_OP_LOG_LT, "<"},
			{TokenType.BINARY_OP_LOG_EQ, "="},
			{TokenType.BINARY_OP_LOG_AND, "&"},
			{TokenType.UNARY_OP_LOG_NEG, "!"},
			{TokenType.END_STATEMENT, ";"},
			{TokenType.PARENTHESIS_LEFT, "("},
			{TokenType.PARENTHESIS_RIGHT, ")"},
			{TokenType.BINARY_OP_MUL, "*"},
			{TokenType.UNARY_OP_POSITIVE, "+"},
			{TokenType.UNARY_OP_NEGATIVE, "-"},
			{TokenType.BINARY_OP_DIV, "/"},
			{TokenType.SET_TYPE, ":"},
			{TokenType.ASSIGN, ":="},
			{TokenType.ID, "identifier"},
			{TokenType.END_OF_FILE, "end of file"},
			{TokenType.UNDEFINED, "undefined"}
		};

		public static readonly string LINEBREAK = UNIX ? "\n" : "\r\n";
		public static readonly char UTF8_SMALL_LETTERS_START = (char)0x41;
		public static readonly char UTF8_SMALL_LETTERS_END = (char)0x5a;
		public static readonly char UTF8_CAPITAL_LETTERS_START = (char)0x61;
		public static readonly char UTF8_CAPITAL_LETTERS_END = (char)0x7a;
		public static readonly char UTF8_LOWLINE = (char)0x5f;
		public static readonly char UTF8_NUMERIC_START = (char)0x30;
		public static readonly char UTF8_NUMERIC_END = (char)0x39;

		public static readonly string ASSERTION_FAILED = "Assertion failed: ";
		public static readonly Tuple<char, char> PRINT_ROW_AND_COLUMN_PARENTHESES = Tuple.Create('[', ']');
		public static readonly string PRINT_ROW = "Line: ";
		public static readonly string PRINT_COL = "Column: ";
		public static readonly string PRINT_ROW_COL_DELIMITER = "; ";

		public static readonly Dictionary<char, char> UTF8_VALID_ID_CHAR_RANGES = new Dictionary<char, char> ()
		{
			{UTF8_SMALL_LETTERS_START, UTF8_SMALL_LETTERS_END},
			{UTF8_CAPITAL_LETTERS_START, UTF8_CAPITAL_LETTERS_END},
			{UTF8_LOWLINE, UTF8_LOWLINE},
			{UTF8_NUMERIC_START, UTF8_NUMERIC_END}
		};

	}
}