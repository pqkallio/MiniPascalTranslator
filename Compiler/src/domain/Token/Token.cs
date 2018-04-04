using System;
using System.Text;

namespace Compiler
{
	/// <summary>
	/// Represents a single symbol in the source code.
	/// </summary>
	public class Token
	{
		private readonly int row;		// the row in the source code this token is in
		private readonly int column;	// the position of this token in the code line
		private string value;			// the value of the token, if meaningful
		private TokenType tokenType;	// an enumerated type

		/// <summary>
		/// Initializes a new instance of the <see cref="MiniPLInterpreter.Token"/> class.
		/// </summary>
		/// <param name="row">Row.</param>
		/// <param name="column">Column.</param>
		public Token (int row, int column)
			: this (row, column, null, TokenType.UNDEFINED)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="MiniPLInterpreter.Token"/> class.
		/// </summary>
		/// <param name="row">Row.</param>
		/// <param name="column">Column.</param>
		/// <param name="value">Value.</param>
		public Token (int row, int column, string value)
			: this (row, column, value, TokenType.UNDEFINED)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="MiniPLInterpreter.Token"/> class.
		/// </summary>
		/// <param name="row">Row.</param>
		/// <param name="column">Column.</param>
		/// <param name="value">Value.</param>
		/// <param name="tokenType">Token type.</param>
		public Token (int row, int column, string value, TokenType tokenType)
		{
			this.row = row;
			this.column = column;
			this.value = value;
			this.tokenType = tokenType;
		}

		public TokenType Type
		{
			get { return tokenType; }
			set { tokenType = value; }
		}

		public string Value
		{
			set { this.value = value; }
			get { return this.value; }
		}

		public int Row
		{
			get { return row; }
		}

		public int Column
		{
			get { return column; }
		}

		public override string ToString ()
		{
			return string.Format ("[ Token: Type = {0}, Value = {1}, Line = {2}, Column = {3} ]", 
				Type, Value, Row, Column);
		}
	}
}