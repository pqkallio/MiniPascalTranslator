using System;
using System.Collections.Generic;

namespace Compiler
{
	public class StringFormatter
	{
		public StringFormatter ()
		{}

		public static string formatError (Error error, string[] sourceLines)
		{
			int numRow = error.Token.Row < sourceLines.Length ? error.Token.Row : sourceLines.Length - 1;
			int numCol = error.Token.Row < sourceLines.Length ? error.Token.Column : sourceLines[sourceLines.Length - 1].Length;

			string errorMessage = line(error.ToString () + ' ' + formatRowAndColumn(numRow, numCol));

			string sourceLine = sourceLines[numRow];
			errorMessage += line (sourceLine);

			for (int i = 0; i < numCol; i++) {
				errorMessage += ' ';
			}

			errorMessage += '^';

			return errorMessage;
		}

		public static string formatRuntimeException (RuntimeException exception, string[] sourceLines)
		{
			int numRow = exception.Token.Row;
			int numCol = exception.Token.Column;

			string sourceLine = sourceLines [numRow];
			string errorMessage = line (exception.ToString () + ' ' + formatRowAndColumn (numRow, numCol)) + line (sourceLine);
			return errorMessage;
		}

		public static string formatFailedAssertion (AssertNode assertNode)
		{
			return line(StringFormattingConstants.ASSERTION_FAILED + assertNode.Expression.ToString());
		}

		public static string formatRowAndColumn (int numRow, int numCol)
		{
			Tuple<char, char> parentheses = StringFormattingConstants.PRINT_ROW_AND_COLUMN_PARENTHESES;

			string rowAndCol = parentheses.Item1 + StringFormattingConstants.PRINT_ROW + formatSourcePosition(numRow);
			rowAndCol += StringFormattingConstants.PRINT_ROW_COL_DELIMITER + StringFormattingConstants.PRINT_COL + formatSourcePosition(numCol);
			rowAndCol += parentheses.Item2;

			return rowAndCol;
		}

		public static string formatSourcePosition (int pos)
		{
			return StringUtils.IntToString (pos + 1);
		}

		public static string line (string str)
		{
			return str + StringFormattingConstants.LINEBREAK;
		}
	}
}