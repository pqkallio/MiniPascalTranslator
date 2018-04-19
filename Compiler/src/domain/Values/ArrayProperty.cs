using System;

namespace Compiler
{
	public class ArrayProperty : Property
	{
		private int size;
		private TokenType arrayElementType;

		public ArrayProperty (TokenType valueType, int size = -1)
			: base(true)
		{
			this.arrayElementType = valueType;
			this.size = size;
		}

		public int Size
		{
			get { return this.size; }
		}

		public override TokenType GetTokenType ()
		{
			return TokenType.TYPE_ARRAY;
		}

		public TokenType ArrayElementType
		{
			get { return arrayElementType; }
		}
	}
}

