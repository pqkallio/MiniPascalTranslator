using System;
using System.Collections.Generic;

namespace Compiler
{
	/// <summary>
	/// A recursive-descent parser for Mini-Pascal programming language
	/// Performs the syntactical analysis on a source code.
	/// Also, builds an AST and a global symbol table for the
	/// semantical analyzer.
	/// </summary>
	public class Parser : IErrorAggregator
	{
		private SyntaxTree syntaxTree;		// the AST to be built
		private List<Error> errors;			// a list of errors encountered while parsing
		private Scanner scanner;			// we ask tokens from the scanner
		private bool syntaxTreeBuilt;		// true if no errors were encountered, false otherwise
		private NodeBuilder nodeBuilder;	// we ask ST-nodes from the NodeBuilder
		private Scope programScope;			// the highest scope level
		private Token bufferedToken;

		/// <summary>
		/// Initializes a new instance of the <see cref="Compiler.Parser"/> class.
		/// </summary>
		/// <param name="symbolTable">The universal symbol table provided by the compiler frontend</param>
		/// <param name="scanner">The scanner that works for this parser</param>
		public Parser (Scanner scanner)
		{
			this.syntaxTree = new SyntaxTree ();
			this.errors = new List<Error> ();
			this.syntaxTreeBuilt = false;
			this.nodeBuilder = new NodeBuilder ();
			this.scanner = scanner;
			this.programScope = null;
			this.bufferedToken = null;
		}

		public Scanner Scanner {
			get { return scanner; }
			set { this.scanner = value; }
		}

		public SyntaxTree SyntaxTree {
			get { return syntaxTree; }
		}

		private Token getNextToken ()
		{
			Token nextToken;

			if (bufferedToken != null) {
				nextToken = bufferedToken;
				bufferedToken = null;
			} else {
				nextToken = getNextToken ();
			}

			return nextToken;
		}
			
		/// <summary>
		/// This method is called when we want the parsing to be done.
		/// The AST is returned when finished
		/// <returns>The AST as a SyntaxTree object</returns>
		/// </summary>
		public SyntaxTree Parse () {
			// make the preparations first
			syntaxTreeBuilt = true;
			/* Then start parsing by asking for the first token from the scanner.
			 * 
			 * So, the first pass over the source starts here, and it's made
			 * over the original source code. All the three phases are somewhat
			 * interleaved during parsing: the lexical and syntactical analyses
			 * completely and the building of the AST, which is actually a part
			 * of the third frontend phase, the semantical analysis. */
			syntaxTree.Root = ParseProgram ();

			return syntaxTree;
		}

		private ProgramNode ParseProgram ()
		{
			Token token = getNextToken ();

			switch (token.Type) {
				case TokenType.PROGRAM:
						/* The exception handling in the parser is handled using exceptions:
						
						   If a parsing method encounters a token it isn't expecting, it throws a
						   UnexpectedTokenException, which is then catched by some of the methods
						   lower in the call stack. This method then reports the error and finds
						   a safe spot where the parsing can continue, by asking new tokens
						   from the scanner and discarding them until it finds a token it can cope with.

						   This here being the very first parsing method, it means that if the error thrown
						   higher in the stack is not catched before this point, the next safe point
						   is the end of file. */
					try {
						this.programScope = new Scope();
						Token programIdToken = getNextToken ();
						match(programIdToken, TokenType.ID);
						syntaxTree.ProgramName = programIdToken.Value;
						match (getNextToken (), TokenType.END_STATEMENT);
							
						FunctionNode functionNode = ParseFunctionsAndProcedures ();
						BlockNode blockNode = ParseBlock (programScope);
						
						match (getNextToken (), TokenType.DOT);
						match(getNextToken(), TokenType.END_OF_FILE);
							
						if (SyntaxTreeBuilt) {
							return nodeBuilder.CreateProgramNode (token, functionNode, blockNode, this.programScope);
						}
					} catch (UnexpectedTokenException ex) {
						notifyError (new SyntaxError (ex.Token, ex.ExpectedType, ex.ExpectationSet));
						FastForwardTo (ParserConstants.PROGRAM_FASTFORWARD_TO, token);
					}

					break;
			}

			return null;
		}

		private FunctionNode ParseFunctionsAndProcedures ()
		{
			Token token = getNextToken ();
			switch (token.Type) {
				case TokenType.FUNCTION:
					return ParseFunction (token);
				case TokenType.PROCEDURE:
					return ParseFunction (token, procedure: true);
				case TokenType.BEGIN:
					break;
				default:
					throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_FUNCTIONS_AND_PROCEDURES);
			}
			return null;
		}

