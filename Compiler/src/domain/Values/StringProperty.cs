using System;

namespace Compiler
{
	/// <summary>
	/// Designed to save and access a string.
	/// </summary>
	public class StringProperty : IProperty
	{
		private string value;
		private bool declared;
		private bool constant;

		public StringProperty (string value)
		{
			this.value = value;
			this.declared = false;
			this.constant = false;
		}

		public TokenType GetTokenType ()
		{
			return TokenType.STRING_VAL;
		}

		public string Value {
			get { return this.value; }
			set { this.value = value; }
		}

		public bool Declared
		{
			get { return declared; }
			set { declared = value; }
		}

		public bool Constant
		{
			get { return this.constant; }
			set { this.constant = value; }
		}

		public IProperty asProperty ()
		{
			return this;
		}

		public int asInteger ()
		{
			return StringUtils.isNumeral(Value) ? StringUtils.parseToInt(Value) : 0;
		}

		public string asString ()
		{
			return Value;
		}

		public bool asBoolean ()
		{
			return Value != null && Value != "";
		}

		public void setInteger (int value) {}

		public void setString (string value)
		{
			Value = value;
		}

		public void setBoolean (bool value) {}
	}
}

