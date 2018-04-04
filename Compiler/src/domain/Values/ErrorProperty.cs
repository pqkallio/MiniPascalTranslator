using System;

namespace Compiler
{
	/// <summary>
	/// Represents an unusable property.
	/// </summary>
	public class ErrorProperty : IProperty
	{
		public ErrorProperty ()
		{}

		public TokenType GetTokenType ()
		{
			return TokenType.ERROR;
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
			return -1;
		}

		public string asString ()
		{
			return "error";
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