		private FunctionNode ParseFunction (Token token, bool procedure=false)
		{
			VariableIdNode idNode = null;
			ParametersNode parameters = null;
			Scope functionScope = new Scope (programScope);

			try {
				idNode = ParseVarId (programScope);

				match(getNextToken(), TokenType.PARENTHESIS_LEFT);
				parameters = ParseParameters(functionScope);

				if (procedure) {
					idNode.VariableType = TokenType.VOID;
				} else {
					match(getNextToken(), TokenType.SET_TYPE);
					ParseType (idNode);
				}

				match(getNextToken(), TokenType.END_STATEMENT);
			} catch (UnexpectedTokenException ex) {
				FastForwardToStatementEnd (ex);
			}

			try {
				BlockNode blockNode = ParseBlock(functionScope);
				match(getNextToken(), TokenType.END_STATEMENT);

				if (SyntaxTreeBuilt) {
					FunctionNode functionNode = nodeBuilder.CreateFunctionNode(token, idNode, parameters, blockNode, functionScope);
					functionNode.Sequitor = ParseFunctionsAndProcedures ();
					return functionNode;
				}
			} catch (UnexpectedTokenException ex) {
				notifyError (new SyntaxError (ex.Token, ex.ExpectedType, ex.ExpectationSet));
				FastForwardTo (ParserConstants.FUNCTION_FASTFORWARD_TO, token);
			}

			return null;
		}

		private ParametersNode ParseParameters (Scope scope)
		{
			List<Parameter> parameters = new List<Parameter> ();
			Token token = null;

			while (true) {
				token = getNextToken ();

				switch (token.Type) {
					case TokenType.VAR:
					case TokenType.ID:
						parameters.Add (ParseParameter(token, scope));
						break;
					case TokenType.PARENTHESIS_RIGHT:
						goto END_WHILE;
					default:
						throw new UnexpectedTokenException(token, ParserConstants.EXPECTATION_SET_PARAMETERS);
				}

				token = getNextToken();

				switch (token.Type) {
					case TokenType.PARENTHESIS_RIGHT:
						goto END_WHILE;
					case TokenType.COMMA:
						break;
					default:
						throw new UnexpectedTokenException(token, ParserConstants.EXPECTATION_SET_PARAMETER_TAIL);
				}
			}

			END_WHILE:
			if (SyntaxTreeBuilt) {
				return nodeBuilder.CreateParametersNode(token, parameters);
			}

			return null;
		}

