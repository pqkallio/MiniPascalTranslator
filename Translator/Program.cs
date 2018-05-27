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

			CompilerFrontend cf = new CompilerFrontend ();
			cf.Compile (@args [0]);

			ConsolePrinter printer = new ConsolePrinter (cf.SourceLines);

			printer.printErrors (cf.getErrors ());

			/*
			Scanner sc = new Scanner (sourceLines);
			Parser parser = new Parser (sc);
			parser.Parse ();

			if (sc.getErrors ().Count == 0 && parser.getErrors ().Count == 0) {
				SemanticAnalyzer analyzer = new SemanticAnalyzer (parser.SyntaxTree);
				analyzer.Analyze ();

				if (analyzer.getErrors ().Count == 0) {
					ITargetCodeTranslator translator = new SimplifiedCTranslator (analyzer.SyntaxTree);
					List<string> translation = translator.Translate ();
					foreach (string line in translation) {
						Console.Write (line);
					}
				} else {
					foreach (Error e in analyzer.getErrors()) {
						Console.WriteLine(StringFormatter.formatError(e, sourceLines));
					}
				}
			} else {
				foreach (Error e in sc.getErrors()) {
					Console.WriteLine(StringFormatter.formatError(e, sourceLines));
				}

				foreach (Error e in parser.getErrors()) {
					Console.WriteLine(StringFormatter.formatError(e, sourceLines));
				}
			}
			*/
		}
	}
}
