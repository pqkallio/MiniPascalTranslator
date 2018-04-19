using System;

namespace Compiler
{
	/// <summary>
	/// Defines the methods for storing and retrieving data during 
	/// compilation and interpretation of a Mini-Pascal program
	/// 
	/// Implements the ISemanticCheckValue interface so anything
	/// implementing this interface can also be used as Visitors'
	/// methods' return values.
	/// </summary>
	public abstract class Property : ISemanticCheckValue
	{
		private bool assigned;

		public Property (bool assigned = false)
		{
			this.assigned = assigned;
		}

		public abstract TokenType GetTokenType ();	// returns the token type of the property

		bool Assigned {
			get { return assigned; }
			set { this.assigned = value; }
		}

		public Property asProperty ()
		{
			return this;
		}
	}
}

