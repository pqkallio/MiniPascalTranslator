using System;

namespace Compiler
{
	/// <summary>
	/// Defines the methods for storing and retrieving data during 
	/// compilation and interpretation of a Mini-PL program
	/// 
	/// Implements the ISemanticCheckValue interface so anything
	/// implementing this interface can also be used as Visitors'
	/// methods' return values.
	/// </summary>
	public interface IProperty : ISemanticCheckValue
	{
		TokenType GetTokenType ();	// returns the token type of the property
		bool Declared {				// an accessor for defining if the property is declared or not
			get;
			set;
		}
		bool Constant {				// an accessor for defining if the property is constant or not
			get;
			set;
		}
		int asInteger ();			// returns the property as an integer
		string asString ();			// returns the property as a string
		bool asBoolean ();			// returns the property as a boolean
		void setInteger (int value);	// sets the property's integer value
		void setString (string value);	// sets the property's string value
		void setBoolean (bool value);	// sets the property's boolean value
	}
}

