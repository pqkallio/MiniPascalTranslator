using System;
using System.Collections.Generic;

namespace Compiler
{
	public class SemanticAnalysisConstants
	{
		public static readonly Dictionary<TokenType, string> LOGICAL_OPERATIONS = new Dictionary<TokenType, string> ()
		{
			{TokenType.BINARY_OP_LOG_AND, null},
			{TokenType.BINARY_OP_LOG_EQ, null},
			{TokenType.BINARY_OP_LOG_LT, null},
			{TokenType.UNARY_OP_LOG_NEG, null}
		};
	}
}

