using System;

namespace Compiler
{
	/// <summary>
	/// A void property, has no meaningful return value.
	/// </summary>
	public class VoidProperty : IProperty
	{
		public VoidProperty ()
		{}

		public TokenType GetTokenType ()
		{
			return TokenType.VOID;
		}

		public bool Declared {
			get { return false; }
			set { }
		}

		public bool Constant
		{
			get { return false; }
			set { }
		}

		public IProperty asProperty ()
		{
			return this;
		}

		public int asInteger ()
		{
			return 0;
		}

		public string asString ()
		{
			return "void";
		}

		public bool asBoolean ()
		{
			return false;
		}

		public void setInteger (int value) {}
		public void setString (string value) {}
		public void setBoolean (bool value) {}
	}
}

