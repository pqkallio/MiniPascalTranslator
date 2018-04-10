using System;

namespace Compiler
{
	public class ArrayProperty : IProperty
	{
		private int size;
		private bool declared;
		private bool constant;
		private TokenType valueType;

		public ArrayProperty (TokenType valueType, int size=0)
		{
			this.valueType = valueType;
			this.size = size;
			this.declared = false;
			this.constant = false;
		}

		public int Size
		{
			get { return this.size; }
		}

		public TokenType GetTokenType ()
		{
			return this.valueType;
		}

		bool Declared {				// an accessor for defining if the property is declared or not
			get { return this.declared; }
			set { this.declared = value; }
		}

		bool Constant {				// an accessor for defining if the property is constant or not
			get { return this.constant; }
			set { this.constant = value; }
		}

		public int asInteger ()			// returns the property as an integer
		{
			return -1;
		}

		public string asString ()
		{
			return null;
		}

		public bool asBoolean ()
		{
			return false;
		}

		public void setInteger (int value)
		{}

		public void setString (string value)
		{}

		public void setBoolean (bool value)
		{}
	}
}

