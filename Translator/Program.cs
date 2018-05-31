using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Compiler;

namespace Translator
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			IPrinter printer = new ConsolePrinter ();

			if (args.Length < 1) {
				printer.printLine ("Nothing to compile.");
				printer.printLine ("Please supply the Mini-Pascal source code file as the first argument when running this program.");
				printer.printLine ("If you want to save the the translation to a file, please supply the target file name as the second argument.");
				return;
			}

			string sourceFilename = @args [0];

			if (!
				File.Exists(sourceFilename)) {
				printer.printLine (String.Format ("Unable to open the source file {0}.\n", sourceFilename));
				return;
			}

			CompilerFrontend cf = new CompilerFrontend ();
			SyntaxTree tree = cf.Compile (sourceFilename);

			if (cf.getErrors ().Count > 0) {
				printer.printLine (String.Format ("The following errors were encountered during compilation of file {0}:\n", sourceFilename));
				printer.SourceLines = cf.SourceLines;
				printer.printErrors (cf.getErrors ());
				return;
			}

			printer.printLine (String.Format ("Translation of the file {0} from Mini-Pascal to an AST finished succesfully.", sourceFilename));

			SimplifiedCSynthesizer synthesizer = new SimplifiedCSynthesizer (tree);
			List<string> translation = synthesizer.Translate ();

			printer = new ConsolePrinter (translation.ToArray ());

			if (args.Length > 1) {
				string targetFilename = @args [1];
				bool fileWritten = FileWriter.writeListToFile (translation, targetFilename);

				if (fileWritten) {
					printer.printLine (String.Format ("Synthesis of the AST to Simplified C was successfully written to {0}.\n", targetFilename));
					return;
				}

				printer.printLine (String.Format ("Unable to write to the target file {0}.\n", targetFilename));
			}

			printer.printLine ("Synthesis of the AST to Simplified C:\n");
			printer.printSourceLines ();
		}
	}
}
