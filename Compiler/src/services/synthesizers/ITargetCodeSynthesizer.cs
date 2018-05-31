using System;
using System.Collections.Generic;

namespace Compiler
{
	public interface ITargetCodeSynthesizer
	{
		List<string> Translate (SyntaxTree syntaxTree = null);
	}
}

