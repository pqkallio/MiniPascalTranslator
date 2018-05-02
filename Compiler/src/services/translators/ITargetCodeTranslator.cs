using System;
using System.Collections.Generic;

namespace Compiler
{
	public interface ITargetCodeTranslator
	{
		List<string> Translate (SyntaxTree syntaxTree = null);
	}
}