		private Parameter ParseParameter (Token token, Scope scope)
		{
			switch (token.Type) {
				case TokenType.VAR:
					return Param (scope, reference: true);
				case TokenType.ID:
					return Param (scope, token);
				default:
					throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_PARAMETER);
			}
		}

		private Parameter Param(Scope scope, Token idToken=null, bool reference=false)
		{
			if (reference) {
				idToken = getNextToken ();
			}

			if (idToken.Type != TokenType.ID) {
				throw new UnexpectedTokenException (idToken, TokenType.ID);
			}

			VariableIdNode idNode = new VariableIdNode (idToken.Value, scope, idToken);

			match (getNextToken (), TokenType.SET_TYPE);
			TokenType type = ParseType ();
			scope.AddProperty (idNode.ID, null);

			return new Parameter (idNode, type, reference);
		}

		private BlockNode ParseBlock (Scope scope)
		{
			try {
				Scope newScope = scope.AddChildScope ();
				StatementsNode statements = ParseStatements (scope);
				match (getNextToken (), TokenType.END);
				if (SyntaxTreeBuilt) {
					return new BlockNode (newScope, statements);
				}
			} catch (UnexpectedTokenException ex) {
				FastForwardToEndOfBlock (ex);
			}

			return null;
		}

		private StatementsNode ParseStatements (Scope scope)
		{
			IExpressionContainer statement = null;
			Token token = getNextToken ();

			switch (token.Type) {
				case TokenType.ID:
				case TokenType.RETURN:
				case TokenType.READ:
				case TokenType.WRITELN:
				case TokenType.ASSERT:
					statement = ParseSimpleStatement (scope, token);
					break;
				case TokenType.BEGIN:
				case TokenType.IF:
				case TokenType.WHILE_LOOP:
					statement = ParseStructuredStatement (scope, token);
					break;
				case TokenType.VAR:
					statement = ParseVarDeclaration (scope);
					break;
				case TokenType.END:
					return null;
				default:
					throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_STATEMENTS);
			}

			StatementsNode statements = nodeBuilder.CreateStatementsNode (token);
			Token nextToken = getNextToken ();

			switch (nextToken.Type) {
				case TokenType.END:
					break;
				case TokenType.END_STATEMENT:
					Token nextNextToken = getNextToken ();

					if (nextNextToken.Type != TokenType.END) {
						bufferedToken = nextNextToken;
						statements.Sequitor = ParseStatements (scope);
					}

					break;
			}

			if (SyntaxTreeBuilt) {
				return statements;
			}

			return null;
		}

		private IExpressionContainer ParseSimpleStatement (Scope scope, Token token)
		{
			try {
				switch (token.Type) {
					case TokenType.ID:
						return ParseIdStatement (scope, token);
					case TokenType.RETURN:
						return ParseReturnStatement (scope, token);
					case TokenType.READ:
						return ParseReadStatement (scope, token);
					case TokenType.WRITELN:
						return ParseWriteStatement (scope, token);
					case TokenType.ASSERT:
						return ParseAssertStatement (scope, token);
				}
			} catch (UnexpectedTokenException ex) {
				FastForwardToStatementEnd (ex);
			}
		}

		private IExpressionContainer ParseIdStatement (Scope scope, Token token)
		{
			VariableIdNode idNode = ParseVarId (scope, token);
			Token next = getNextToken ();

			switch (next.Type) {
				case TokenType.BRACKET_LEFT:
					return ParseAssignToArray (idNode, scope);
				case TokenType.ASSIGN:
					return ParseAssign (next, idNode, scope);
				case TokenType.PARENTHESIS_LEFT:
					ParseFunctionCall (idNode);
				default:
					throw new UnexpectedTokenException (next, ParserConstants.EXPECTATION_SET_ID_STATEMENT);
			}
		}

		private IExpressionContainer ParseAssignToArray (VariableIdNode idNode, Scope scope)
		{
			IExpressionNode arrayIndexExpression = ParseExpression (scope);
			match (getNextToken (), TokenType.BRACKET_RIGHT);
			idNode.ArrayIndex = arrayIndexExpression;

			Token token = getNextToken ();

			switch (token.Type) {
				case TokenType.ASSIGN:
					return ParseAssignToArray (getNextToken, idNode);
				default:
					throw new UnexpectedTokenException (token, TokenType.ASSIGN);
			}
		}

		private IExpressionContainer ParseAssign (Token token, VariableIdNode idNode, Scope scope)
		{
			IExpressionNode expression = ParseExpression (scope);

			if (SyntaxTreeBuilt) {
				return nodeBuilder.CreateAssignNode (idNode, scope, token, expression);
			}

			return null;
		}

		private IExpressionNode ParseExpression (Scope scope)
		{
			Token token = getNextToken ();

			switch (token.Type) {
				case TokenType.SIGN_MINUS:
				case TokenType.SIGN_PLUS:
				case TokenType.ID:
				case TokenType.STRING_VAL:
				case TokenType.INTEGER_VAL:
				case TokenType.BOOLEAN_VAL_FALSE:
				case TokenType.BOOLEAN_VAL_TRUE:
				case TokenType.REAL_VAL:
				case TokenType.PARENTHESIS_LEFT:
				case TokenType.UNARY_OP_LOG_NEG:
					IExpressionNode expression = ParseSimpleExpression (scope, token);
					return ParseExpressionTail (expression, scope);
				default:
					throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_EXPRESSION);
			}
		}

		private IExpressionNode ParseSimpleExpression (Scope scope, Token token)
		{
			IExpressionNode lhs = null;
			switch (token.Type) {
				case TokenType.ID:
				case TokenType.STRING_VAL:
				case TokenType.INTEGER_VAL:
				case TokenType.BOOLEAN_VAL_FALSE:
				case TokenType.BOOLEAN_VAL_TRUE:
				case TokenType.REAL_VAL:
				case TokenType.PARENTHESIS_LEFT:
				case TokenType.UNARY_OP_LOG_NEG:
					lhs = ParseTerm (scope, token);
				case TokenType.SIGN_MINUS:
					lhs = ParseUnaryOp (TokenType.UNARY_OP_NEGATIVE, scope);
				case TokenType.SIGN_PLUS:
					lhs = ParseUnaryOp (TokenType.UNARY_OP_POSITIVE, scope);
				default:
					throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_EXPRESSION);
			}
		
			return ParseSimpleExpressionTail (lhs, scope);
		}

		private IExpressionNode ParseTerm (Scope scope, Token token)
		{
			switch (token.Type) {
			case TokenType.ID:
			case TokenType.STRING_VAL:
			case TokenType.INTEGER_VAL:
			case TokenType.BOOLEAN_VAL_FALSE:
			case TokenType.BOOLEAN_VAL_TRUE:
			case TokenType.REAL_VAL:
			case TokenType.PARENTHESIS_LEFT:
			case TokenType.UNARY_OP_LOG_NEG:
				IExpressionNode factor = ParseFactor (scope, token);
				return ParseTermTail (factor, scope);
			default:
				throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_EXPRESSION);
			}
		}

		private IExpressionNode ParseFactor (Scope scope, Token token)
		{
			switch (token.Type) {
			case TokenType.ID:
			case TokenType.STRING_VAL:
			case TokenType.INTEGER_VAL:
			case TokenType.BOOLEAN_VAL_FALSE:
			case TokenType.BOOLEAN_VAL_TRUE:
			case TokenType.REAL_VAL:
			case TokenType.PARENTHESIS_LEFT:
			case TokenType.UNARY_OP_LOG_NEG:
				IExpressionNode factor = ParseFactorMain (scope, token);
				return ParseFactorTail (factor, scope);
			default:
				throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_EXPRESSION);
			}
		}

		private IExpressionNode ParseFactorMain (Scope scope, Token token)
		{
			switch (token.Type) {
			case TokenType.ID:
				VariableIdNode idNode = ParseVarId (scope, token);
				return ParseFactorIdTail (idNode, scope);
			case TokenType.STRING_VAL:
			case TokenType.INTEGER_VAL:
			case TokenType.BOOLEAN_VAL_FALSE:
			case TokenType.BOOLEAN_VAL_TRUE:
			case TokenType.REAL_VAL:
				return ParseLiteral (token);
			case TokenType.PARENTHESIS_LEFT:
				return ParseExpression (scope);
			case TokenType.UNARY_OP_LOG_NEG:
				IExpressionNode factor = ParseFactor (scope, getNextToken ());
				return nodeBuilder.CreateUnOpNode(token, factor);
			default:
				throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_EXPRESSION);
			}
		}

		private IExpressionNode ParseFactorIdTail (VariableIdNode idNode, Scope scope)
		{
			Token token = getNextToken ();
			switch (token.Type) {
			case (TokenType.PARENTHESIS_LEFT):
				return ParseCallTail(idNode, scope, token);
			default:
				bufferedToken = token;
				return idNode;
			}
		}

		private IExpressionNode ParseCallTail(VariableIdNode idNode, Scope scope, Token token)
		{
			ArgumentsNode arguments = ParseArguments (scope);
		}

		/*
		/// <summary>
		/// Starting point for the recursive descent parsing.
		/// </summary>
		/// <param name="token">Token from the scanner</param>
		/// <param name="root">The root node of the AST</param>
		private void ParseProgram (Token token, IStatementsContainer root)
		{
			// All the parsing methods use switch statements to decide the actions
			// based on the current tokens type
			switch (token.Type) {
				case TokenType.VAR:					// if it's one of the different statement types
				case TokenType.WHILE_LOOP:
				case TokenType.READ:
				case TokenType.WRITELN:
				case TokenType.ASSERT:
				case TokenType.ID:
					Token next;
					
					try {	
						next = ParseStatements (token, root);	// try to parse statements
					} catch (UnexpectedTokenException ex) {
						
						next = FastForwardToSourceEnd (ex);
					}

					try {
						match (next, TokenType.END_OF_FILE);	// we are excpecting the token to be end of file
					} catch (UnexpectedTokenException ex) {
						notifyError (new SyntaxError (ex.Token, ex.ExpectedType, null));
					}
					
					break;
				case TokenType.END_OF_FILE:
					// the source file was empty, no shame in that
					break;
				default:
					notifyError (new SyntaxError (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_PROGRAM));
					break;
			}
		}

		/// <summary>
		/// Parses statements.
		/// In the AST a StatementsNode connects a single statement or a block to its successor. 
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="token">Token.</param>
		/// <param name="parent">IStatementContainer to affiliate the statements with.</param>
		private Token ParseStatements(Token token, IStatementsContainer parent)
		{
			switch (token.Type) {
				case TokenType.VAR:
				case TokenType.WHILE_LOOP:
				case TokenType.READ:
				case TokenType.WRITELN:
				case TokenType.ASSERT:
				case TokenType.ID:
					
					StatementsNode statements = nodeFactory.CreateStatementsNode (parent, token);
					Token next;
					
					try {
						// try to parse a statement and affiliate it with the statements node
						next = ParseStatement (token, statements);
					} catch (UnexpectedTokenException ex) {
						// fastforward to the end of statement if it's malformed
						next = FastForwardToStatementEnd (ex);
					}
					
					match (next, TokenType.END_STATEMENT);

					// connect another statements node to this one
					return ParseStatements (scanner.getNextToken(next), statements);
				case TokenType.END:
				case TokenType.END_OF_FILE:
					// if the end of file or block is found, create no more statements to the current block / AST
					return token;
				case TokenType.ERROR:
					// this statement cannot be parsed, notify error and fastforward to a safe spot
					notifyError (new SyntaxError (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_STATEMENTS));
					
					next = FastForwardToStatementEnd(token);
					// try to parse more statements
					return ParseStatements (scanner.getNextToken(next), parent);
				default:
					return token;
			}
		}

		/// <summary>
		/// Parses a single statement as a StatementsNode's executable statement.
		/// </summary>
		/// <returns>The next token</returns>
		/// <param name="token">Token.</param>
		/// <param name="statementsNode">A StatementsNode.</param>
		private Token ParseStatement(Token token, StatementsNode statementsNode)
		{
			switch (token.Type) {
				case TokenType.VAR:
					return ParseDeclaration (token, statementsNode);
				case TokenType.ID:
					return ParseVariableAssign (token, statementsNode);
				case TokenType.WHILE_LOOP:
					return null;
				case TokenType.READ:
					return ParseRead (token, statementsNode);
				case TokenType.WRITELN:
					return ParsePrint (token, statementsNode);
				case TokenType.ASSERT:
					return ParseAssert (token, statementsNode);
				case TokenType.ERROR:
					notifyError(new SyntaxError (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_STATEMENT));
					return FastForwardToStatementEnd(token);
				default:
					throw new UnexpectedTokenException (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_STATEMENT);
			}
		}

		/// <summary>
		/// Parses a declaration statement.
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="token">Token.</param>
		/// <param name="statementsNode">A StatementsNode.</param>
		private Token ParseDeclaration(Token token, StatementsNode statementsNode)
		{
			// Try to parse all the pieces that a DeclarationNode needs to be evaluated.
			try {
				VariableIdNode idNode = nodeFactory.CreateIdNode ();
				// parse the target id
				Token next = ParseVarId (scanner.getNextToken (token), idNode);

				match (next, TokenType.SET_TYPE);
				// parse the id's type
				next = ParseType (scanner.getNextToken (next), idNode);

				// create the actual DeclarationNode
				DeclarationNode declarationNode = nodeFactory.CreateDeclarationNode(idNode, statementsNode, token);
				// parse the assign for the DeclarationNode
				return ParseAssign (next, declarationNode.AssignNode);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		/// <summary>
		/// Parses an assign statement.
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="token">Token.</param>
		/// <param name="statementsNode">Statements node.</param>
		private Token ParseVariableAssign(Token token, StatementsNode statementsNode)
		{
			try {
				VariableIdNode idNode = nodeFactory.CreateIdNode ();
				// parse the target id
				Token next = ParseVarId (token, idNode);

				match (next, TokenType.ASSIGN);
				AssignNode assignNode = nodeFactory.CreateAssignNode(idNode, statementsNode, token);

				// parses the expression of the assignment
				return ParseExpression (scanner.getNextToken(next), assignNode);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		/// <summary>
		/// Parses a read statement into an IOReadNode.
		/// </summary>
		/// <returns>The next token</returns>
		/// <param name="token">Token.</param>
		/// <param name="statementsNode">A StatementsNode.</param>
		private Token ParseRead(Token token, StatementsNode statementsNode)
		{
			try {
				VariableIdNode varId = nodeFactory.CreateIdNode ();
				// create the IOReadNode and affiliate it with the statementsNode
				nodeFactory.CreateIOReadNode(varId, statementsNode, token);
				// parses the variable id that the read operation's value is saved to
				return ParseVarId (scanner.getNextToken(token), varId);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		/// <summary>
		/// Parses a print statement into an IOPrintNode
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="token">Token.</param>
		/// <param name="statementsNode">A StatementsNode.</param>
		private Token ParsePrint (Token token, StatementsNode statementsNode)
		{
			try {
				// build the print statement node
				IOPrintNode printNode = nodeFactory.CreateIOPrintNode(statementsNode, token);
				// parse the expression to print
				return ParseExpression (scanner.getNextToken(token), printNode);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		/// <summary>
		/// Parses an assert statement into an AssertNode
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="token">Token.</param>
		/// <param name="statementsNode">A StatementsNode.</param>
		private Token ParseAssert (Token token, StatementsNode statementsNode)
		{
			try {
				Token next = scanner.getNextToken (token);
				match (next, TokenType.PARENTHESIS_LEFT);

				// create the AssertNode
				AssertNode assertNode = nodeFactory.CreateAssertNode(statementsNode, token);

				// parse the expression to assert when executed
				next = ParseExpression (scanner.getNextToken (next), assertNode);
				match (next, TokenType.PARENTHESIS_RIGHT);

				// create an IOPrinterNode to print a message in case of a failed assertion
				nodeFactory.CreateIOPrintNodeForAssertNode(assertNode);

				return scanner.getNextToken (next);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}
		*/

		private VariableIdNode ParseVarId(Scope scope, Token idToken=null) {
			Token token = idToken == null ? getNextToken () : idToken;

			switch (token.Type) {
				case TokenType.ID:
					if (SyntaxTreeBuilt) {
						return nodeBuilder.CreateIdNode (token, scope);
					}
					break;
				default:
					throw new UnexpectedTokenException (token, TokenType.ID, null);
				}

			return null;
		}

		/*
		/// <summary>
		/// Parses a variable id into a VariableIdNode
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="token">Token.</param>
		/// <param name="idNode">An IdentifierNode.</param>
		private Token ParseVarId(Token token, VariableIdNode idNode)
		{
			switch (token.Type) {
				case TokenType.ID:
					idNode.ID = token.Value;
					idNode.Token = token;
					
					return scanner.getNextToken (token);
				default:
					throw new UnexpectedTokenException (token, TokenType.ID, null);
			}
		}

		/// <summary>
		/// Parses a variable's type into a VariableIdNode and adds the variable to the symbol table.
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="token">Token.</param>
		/// <param name="idNode">Identifier node.</param>
		private Token ParseType (Token token, VariableIdNode idNode)
		{
			// In case the symbol table already contains the variable, we don't try ro add it twice.
			// This would of course be an error since it means the declaration for this variable
			// has already been made earlier, but this will be handled during the semantic analysis.
			if (!symbolTable.ContainsKey (idNode.ID)) {
				switch (token.Type) {
					case TokenType.TYPE_INTEGER:
						// set the id node's type
						idNode.VariableType = TokenType.INTEGER_VAL;
						// add the id to the symbol table, with its value set to default value
						symbolTable.Add (idNode.ID, (new IntegerProperty (SemanticAnalysisConstants.DEFAULT_INTEGER_VALUE)));
						break;
					case TokenType.TYPE_STRING:
						idNode.VariableType = TokenType.STRING_VAL;
						symbolTable.Add (idNode.ID, new StringProperty (SemanticAnalysisConstants.DEFAULT_STRING_VALUE));
						break;
					case TokenType.TYPE_BOOLEAN:
						idNode.VariableType = TokenType.BOOLEAN_VAL_FALSE;
						symbolTable.Add (idNode.ID, new BooleanProperty (SemanticAnalysisConstants.DEFAULT_BOOL_VALUE));
						break;
					default:
						throw new UnexpectedTokenException (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_DECLARATION_TYPE);
				}
			}

			return scanner.getNextToken (token);
		}
		*/

		private void ParseType (VariableIdNode idNode)
		{
			Token token = getNextToken ();

			switch (token.Type) {
				case TokenType.TYPE_INTEGER:
					// set the id node's type
					idNode.VariableType = TokenType.INTEGER_VAL;
					// add the id to the symbol table, with its value set to default value
					// symbolTable.Add (idNode.ID, (new IntegerProperty (SemanticAnalysisConstants.DEFAULT_INTEGER_VALUE)));
					break;
				case TokenType.TYPE_STRING:
					idNode.VariableType = TokenType.STRING_VAL;
					// symbolTable.Add (idNode.ID, new StringProperty (SemanticAnalysisConstants.DEFAULT_STRING_VALUE));
					break;
				case TokenType.TYPE_BOOLEAN:
					idNode.VariableType = TokenType.BOOLEAN_VAL;
					// symbolTable.Add (idNode.ID, new BooleanProperty (SemanticAnalysisConstants.DEFAULT_BOOL_VALUE));
					break;
				case TokenType.TYPE_REAL:
					idNode.VariableType = TokenType.REAL_VAL;
					break;
				default:
					throw new UnexpectedTokenException (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_DECLARATION_TYPE);
			}
		}

		private TokenType ParseType ()
		{
			Token token = getNextToken ();
			switch (token.Type) {
				case TokenType.TYPE_INTEGER:
				case TokenType.TYPE_STRING:
				case TokenType.TYPE_BOOLEAN:
				case TokenType.TYPE_REAL:
					return token.Type;
				default:
					throw new UnexpectedTokenException (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_DECLARATION_TYPE);
			}
		}

		/*
		/// <summary>
		/// Parses an assignment during a declaration statement into an AssignNode. 
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="token">Token.</param>
		/// <param name="assignNode">An AssignNode.</param>
		private Token ParseAssign (Token token, AssignNode assignNode)
		{
			switch (token.Type) {
				case TokenType.ASSIGN:
					// if assignment during a declaration was made, parse the expression to the AssignNode
					assignNode.Token = token;
					Token next = scanner.getNextToken (token);
					return ParseExpression (next, assignNode);
				case TokenType.END_STATEMENT:
					// otherwise, set a default assignment
					setDefaultAssignment (assignNode);
					return token;
				default:
					throw new UnexpectedTokenException (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_ASSIGN);
			}
		}

		/// <summary>
		/// Parses the an expression into a node that implements IExpressionContainer interface.
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="token">Token.</param>
		/// <param name="node">An IExpressionContainer.</param>
		private Token ParseExpression (Token token, IExpressionContainer node)
		{
			Token next;

			switch (token.Type) {
				case TokenType.INTEGER_VAL:
				case TokenType.STRING_VAL:
				case TokenType.BOOLEAN_VAL_FALSE:
				case TokenType.PARENTHESIS_LEFT:
				case TokenType.ID:
					// parse a binary operation
					BinOpNode binOp = nodeFactory.CreateBinOpNode(node, token);
					// parse the first operand
					next = ParseOperand (token, binOp);
					// parse the rest of the operation
					return ParseBinaryOp (next, binOp);
				case TokenType.UNARY_OP_LOG_NEG:
					// parse a unary operation
					UnOpNode unOp = nodeFactory.CreateUnOpNode (node, token);
					// parse the operation, then the operand
					next = ParseUnaryOp (token, unOp);
					return ParseOperand (next, unOp);
				default:
					throw new UnexpectedTokenException (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_EXPRESSION);
			}
		}

		/// <summary>
		/// Parses an expression operand into an IExpressionContainer
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="token">Token.</param>
		/// <param name="parent">An IExpressionContainer.</param>
		private Token ParseOperand (Token token, IExpressionContainer parent)
		{
			// an operand can be an integer, string, boolean, variable or an expression
			// check the token's type to know what we're parsing
			switch (token.Type) {
				case TokenType.INTEGER_VAL:
					ParseIntegerOperand(token, parent);
					break;
				case TokenType.STRING_VAL:
					nodeFactory.CreateStringValueNode(token, parent);
					break;
				case TokenType.BOOLEAN_VAL_FALSE:
					nodeFactory.CreateBoolValueNode(token, parent);
					break;
				case TokenType.ID:
					nodeFactory.CreateIdNode(token, parent);
					break;
				case TokenType.PARENTHESIS_LEFT:
					Token next = ParseExpression (scanner.getNextToken (token), parent);
					match (next, TokenType.PARENTHESIS_RIGHT);
					break;
				default:
					throw new UnexpectedTokenException (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_EXPRESSION);
			}

			return scanner.getNextToken (token);
		}

		/// <summary>
		/// Parses an integer operand into an IExpressionContainer
		/// </summary>
		/// <param name="token">Token.</param>
		/// <param name="parent">An IExpressionContainer.</param>
		private void ParseIntegerOperand(Token token, IExpressionContainer parent) {
			try {
				// try to create an IntValueNode for the value
				nodeFactory.CreateIntValueNode (token, parent);
			} catch (OverflowException) {
				// In case the token's value is an integer that cannot be represented as a
				// signed 32-bit integer, an OverflowException is thrown.
				// In this case, the parses reports an IntegerOverflowError.
				notifyError (new IntegerOverflowError (token));
				nodeFactory.CreateDefaultIntValueNode (token, parent);
			}
		}

		/// <summary>
		/// Parses a binary operation's operation and righthand side into a BinOpNode
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="token">Token.</param>
		/// <param name="binOp">A BinOpNode.</param>
		private Token ParseBinaryOp (Token token, BinOpNode binOp)
		{
			switch (token.Type) {
				case TokenType.UNARY_OP_POSITIVE:
				case TokenType.UNARY_OP_NEGATIVE:
				case TokenType.BINARY_OP_MUL:
				case TokenType.BINARY_OP_DIV:
				case TokenType.BINARY_OP_LOG_LT:
				case TokenType.BINARY_OP_LOG_EQ:
				case TokenType.BINARY_OP_LOG_AND:
					// In case it is a binary operation, the operation and the
					// righthand side is parsed
					Token next = ParseOperation (token, binOp);
					return ParseOperand (next, binOp);
				default:
					// in case it wasn't a binary operation, the BinOpNode's
					// operation remains "no operation" and the righthand side
					// is not parsed
					return token;
			}
		}

		/// <summary>
		/// Parses a unary operation into a UnOpNode.
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="token">Token.</param>
		/// <param name="unOp">A UnOpNode.</param>
		private Token ParseUnaryOp (Token token, UnOpNode unOp)
		{
			switch (token.Type) {
				case TokenType.UNARY_OP_LOG_NEG:
					unOp.Operation = TokenType.UNARY_OP_LOG_NEG;
					unOp.Token = token;
					return scanner.getNextToken (token);
				default:
					throw new UnexpectedTokenException (token, TokenType.UNARY_OP_LOG_NEG, null);
			}
		}

		/// <summary>
		/// Parses a binary operation's operation into a BinOpNode,
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="token">Token.</param>
		/// <param name="binOp">A BinOpNode.</param>
		private Token ParseOperation (Token token, BinOpNode binOp)
		{
			switch (token.Type) {
				case TokenType.UNARY_OP_POSITIVE:
				case TokenType.BINARY_OP_DIV:
				case TokenType.BINARY_OP_LOG_AND:
				case TokenType.BINARY_OP_LOG_EQ:
				case TokenType.BINARY_OP_LOG_LT:
				case TokenType.BINARY_OP_MUL:
				case TokenType.UNARY_OP_NEGATIVE:
					binOp.Operation = token.Type;
					return scanner.getNextToken (token);
				default:
					throw new UnexpectedTokenException (token, TokenType.UNDEFINED, null);
			}
		}

		/// <summary>
		/// Sets a default assignment to an AssignNode.
		/// </summary>
		/// <param name="assignNode">An AssignNode.</param>
		private void setDefaultAssignment (AssignNode assignNode)
		{
			TokenType idType = assignNode.IDNode.EvaluationType;

			switch (idType) {
				case TokenType.STRING_VAL:
					nodeFactory.CreateDefaultStringValueNode(assignNode.Token, assignNode);
					break;
				case TokenType.INTEGER_VAL:
					nodeFactory.CreateDefaultIntValueNode(assignNode.Token, assignNode);
					break;
				case TokenType.BOOLEAN_VAL_FALSE:
					nodeFactory.CreateDefaultBoolValueNode (assignNode.Token, assignNode);
					break;
				default:
					throw new UnexpectedTokenException (assignNode.IDNode.Token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_ID_VAL);
			} 
		}
		*/

		/// <summary>
		/// Match the specified token and expectedType.
		/// </summary>
		/// <param name="token">Token.</param>
		/// <param name="expectedType">A TokenType.</param>
		private void match(Token token, TokenType expectedType)
		{
			// If the token's type doesn't match the expected type, an UnexpectedTokenException is thrown
			// and the AST's build status is set to false
			// The calling method must then handle the exception or throw it.
			if (token.Type != expectedType) {
				syntaxTreeBuilt = false;
				throw new UnexpectedTokenException (token, expectedType, null);
			}
		}

		/// <summary>
		/// Fastforwards to the source code's end.
		/// </summary>
		/// <returns>The last token, namely the EOF token.</returns>
		/// <param name="ex">An UnexpectedTokenException.</param>
		private void FastForwardToSourceEnd (UnexpectedTokenException ex)
		{
			// report the error
			notifyError(new SyntaxError(ex.Token, ex.ExpectedType, ex.ExpectationSet));

			syntaxTreeBuilt = false;
			Token token;

			// ask for new token's until the end of file has been reached
			do {
				token = getNextToken (null);
			} while (token.Type != TokenType.END_OF_FILE);
		}

		/// <summary>
		/// Fastforwards to the next token whose type is listed in the tokenTypes argument's
		/// keyset.
		/// </summary>
		/// <returns>The token fastforwarded to.</returns>
		/// <param name="tokenTypes">Token types.</param>
		/// <param name="errorToken">The token that caused the error.</param>
		private void FastForwardTo (Dictionary<TokenType, string> tokenTypes, Token errorToken)
		{
			syntaxTreeBuilt = false;

			Token token = errorToken;

			// ask for another token until the token's type matches one of
			// the types in the keyset
			while (!tokenTypes.ContainsKey(token.Type)) {
				token = getNextToken (token);
			}
		}

		/// <summary>
		/// Fastforwards to the statement's end.
		/// </summary>
		/// <returns>The token that ends the statement.</returns>
		/// <param name="ex">An UnexpectedTokenException.</param>
		private void FastForwardToStatementEnd (UnexpectedTokenException ex)
		{
			notifyError (new SyntaxError (ex.Token, ex.ExpectedType, ex.ExpectationSet));

			FastForwardTo (ParserConstants.STATEMENT_FASTFORWARD_TO, ex.Token);
		}

		/// <summary>
		/// Fastforwards to the statement's end.
		/// </summary>
		/// <returns>The token that ends the statement.</returns>
		/// <param name="token">A Token.</param>
		private void FastForwardToStatementEnd (Token token)
		{
			FastForwardTo (ParserConstants.STATEMENT_FASTFORWARD_TO, token);
		}

		/// <summary>
		/// Fastforwards to the end of a for-loop block.
		/// </summary>
		/// <returns>The token that ends the for-loop block</returns>
		/// <param name="ex">An UnexpectedTokenException.</param>
		private void FastForwardToEndOfBlock (UnexpectedTokenException ex)
		{
			notifyError (new SyntaxError (ex.Token, ex.ExpectedType, ex.ExpectationSet));
			Token token = ex.Token;
			// since blocks can be nested, we must track the depth we're in to make sure
			// we find the correct end statement
			int blockDepth = 0;

			while (token.Type != TokenType.END_OF_FILE) {
				if (token.Type == TokenType.END) {		// if an end of block is found
					if (blockDepth == 0) {						// and the depth matches
						break;		// return the next token
					} else {
						blockDepth--;
					}
				} else if (token.Type == TokenType.BEGIN) {
					blockDepth++;
				}

				token = getNextToken (null);
			}
		}

		public void notifyError (Error error)
		{
			this.syntaxTreeBuilt = false;
			this.errors.Add (error);
		}

		public List<Error> getErrors ()
		{
			return this.errors;
		}

		public bool SyntaxTreeBuilt
		{
			get { return syntaxTreeBuilt; }
		}
	}
}

