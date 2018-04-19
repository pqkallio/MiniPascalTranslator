using System;

namespace Compiler
{
	/// <summary>
	/// A void property, has no meaningful return value.
	/// </summary>
	public class VoidProperty : Property
	{
		public override TokenType GetTokenType ()
		{
			return TokenType.VOID;
		}
	}
}

