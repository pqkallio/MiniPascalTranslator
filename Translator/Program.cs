using System;
using System.Collections.Generic;
using Compiler;

namespace Translator
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length < 1) {
				Console.WriteLine ("nothing to compile");
				return;
			}

			SourceBuffer buf = new SourceBuffer(@args[0]);
			string[] sourceLines = buf.SourceLines;
			Scanner sc = new Scanner (sourceLines);
			Parser parser = new Parser (sc);
			parser.Parse ();
		}
	}
}
