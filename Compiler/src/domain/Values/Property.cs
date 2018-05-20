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
	public abstract class Property : IComparable
	{
		private bool assigned;
		private int declarationRow;
		private bool reference;
		private bool declared;

		public Property (int declarationRow = int.MaxValue, bool assigned = false, bool reference = false, bool declared = false)
		{
			this.assigned = assigned;
			this.declarationRow = declarationRow;
			this.reference = reference;
			this.declared = declared;
		}

		public abstract TokenType GetTokenType ();	// returns the token type of the property

		public bool Assigned {
			get { return assigned; }
			set { this.assigned = value; }
		}

		public int DeclarationRow
		{
			get { return this.declarationRow; }
		}

		public bool Reference
		{
			get { return reference; }
		}

		public bool Declared
		{
			get { return declared; }
			set { this.declared = value; }
		}

		public int CompareTo (object objet)
		{
			if (this.GetTokenType () == TokenType.TYPE_ARRAY) {
				return 1;
			}

			if (objet.GetType () != this.GetType ()) {
				return 0;
			}

			Property prop = (Property)objet;

			if (prop.GetTokenType () == TokenType.TYPE_ARRAY) {
				return -1;
			}

			return 0;
		}
	}
}