using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler
{
	public class StringUtils
	{
		public static bool isAlpha (char c)
		{
			return (c >= StringFormattingConstants.UTF8_CAPITAL_LETTERS_START && c <= StringFormattingConstants.UTF8_CAPITAL_LETTERS_END) ||
				(c >= StringFormattingConstants.UTF8_SMALL_LETTERS_START && c <= StringFormattingConstants.UTF8_SMALL_LETTERS_END);
		}

		public static bool isNumeral (char c)
		{
			return c >= 0x30 && c <= 0x39;
		}

		public static bool isNumeral (string str)
		{
			if (String.IsNullOrEmpty (str)) {
				return false;
			}

			if (str [0] != '-' && str [0] != '+' 
				&& !NumericUtils.IntBetween ((int) Char.GetNumericValue(str [0]), 0, 9)) {
				return false;
			}
			
			for (int i = 1; i < str.Length; i++) {
				int value = (int) Char.GetNumericValue (str[i]);

				if (!NumericUtils.IntBetween(value, 0, 9)) {
					return false;
				}
			}

			return true;
		}

		public static int parseToInt(string str)
		{
			if (!isNumeral (str)) {
				throw new ArgumentException ();
			}
	
			bool negative = str [0] == '-';
			bool signed = negative | str [0] == '+';
			int downTo = signed ? 0 : -1;
			int coefficient = 1;
			int value = 0;
			int[] barriers = { negative ? 8 : 7, 4, 6, 3, 8, 4, 7, 4, 1, 2 };

			if (barriers.Length < str.Length - (downTo + 1)) {
				throw new OverflowException();
			}

			bool overflow = false;
			int i, j, current;

			for (i = str.Length - 1, j = 0; i > downTo && j < barriers.Length; i--, j++) {
				current = (int)Char.GetNumericValue (str [i]);
				overflow = barriers [j] < current || (barriers [j] == current && overflow);
				value += current * coefficient;
				coefficient *= 10;
			}

			if (j > barriers.Length || (j == barriers.Length && overflow)) {
				throw new OverflowException();
			}

			return negative ? -1 * value : value;
		}

		public static bool parseToBoolean (string str)
		{
			if (String.IsNullOrEmpty (str)) {
				throw new ArgumentException ();
			}

			if (str.Equals ("true")) {
				return true;
			}

			if (str.Equals ("false")) {
				return false;
			}

			throw new ArgumentException ();
		}

		public static bool sequenceMatch (string input, int index, string sequence)
		{
			if (input == null || sequence == null) {
				throw new ArgumentNullException ();
			}

			if (input == "" && sequence == "") {
				return true;
			}

			if (index < 0 || index >= input.Length) {
				throw new ArgumentOutOfRangeException ();
			}

			int i, j;

			for (i = index, j = 0; j < sequence.Length && i < input.Length; i++, j++) {
				if (i >= input.Length || input [i] != sequence [j]) {
					return false;
				}
			}

			if (j != sequence.Length) {
				return false;
			}

			return true;
		}

		public static bool delimited (string input, char delimiter)
		{
			if (input == null) {
				throw new ArgumentNullException ();
			}

			if (input == "") {
				return false;
			}

			return input [0] == delimiter && input [input.Length - 1] == delimiter;
		}

		public static bool validId(string input)
		{
			if (input == null) {
				throw new ArgumentNullException ();
			}

			if (input == "") {
				return false;
			}

			char c = input [0];

			if (!(NumericUtils.IntBetween ((int)c, StringFormattingConstants.UTF8_SMALL_LETTERS_START, StringFormattingConstants.UTF8_SMALL_LETTERS_END) ||
				NumericUtils.IntBetween ((int)c, StringFormattingConstants.UTF8_CAPITAL_LETTERS_START, StringFormattingConstants.UTF8_CAPITAL_LETTERS_END))) {
				return false;
			}

			for (int i = 1; i < input.Length; i++) {
				c = input [i];
				bool valid = validIdChar (c);

				if (!valid) {
					return false;
				}
			}

			return true;
		}

		private static bool validIdChar(char c) {
			Dictionary<char, char> validChars = StringFormattingConstants.UTF8_VALID_ID_CHAR_RANGES;

			foreach (char key in validChars.Keys) {
				if (NumericUtils.IntBetween ((int)c, key, validChars [key])) {
					return true;
				}
			}

			return false;
		}

		public static string IntToString (int value)
		{
			if (value == 0) {
				return "0";
			}

			bool neg = value < 0;
			int remainder = value;

			StringBuilder sb = new StringBuilder ();

			while (remainder != 0) {
				sb.Insert (0, neg ? (remainder % 10) * -1 : remainder % 10);
				remainder /= 10;
			}

			if (neg) {
				sb.Insert (0, '-');
			}

			return sb.ToString ();
		}
	}
}

