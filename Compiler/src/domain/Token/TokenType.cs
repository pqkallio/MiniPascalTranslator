using System;

namespace Compiler
{
	/// <summary>
	/// Enumerates different token types.
	/// </summary>
	public enum TokenType
	{
		ID,					// an id variable
		SIGN_PLUS,			// a plus sign, "+"
		SIGN_MINUS,			// a minus sign, "-"
		UNARY_OP_LOG_NEG,	// unary operation, logical negation
		UNARY_OP_POSITIVE,	// unary operation, positive
		UNARY_OP_NEGATIVE,	// unary operation, negative
		BINARY_OP_ADD,		// binary operation, addition
		BINARY_OP_SUB,		// binary operation, subtraction
		BINARY_OP_MUL,		// binary operation, multiplication
		BINARY_OP_DIV,		// binary operation, division
		BINARY_OP_MOD,		// binary operation, modulo
		BINARY_OP_LOG_EQ,	// binary operation, logical "equals"
		BINARY_OP_LOG_NEQ,	// binary operation, logical "not equals"
		BINARY_OP_LOG_LT,	// binary operation, logical less than
		BINARY_OP_LOG_LTE,	// binary operation, logical less than or equal
		BINARY_OP_LOG_GTE,	// binary operation, logical greater than or equal
		BINARY_OP_LOG_GT,	// binary operation, logical greater than
		BINARY_OP_LOG_AND,	// binary operation, logical and
		BINARY_OP_LOG_OR,	// binary operation, logical OR
		PARENTHESIS_LEFT,	// left parenthesis, "(" terminal
		PARENTHESIS_RIGHT,	// right parenthesis ")" terminal
		BRACKET_LEFT,		// left bracket, "[" terminal
		BRACKET_RIGHT,		// right bracket "]" terminal
		STRING_VAL,			// a string value
		END_STATEMENT,		// end of statement, ";" terminal
		VAR,				// declaration or reference, "var" terminal
		WHILE_LOOP,			// while-loop, "while" terminal
		DO_WHILE,			// start of while-block, "do" terminal
		READ,				// "read" terminal
		WRITELN,			// "writeln" terminal
		BEGIN,				// the beginning of a block, "begin" terminal 
		END,				// the end of block, "end" terminal
		RETURN,				// return statement, "return" terminal
		IF,					// if keyword
		THEN,				// then keyword
		ELSE,				// else keyword
		ASSERT,				// "assert" terminal
		INTEGER_VAL,			// an integer value
		TYPE_INTEGER,		// integer type
		BOOLEAN_VAL,		// boolean value in general
		BOOLEAN_VAL_FALSE,	// a boolean value, false
		BOOLEAN_VAL_TRUE,	// a boolean value, true
		TYPE_BOOLEAN,		// boolean type
		TYPE_STRING,		// string type
		UNDEFINED,			// token's type is undefined
		END_OF_FILE,		// end of file, EOF, you know..
		SET_TYPE,			// set type, ":" terminal
		ASSIGN,				// assign, ":=" terminal
		PROGRAM,			// the start of a program
		PROCEDURE,			// the start of a procedure
		FUNCTION,			// the start of a function
		ERROR,				// error token, if something goes wrong
		BINARY_OP_NO_OP,	// binary expression, no operation (only lefthand side)
		VOID,				// a void property
		SIZE,				// an array's size accessor
		TYPE_ARRAY,				// an array
		TYPE_REAL,			// a real variable
		REAL_VAL,			// a real value
		COMMA,				// a comma terminal
		DOT,				// a dot terminal
		OF					// "of" keyword
	}
}

