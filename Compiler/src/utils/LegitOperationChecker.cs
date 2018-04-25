using System;
using System.Collections.Generic;

namespace Compiler
{
	public static class LegitOperationChecker
	{
		private static readonly Dictionary<TokenType, string> integer = new Dictionary<TokenType, string> ()
		{
			{TokenType.INTEGER_VAL, null}
		};

		private static readonly Dictionary<TokenType, Dictionary<TokenType, string>> integerToInteger = new Dictionary<TokenType, Dictionary<TokenType, string>> ()
		{
			{TokenType.INTEGER_VAL, integer}
		};

		private static readonly Dictionary<TokenType, string> real = new Dictionary<TokenType, string> ()
		{
			{TokenType.REAL_VAL, null}
		};

		private static readonly Dictionary<TokenType, Dictionary<TokenType, string>> realToReal = new Dictionary<TokenType, Dictionary<TokenType, string>> ()
		{
			{TokenType.REAL_VAL, real}
		};

		private static readonly Dictionary<TokenType, string> numerics = new Dictionary<TokenType, string> ()
		{
			{TokenType.INTEGER_VAL, null},
			{TokenType.REAL_VAL, null}
		};

		private static readonly Dictionary<TokenType, Dictionary<TokenType, string>> numericsToNumerics = new Dictionary<TokenType, Dictionary<TokenType, string>> ()
		{
			{TokenType.INTEGER_VAL, numerics},
			{TokenType.REAL_VAL, numerics}
		};

		private static readonly Dictionary<TokenType, string> boolean = new Dictionary<TokenType, string> ()
		{
			{TokenType.BOOLEAN_VAL, null}
		};

		private static readonly Dictionary<TokenType, Dictionary<TokenType, string>> booleanToBoolean = new Dictionary<TokenType, Dictionary<TokenType, string>> ()
		{
			{TokenType.BOOLEAN_VAL, boolean}
		};

		private static readonly Dictionary<TokenType, string> stringd = new Dictionary<TokenType, string> ()
		{
			{TokenType.STRING_VAL, null}
		};

		private static readonly Dictionary<TokenType, Dictionary<TokenType, string>> stringToString = new Dictionary<TokenType, Dictionary<TokenType, string>> ()
		{
			{TokenType.STRING_VAL, stringd}
		};

		private static readonly Dictionary<TokenType, Dictionary<TokenType, string>> numericsAndStrings = new Dictionary<TokenType, Dictionary<TokenType, string>> ()
		{
			{TokenType.STRING_VAL, stringd},
			{TokenType.INTEGER_VAL, numerics},
			{TokenType.REAL_VAL, numerics}
		};

		private static readonly Dictionary<TokenType, Dictionary<TokenType, string>> typesToTypes = new Dictionary<TokenType, Dictionary<TokenType, string>> ()
		{
			{TokenType.STRING_VAL, stringd},
			{TokenType.INTEGER_VAL, integer},
			{TokenType.REAL_VAL, real},
			{TokenType.BOOLEAN_VAL, boolean},
		};

		private static readonly Dictionary<TokenType, Dictionary<TokenType, string>> unaryOps = new Dictionary<TokenType, Dictionary<TokenType, string>> ()
		{
			{TokenType.UNARY_OP_LOG_NEG, boolean},
			{TokenType.UNARY_OP_NEGATIVE, numerics},
			{TokenType.UNARY_OP_POSITIVE, numerics}
		};


		private static readonly Dictionary<TokenType, Dictionary<TokenType, Dictionary<TokenType, string>>> binaryOps = new Dictionary<TokenType, Dictionary<TokenType, Dictionary<TokenType, string>>> ()
		{
			{TokenType.BINARY_OP_ADD, numericsAndStrings},
			{TokenType.BINARY_OP_DIV, numericsToNumerics},
			{TokenType.BINARY_OP_LOG_AND, booleanToBoolean},
			{TokenType.BINARY_OP_LOG_EQ, typesToTypes},
			{TokenType.BINARY_OP_LOG_GT, typesToTypes},
			{TokenType.BINARY_OP_LOG_GTE, typesToTypes},
			{TokenType.BINARY_OP_LOG_LT, typesToTypes},
			{TokenType.BINARY_OP_LOG_LTE, typesToTypes},
			{TokenType.BINARY_OP_LOG_NEQ, typesToTypes},
			{TokenType.BINARY_OP_LOG_OR, booleanToBoolean},
			{TokenType.BINARY_OP_MOD, integerToInteger},
			{TokenType.BINARY_OP_MUL, numericsToNumerics},
			{TokenType.BINARY_OP_SUB, numericsToNumerics}
		};

		private static readonly Dictionary<TokenType, Dictionary<TokenType, string>> tailCalls = new Dictionary<TokenType, Dictionary<TokenType, string>> ()
		{
			{TokenType.SIZE, new Dictionary<TokenType, string> ()
				{
					{TokenType.ID, null}
				}
			}
		};

		public static bool IsLegitOperationForEvaluations (TokenType operation, TokenType firstOperand, TokenType secondOperand = TokenType.UNDEFINED)
		{
			if (unaryOps.ContainsKey (operation)) {
				return unaryOps [operation].ContainsKey (firstOperand);
			}

			if (binaryOps.ContainsKey (operation) && binaryOps [operation].ContainsKey (firstOperand)) {
				return binaryOps [operation] [operation].ContainsKey (secondOperand);
			}
			
			return false;
		}

		public static bool IsLegitTailCallForEvaluation (TokenType tailCall, TokenType evaluation)
		{
			if (tailCalls.ContainsKey (tailCall)) {
				return tailCalls [tailCall].ContainsKey (evaluation);
			}

			return false;
		}
	}
}

