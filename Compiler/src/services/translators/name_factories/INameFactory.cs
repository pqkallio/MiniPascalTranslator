using System;

namespace Compiler
{
	public interface INameFactory
	{
		string GetLabel ();
		string GetTempVarId (Scope scope, TokenType type);
		string GetCName (Scope scope, string id);
	}
}

