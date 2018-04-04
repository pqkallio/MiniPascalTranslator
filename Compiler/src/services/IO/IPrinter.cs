using System;
using System.Collections.Generic;

namespace Compiler
{
	public interface IPrinter
	{
		void printErrors (List<Error> errors);
		void printError (Error error);
		void print (string str);
		void printLine (string str);
		void printRuntimeException (RuntimeException exception);
	}
}

