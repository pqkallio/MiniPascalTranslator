using System;

namespace Compiler
{
	public interface INameFactory
	{
		string GetLabel ();
		string GetTempVarId (Scope scope, TokenType type, ref bool declared);
		string GetCName (Scope scope, string id);
	}
}

