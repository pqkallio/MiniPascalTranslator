using System;
using System.Collections.Generic;

namespace Compiler
{
	public class ScannerConstants
	{
		public static readonly Dictionary<char, string> WHITESPACES = new Dictionary<char, string> ()
		{
			{' ', null},
			{'\t', null}
		};

		public static readonly Dictionary<string, TokenType> RESERVED_SEQUENCES = new Dictionary<string, TokenType> ()	
		{
			{"var", TokenType.VAR},
			{"false", TokenType.BOOLEAN_VAL_FALSE},
			{"true", TokenType.BOOLEAN_VAL_TRUE},
			{"end", TokenType.END},
			{"integer", TokenType.TYPE_INTEGER},
			{"do", TokenType.DO_WHILE},
			{"read", TokenType.READ},
			{"writeln", TokenType.WRITELN},
			{"string", TokenType.TYPE_STRING},
			{"Boolean", TokenType.TYPE_BOOLEAN},
			{"assert", TokenType.ASSERT},
			{"size", TokenType.SIZE},
			{"array", TokenType.ARRAY},
			{"real", TokenType.TYPE_REAL},
			{"or", TokenType.BINARY_OP_LOG_OR},
			{"and", TokenType.BINARY_OP_LOG_AND},
			{"not", TokenType.UNARY_OP_LOG_NEG},
			{"program", TokenType.PROGRAM},
			{"procedure", TokenType.PROCEDURE},
			{"function", TokenType.FUNCTION},
			{"of", TokenType.OF},
			{"begin", TokenType.BEGIN},
			{"return", TokenType.RETURN},
			{"if", TokenType.IF},
			{"then", TokenType.THEN},
			{"else", TokenType.ELSE},
			{"while", TokenType.WHILE_LOOP},
		};

		public static readonly Dictionary<string, TokenType> KEYWORDS = new Dictionary<string, TokenType> ()	
		{
			{"var", TokenType.VAR},
			{"end", TokenType.END},
			{"do", TokenType.DO_WHILE},
			{"assert", TokenType.ASSERT},
			{"array", TokenType.ARRAY},
			{"or", TokenType.BINARY_OP_LOG_OR},
			{"and", TokenType.BINARY_OP_LOG_AND},
			{"not", TokenType.UNARY_OP_LOG_NEG},
			{"program", TokenType.PROGRAM},
			{"procedure", TokenType.PROCEDURE},
			{"function", TokenType.FUNCTION},
			{"of", TokenType.OF},
			{"begin", TokenType.BEGIN},
			{"return", TokenType.RETURN},
			{"if", TokenType.IF},
			{"then", TokenType.THEN},
			{"else", TokenType.ELSE},
			{"while", TokenType.WHILE_LOOP},
		};

		public static readonly Dictionary<char, TokenType> INDEPENDENT_CHARS = new Dictionary<char, TokenType> ()
		{
			{'=', TokenType.BINARY_OP_LOG_EQ},
			{';', TokenType.END_STATEMENT},
			{',', TokenType.COMMA},
			{'(', TokenType.PARENTHESIS_LEFT}, 
			{')', TokenType.PARENTHESIS_RIGHT},
			{'[', TokenType.BRACKET_LEFT}, 
			{']', TokenType.BRACKET_RIGHT},
			{'*', TokenType.BINARY_OP_MUL},
			{'/', TokenType.BINARY_OP_DIV},
			{'+', TokenType.SIGN_PLUS}, 
			{'-', TokenType.SIGN_MINUS},
			{'%', TokenType.BINARY_OP_MOD}
		};

		public static readonly Dictionary<char, Dictionary<char, TokenType>> SUCCESSOR_DEPENDENTS = new Dictionary<char, Dictionary<char, TokenType>> ()
		{
			{':', new Dictionary<char, TokenType> () 
				{
					{'=', TokenType.ASSIGN}
				}
			},
			{'<', new Dictionary<char, TokenType> () 
				{
					{'=', TokenType.BINARY_OP_LOG_LTE},
					{'>', TokenType.BINARY_OP_LOG_NEQ}
				}
			},
			{'>', new Dictionary<char, TokenType> () 
				{
					{'=', TokenType.BINARY_OP_LOG_GTE}
				}
			}
		};

		public static readonly Dictionary<char, TokenType> NO_SUCCESSIVE_CHAR = new Dictionary<char, TokenType> ()
		{
			{':', TokenType.SET_TYPE},
			{'<', TokenType.BINARY_OP_LOG_LT},
			{'>', TokenType.BINARY_OP_LOG_GT},
		};

		public static readonly char ESCAPE_CHAR = '\\';
		public static readonly char STRING_DELIMITER = '"';
		public static readonly char NULL_CHAR = '\0';
		public static readonly char NEWLINE = '\n';
		public static readonly char DOT = '.';
		public static readonly char UNDERSCORE = '_';
		public static readonly char CURLY_BRACE_LEFT = '{';
		public static readonly char CURLY_BRACE_RIGHT = '}';
		public static readonly char ASTERISK = '*';
		public static readonly char EXPONENT = 'e';
		public static readonly char SIGN_MINUS = '-';
		public static readonly char SIGN_PLUS = '+';
	}
}

