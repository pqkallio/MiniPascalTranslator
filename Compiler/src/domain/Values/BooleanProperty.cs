using System;

namespace Compiler
{
	/// <summary>
	/// Designed to save and access a boolean value.
	/// </summary>
	public class BooleanProperty : IProperty
	{
		private bool value;
		private bool declared;
		private bool constant;

		public BooleanProperty ()
			: this(false)
		{}

		public BooleanProperty (bool value)
		{
			this.value = value;
			this.declared = false;
			this.constant = false;
		}

		public TokenType GetTokenType ()
		{
			return TokenType.BOOLEAN_VAL_FALSE;
		}

		public bool Value {
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
			return Value ? 1 : 0;
		}

		public string asString ()
		{
			return Value ? "true" : "false";
		}

		public bool asBoolean ()
		{
			return Value;
		}

		public void setInteger (int value) {}

		public void setString (string value) {}

		public void setBoolean (bool value)
		{
			Value = value;
		}
	}
}

