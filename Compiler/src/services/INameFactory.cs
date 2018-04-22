using System;

namespace Compiler
{
	public interface INameFactory
	{
		string GetLabel ();
		string GetTempVarId (Object objet = null);
	}
}

