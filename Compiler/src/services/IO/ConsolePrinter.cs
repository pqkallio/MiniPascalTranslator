using System;
using System.Collections.Generic;

namespace Compiler
{
	public class ConsolePrinter : IPrinter
	{
		private string[] sourceLines;

		public ConsolePrinter()
		{}

		public ConsolePrinter (string[] sourceLines)
		{
			this.sourceLines = sourceLines;
		}

		public string[] SourceLines
		{
			get { return this.sourceLines; }
			set { this.sourceLines = value; }
		}

		public void printSourceLines ()
		{
			if (sourceLines == null) {
				return;
			}

			foreach (string line in sourceLines) {
				print (line);
			}
		}

		public void printErrors (List<Error> errors)
		{
			foreach (Error error in errors) {
				printError (error);
			}
		}

		public void printError (Error error)
		{
			printLine (StringFormatter.formatError (error, sourceLines));
		}

		public void printRuntimeException (RuntimeException exception)
		{
			printLine (StringFormatter.formatRuntimeException (exception, sourceLines));
		}

		public void print (string str)
		{
			Console.Write (str);
		}

		public void printLine (string str)
		{
			Console.WriteLine (str);
		}
	}
}

