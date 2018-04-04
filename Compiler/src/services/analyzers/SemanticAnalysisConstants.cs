using System;
using System.Collections.Generic;

namespace Compiler
{
	public class SemanticAnalysisConstants
	{
		public static readonly bool DEFAULT_BOOL_VALUE = false;
		public static readonly int DEFAULT_INTEGER_VALUE = 0;
		public static readonly string DEFAULT_STRING_VALUE = "";

		public static readonly Dictionary<TokenType, Dictionary<TokenType, string>> LEGIT_OPERATIONS = new Dictionary<TokenType, Dictionary<TokenType, string>> ()
		{
			{TokenType.INTEGER_VAL, 
				new Dictionary<TokenType, string> () 
				{
					{TokenType.UNARY_OP_POSITIVE, null},
					{TokenType.BINARY_OP_DIV, null},
					{TokenType.BINARY_OP_MUL, null},
					{TokenType.BINARY_OP_NO_OP, null},
					{TokenType.UNARY_OP_NEGATIVE, null},
					{TokenType.BINARY_OP_LOG_EQ, null},
					{TokenType.BINARY_OP_LOG_LT, null},
				}
			},
			{TokenType.STRING_VAL, 
				new Dictionary<TokenType, string> () 
				{
					{TokenType.UNARY_OP_POSITIVE, null},
					{TokenType.BINARY_OP_NO_OP, null},
					{TokenType.BINARY_OP_LOG_EQ, null},
					{TokenType.BINARY_OP_LOG_LT, null}
				}
			},
			{TokenType.BOOLEAN_VAL_FALSE, 
				new Dictionary<TokenType, string> () 
				{
					{TokenType.BINARY_OP_NO_OP, null},
					{TokenType.BINARY_OP_LOG_EQ, null},
					{TokenType.BINARY_OP_LOG_LT, null},
					{TokenType.BINARY_OP_LOG_AND, null},
					{TokenType.UNARY_OP_LOG_NEG, null}
				}
			}
		};

		public static readonly Dictionary<TokenType, string> LOGICAL_OPERATIONS = new Dictionary<TokenType, string> ()
		{
			{TokenType.BINARY_OP_LOG_AND, null},
			{TokenType.BINARY_OP_LOG_EQ, null},
			{TokenType.BINARY_OP_LOG_LT, null},
			{TokenType.UNARY_OP_LOG_NEG, null}
		};
	}
}

