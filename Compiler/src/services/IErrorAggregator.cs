using System;
using System.Collections.Generic;

namespace Compiler
{
	public interface IErrorAggregator
	{
		void notifyError (Error error);
		List<Error> getErrors ();
	}
}

