using System;

namespace Compiler
{
	public class NumericUtils
	{
		public static bool IntBetween(int num, int min, int max)
		{
			return num >= min && num <= max;
		}
	}
}

