using System;

namespace Compiler
{
	public class ConsoleReader : IReader
	{
		public ConsoleReader ()
		{}

		public string readLine ()
		{
			return Console.ReadLine ();
		}
	}
}

