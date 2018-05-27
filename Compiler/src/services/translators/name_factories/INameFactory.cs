using System;

namespace Compiler
{
	public interface INameFactory
	{
		string GetLabel ();
		string GetTempVarId (Scope scope, TokenType type, SyntaxTreeNode node);
		string GetCName (Scope scope, string id);
	}
}

