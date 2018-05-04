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
		private INameFactory nameFactory;

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
			this.nameFactory = new CNameFactory ();
		}

		public Scanner Scanner {
			get { return scanner; }
			set { this.scanner = value; }
		}

		public SyntaxTree SyntaxTree {
			get { return syntaxTree; }
		}

		private Token GetNextToken ()
		{
			Token nextToken;

			if (bufferedToken != null) {
				nextToken = bufferedToken;
				bufferedToken = null;
			} else {
				nextToken = scanner.getNextToken ();
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
			Token token = GetNextToken ();

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
						this.programScope = new Scope(); // first we create the program's scope
						
						Token programIdToken = GetNextToken ();
						// MUST CHECK FOR RESERVED KEYWORDS AS WELL!!!!!!!!!!!
						// CAN'T USE MATCH!!!!!!!!!!!
						match(programIdToken, TokenType.ID); // check the program has a valid id
						
						syntaxTree.ProgramName = programIdToken.Value; // save the program's id
						match (GetNextToken (), TokenType.END_STATEMENT); // the declaration ends here
						
						// now we start parsing the functions and procedures to a dictionary
						Dictionary<string, FunctionNode> functions = new Dictionary<string, FunctionNode>();
						ParseFunctionsAndProcedures (functions);
						
						BlockNode blockNode = ParseBlock (programScope); // then the main block
						
						match (GetNextToken (), TokenType.DOT); // the program should end here
						match(GetNextToken(), TokenType.END_OF_FILE); // and nothing more to come after it
							
						// if the parsing went well, return the program's root node 
						if (SyntaxTreeBuilt) {
						return nodeBuilder.CreateProgramNode (token, functions, blockNode, this.programScope);
						}
					} catch (UnexpectedTokenException ex) {
						notifyError (new SyntaxError (ex.Token, ex.ExpectedType, ex.ExpectationSet));
						FastForwardTo (ParserConstants.PROGRAM_FASTFORWARD_TO, token);
					}

					break;
				default:
					notifyError (new SyntaxError (token, TokenType.PROGRAM));
					break;
			}

			// if something went wrong, return null instead of a valid node
			return null;
		}

		/// <summary>
		/// Parses the functions and procedures into a predeclared dictionary of id - function node pairs.
		/// </summary>
		/// <param name="functions">Functions.</param>
		private void ParseFunctionsAndProcedures (Dictionary<string, FunctionNode> functions)
		{
			Token token = GetNextToken ();

			switch (token.Type) {
				case TokenType.FUNCTION:
					ParseFunction (token, functions);
					break;
				case TokenType.PROCEDURE:
					ParseFunction (token, functions, procedure: true);
					break;
				case TokenType.BEGIN:
					break;
				default:
					throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_FUNCTIONS_AND_PROCEDURES);
			}
		}

		/// <summary>
		/// Parses a function or a procedure into a predeclared dictionary.
		/// </summary>
		/// <param name="token">Token.</param>
		/// <param name="functions">Functions.</param>
		/// <param name="procedure">If set to <c>true</c>, parse a procedure, otherwise parse a function.</param>
		private void ParseFunction (Token token, Dictionary<string, FunctionNode> functions, bool procedure=false)
		{
			VariableIdNode idNode = null;
			ParametersNode parameters = null;
			Scope functionScope = programScope.AddChildScope ();

			try {
				idNode = ParseVarId (programScope);

				// first check that the program scope hasn't got a function or procedure with the same name
				// declared already
				if (expectDeclared (idNode, programScope, false)) {
					programScope.AddProperty (idNode.ID.ToLower(), new FunctionProperty (token.Row));
				}

				match(GetNextToken(), TokenType.PARENTHESIS_LEFT);
				parameters = ParseParameters(functionScope);

				// if it's a procedure, set the return type to void
				if (procedure) {
					idNode.VariableType = TokenType.VOID;
				} else {
					// otherwise, parse the type
					match (GetNextToken(), TokenType.SET_TYPE);
					ParseType (idNode, functionScope);
				}
			} catch (UnexpectedTokenException ex) {
				FastForwardToStatementEnd (ex);
			}
				
			try {
				match(GetNextToken(), TokenType.END_STATEMENT);
			} catch (UnexpectedTokenException ex) {
				FastForwardToStatementEnd (ex);
				notifyError (new SyntaxError (ex.Token, ex.ExpectedType, ex.ExpectationSet));
				match(GetNextToken(), TokenType.END_STATEMENT);
			}

			try {
				match(GetNextToken(), TokenType.BEGIN);
				// no we parse the function's main block 
				BlockNode blockNode = ParseBlock(functionScope);
				Token next = GetNextToken();
				match(next, TokenType.END_STATEMENT);

				// if all went well, add a new FunctionNode to the dictionary
				if (SyntaxTreeBuilt) {
					FunctionNode functionNode = null;

					if (procedure) {
						functionNode = new ProcedureNode(token, nameFactory, idNode, parameters, blockNode, functionScope);
					} else {
						functionNode = nodeBuilder.CreateFunctionNode(token, nameFactory, idNode, parameters, blockNode, functionScope);
					}

					functions[idNode.ID] = functionNode;
				}
			} catch (UnexpectedTokenException ex) {
				notifyError (new SyntaxError (ex.Token, ex.ExpectedType, ex.ExpectationSet));
				FastForwardTo (ParserConstants.FUNCTION_FASTFORWARD_TO, token);
			}

			// parse another function
			ParseFunctionsAndProcedures (functions);
		}

		/// <summary>
		/// Parses the parameters of a function or procedure.
		/// </summary>
		/// <returns>The parameters.</returns>
		/// <param name="scope">Scope.</param>
		private ParametersNode ParseParameters (Scope scope)
		{
			// the parameters are parsed in to this list,
			// so that the order remains the same as the one 
			// in the source code
			List<Parameter> parameters = new List<Parameter> ();
			Token token = null;

			while (true) {
				token = GetNextToken ();

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

				token = GetNextToken();

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
				idToken = GetNextToken ();
			}

			if (idToken.Type != TokenType.ID) {
				throw new UnexpectedTokenException (idToken, TokenType.ID);
			}

			VariableIdNode idNode = new VariableIdNode (idToken.Value, scope, idToken);

			match (GetNextToken (), TokenType.SET_TYPE);

			Property property = ParsePropertyForParam (scope);

			if (property.GetTokenType () == TokenType.TYPE_ARRAY) {
				ArrayProperty arrayProp = (ArrayProperty)property;
				idNode.ArrayElementType = arrayProp.ArrayElementType;
				reference = true;
			}

			if (property.GetTokenType () == TokenType.STRING_VAL) {
				reference = true;
			}

			if (expectDeclared (idNode, scope, false)) {
				scope.AddProperty (idNode.ID.ToLower(), property);
			}

			if (SyntaxTreeBuilt) {
				return new Parameter (idNode, property.GetTokenType (), reference);
			}

			return null;
		}

		private Property ParsePropertyForParam (Scope scope)
		{
			TokenType type = ParseType ();

			switch (type) {
				case TokenType.TYPE_INTEGER:
					return new IntegerProperty ();
				case TokenType.TYPE_BOOLEAN:
					return new BooleanProperty ();
				case TokenType.TYPE_REAL:
					return new RealProperty ();
				case TokenType.TYPE_STRING:
					return new StringProperty ();
				case TokenType.TYPE_ARRAY:
					return ParseArrayPropertyForParam (scope);
				default:
					return new ErrorProperty ();
			}
		}

		private Property ParseArrayPropertyForParam (Scope scope)
		{
			match (GetNextToken (), TokenType.BRACKET_LEFT);

			Token token = GetNextToken ();

			switch (token.Type) {
			case TokenType.BRACKET_RIGHT:
				bufferedToken = token;
				break;
			default:
				bufferedToken = token;
				ExpressionNode expression = ParseExpression (scope);
				break;
			}

			match (GetNextToken (), TokenType.BRACKET_RIGHT);
			match (GetNextToken (), TokenType.OF);

			TokenType type = ParseType ();

			if (ParserConstants.SIMPLE_TYPES.ContainsKey (type)) {
				return new ArrayProperty (type);
			}

			notifyError (new IllegalArrayElementTypeError ());
			return new ArrayProperty (TokenType.ERROR);
		}

		private BlockNode ParseBlock (Scope scope)
		{
			try {
				Scope newScope = scope.AddChildScope ();
				Token token = GetNextToken ();
				List<StatementNode> statements = new List<StatementNode> ();
				ParseStatements (token, newScope, statements);

				if (SyntaxTreeBuilt) {
					return new BlockNode (token, newScope, nameFactory, statements);
				}
			} catch (UnexpectedTokenException ex) {
				FastForwardToEndOfBlock (ex);
			}

			return null;
		}

		private void ParseStatements (Token token, Scope scope, List<StatementNode> statements)
		{
			StatementNode statement = null;

			switch (token.Type) {
				case TokenType.ID:
				case TokenType.RETURN:
				case TokenType.READ:
				case TokenType.WRITELN:
				case TokenType.ASSERT:
				case TokenType.BEGIN:
				case TokenType.IF:
				case TokenType.WHILE_LOOP:
				case TokenType.VAR:
				try {
					statement = ParseStatement (scope, token);
					statements.Add (statement);
				} catch (UnexpectedTokenException ex) {
					FastForwardToStatementEnd (ex);
				}
				break;
				case TokenType.END:
					return;
				default:
					throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_STATEMENTS);
			}

			Token nextToken = GetNextToken ();

			switch (nextToken.Type) {
				case TokenType.END:
					break;
				case TokenType.END_STATEMENT:
					Token nextNextToken = GetNextToken ();

					if (nextNextToken.Type != TokenType.END) {
						ParseStatements (nextNextToken, scope, statements);
					}

					break;
				default:
					ParseStatements (nextToken, scope, statements);
					break;
			}
		}

		private StatementNode ParseStatement (Scope scope, Token token)
		{
			StatementNode statement = null;

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
					statement = ParseVarDeclaration (token, scope);
					break;
				case TokenType.END:
					break;
				default:
					throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_STATEMENTS);
			}

			return statement;
		}

		private StatementNode ParseStructuredStatement (Scope scope, Token token)
		{
			switch (token.Type) {
				case TokenType.BEGIN:
					return ParseBlock (scope);
				case TokenType.IF:
					return ParseIfStatement (token, scope);
				case TokenType.WHILE_LOOP:
					return ParseWhileLoop (token, scope);
			}

			return null;
		}

		private StatementNode ParseIfStatement (Token token, Scope scope)
		{
			ExpressionNode ifCondition = ParseExpression (scope);
			match (GetNextToken (), TokenType.THEN);
			StatementNode ifBranch = ParseStatement (scope, GetNextToken ());
			StatementNode elseBranch = ParseElseStatement (scope);

			if (SyntaxTreeBuilt) {
				return new IfNode (token, nameFactory, ifCondition, ifBranch, elseBranch);
			}

			return null;
		}

		private StatementNode ParseElseStatement (Scope scope)
		{
			Token token = GetNextToken ();

			switch (token.Type) {
				case TokenType.ELSE:
					return ParseStatement (scope, GetNextToken ());
				default:
					bufferedToken = token;
					return null;
			}
		}

		private StatementNode ParseWhileLoop (Token token, Scope scope)
		{
			ExpressionNode condition = ParseExpression (scope);
			match (GetNextToken (), TokenType.DO_WHILE);
			StatementNode statement = ParseStatement(scope, GetNextToken ());

			if (SyntaxTreeBuilt) {
				return new WhileNode (token, nameFactory, scope, condition, statement);
			}

			return null;
		}

		private StatementNode ParseVarDeclaration (Token token, Scope scope)
		{
			List<VariableIdNode> ids = new List<VariableIdNode> ();
			ParseIdsToDeclare (ids, scope);
			TypeNode type = ParseTypeNode (scope);

			if (SyntaxTreeBuilt) {
				foreach (VariableIdNode idNode in ids) {
					string id = idNode.ID.ToLower ();

					if (expectDeclared (idNode, scope, false, false)) {
						switch (type.PropertyType) {
							case TokenType.TYPE_INTEGER:
								scope.AddProperty (id, new IntegerProperty (token.Row));
								break;
							case TokenType.TYPE_STRING:
								scope.AddProperty (id, new StringProperty (token.Row));
								break;
							case TokenType.TYPE_BOOLEAN:
								scope.AddProperty (id, new BooleanProperty (token.Row));
								break;
							case TokenType.TYPE_REAL:
								scope.AddProperty (id, new RealProperty (token.Row));
								break;
							case TokenType.TYPE_ARRAY:
								scope.AddProperty (id, new ArrayProperty(type.ArrayElementType, declarationRow: token.Row));
								break;
						}
					}
				}

				return new DeclarationNode (token, nameFactory, scope, type, ids);
			}

			return null;
		}

		private TypeNode ParseTypeNode (Scope scope)
		{
			Token token = GetNextToken ();
			switch (token.Type) {
			case TokenType.TYPE_INTEGER:
			case TokenType.TYPE_BOOLEAN:
			case TokenType.TYPE_REAL:
			case TokenType.TYPE_STRING:
				return new TypeNode (token, token.Type);
			case TokenType.TYPE_ARRAY:
				match (GetNextToken (), TokenType.BRACKET_LEFT);
				ExpressionNode arraySizeExpression = ParseExpression (scope);
				match (GetNextToken (), TokenType.BRACKET_RIGHT);
				match (GetNextToken (), TokenType.OF);
				TokenType elementType = ParseType ();
				return new ArrayTypeNode (token, token.Type, elementType, arraySizeExpression);
			default:
				throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_TYPE);
			}
		}

		private void ParseIdsToDeclare (List<VariableIdNode> ids, Scope scope)
		{
			Token token = GetNextToken ();
			TokenType type = idType (token.Type);

			switch (type) {
				case TokenType.ID:
					VariableIdNode idNode = ParseVarId (scope, token);
					ids.Add (idNode);
					ParseIdDelimiter (ids, scope);
					break;
				default:
					throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_DECLARATION);
			}
		}

		private void ParseIdDelimiter (List<VariableIdNode> ids, Scope scope)
		{
			Token token = GetNextToken ();

			switch (token.Type) {
				case TokenType.COMMA:
					Token next = GetNextToken ();
					if (idType (next.Type) != TokenType.ID) {
						notifyError (new SyntaxError (token, TokenType.ID, message: ErrorConstants.DANGLING_COMMA_ERROR_MSG));
					}
					bufferedToken = next;
					ParseIdsToDeclare (ids, scope);
					break;
				case TokenType.SET_TYPE:
					if (ids.Count == 0) {
						notifyError(new SyntaxError(token, TokenType.ID, message: ErrorConstants.NO_IDS_TO_DECLARE_ERROR_MSG));
					}
					break;
				default:
					throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_DECLARATION);
			}
		}

		private StatementNode ParseSimpleStatement (Scope scope, Token token)
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

			return null;
		}

		private StatementNode ParseReturnStatement (Scope scope, Token token)
		{
			Token next = GetNextToken ();
			ExpressionNode expression = null;

			switch (next.Type) {
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
					bufferedToken = next;
					expression = ParseExpression (scope);
					break;
				case TokenType.ELSE:
				case TokenType.END_STATEMENT:
				case TokenType.END:
					bufferedToken = next;
					break;
				default:
					throw new UnexpectedTokenException (next, ParserConstants.EXPECTATION_SET_RETURN_STATEMENT);
			}

			if (SyntaxTreeBuilt) {
				return nodeBuilder.CreateReturnStatement (token, expression);
			}

			return null;
		}

		private StatementNode ParseReadStatement (Scope scope, Token token)
		{
			match (GetNextToken (), TokenType.PARENTHESIS_LEFT);
			List<VariableIdNode> ids = new List<VariableIdNode> ();
			ParseIdsForReadStatement (scope, ids);
			match (GetNextToken (), TokenType.PARENTHESIS_RIGHT);

			if (SyntaxTreeBuilt) {
				return new IOReadNode (ids, scope, token, nameFactory);
			}

			return null;
		}

		private void ParseIdsForReadStatement (Scope scope, List<VariableIdNode> ids)
		{
			Token token = GetNextToken ();
			TokenType type = idType(token.Type);

			switch (type) {
			case TokenType.ID:
				VariableIdNode idNode = ParseVariable (scope, token);
				ids.Add (idNode);
				ParseIdsForReadStatement (scope, ids);
				break;
			case TokenType.COMMA:
				ParseIdsForReadStatement (scope, ids);
				break;
			case TokenType.PARENTHESIS_RIGHT:
				bufferedToken = token;
				break;
			default:
				throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_IDS_FOR_READ);
			}
		}

		private TokenType idType(TokenType type) {
			if (type == TokenType.ID) {
				return type;
			}
			if (ScannerConstants.RESERVED_SEQUENCES.ContainsValue (type)) {
				if (!ScannerConstants.KEYWORDS.ContainsValue (type)) {
					return TokenType.ID;
				}
			}

			return type;
		}

		private VariableIdNode ParseVariable (Scope scope, Token token)
		{
			Token next = GetNextToken ();

			if (next.Type != TokenType.BRACKET_LEFT) {
				bufferedToken = next;
				return new VariableIdNode (token.Value, scope, token);
			}

			ExpressionNode arrayIndexExpression = ParseExpression (scope);
			match (GetNextToken (), TokenType.BRACKET_RIGHT);

			return new VariableIdNode (token.Value, scope, token, arrayIndexExpression);
		}

		private StatementNode ParseWriteStatement (Scope scope, Token token)
		{
			match (GetNextToken (), TokenType.PARENTHESIS_LEFT);
			List<ExpressionNode> arguments = new List<ExpressionNode> ();
			ArgumentsNode argumentsNode = ParseArguments (scope, arguments);
			match (GetNextToken (), TokenType.PARENTHESIS_RIGHT);

			if (SyntaxTreeBuilt) {
				return new IOPrintNode (token, argumentsNode, nameFactory);
			}

			return null;
		}

		private StatementNode ParseAssertStatement (Scope scope, Token token)
		{
			match (GetNextToken (), TokenType.PARENTHESIS_LEFT);
			ExpressionNode expression = ParseExpression (scope);
			match (GetNextToken (), TokenType.PARENTHESIS_RIGHT);

			if (SyntaxTreeBuilt) {
				return new AssertNode (token, expression, nameFactory);
			}

			return null;
		}

		private StatementNode ParseIdStatement (Scope scope, Token token)
		{
			VariableIdNode idNode = ParseVarId (scope, token);
			Token next = GetNextToken ();

			switch (next.Type) {
				case TokenType.BRACKET_LEFT:
					return ParseAssignToArray (idNode, scope);
				case TokenType.ASSIGN:
					return ParseAssign (next, idNode, scope);
				case TokenType.PARENTHESIS_LEFT:
					return ParseCallTail(idNode, scope, token);
				default:
					throw new UnexpectedTokenException (next, ParserConstants.EXPECTATION_SET_ID_STATEMENT);
			}
		}

		private StatementNode ParseAssignToArray (VariableIdNode idNode, Scope scope)
		{
			ExpressionNode arrayIndexExpression = ParseExpression (scope);
			match (GetNextToken (), TokenType.BRACKET_RIGHT);

			Token token = GetNextToken ();

			switch (token.Type) {
				case TokenType.ASSIGN:
					ExpressionNode assignValueExpression = ParseExpression (scope);
					return nodeBuilder.CreateAssignToArrayNode (idNode, scope, token, nameFactory, arrayIndexExpression, assignValueExpression);
				default:
					throw new UnexpectedTokenException (token, TokenType.ASSIGN);
			}
		}

		private StatementNode ParseAssign (Token token, VariableIdNode idNode, Scope scope)
		{
			ExpressionNode expression = ParseExpression (scope);

			if (SyntaxTreeBuilt) {
				return nodeBuilder.CreateAssignNode (idNode, scope, token, nameFactory, expression);
			}

			return null;
		}

		private ExpressionNode ParseExpression (Scope scope)
		{
			Token token = GetNextToken ();

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
					SimpleExpression expression = ParseSimpleExpression (scope, token);
					ExpressionTail tail = ParseExpressionTail (scope);
					
					if (SyntaxTreeBuilt) {
						return new ExpressionNode (token, expression, tail);
					}

					return null;
				default:
					throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_EXPRESSION);
			}
		}

		private ExpressionTail ParseExpressionTail (Scope scope)
		{
			Token token = GetNextToken ();

			switch (token.Type) {
				case TokenType.BINARY_OP_LOG_EQ:
				case TokenType.BINARY_OP_LOG_NEQ:
				case TokenType.BINARY_OP_LOG_LT:
				case TokenType.BINARY_OP_LOG_LTE:
				case TokenType.BINARY_OP_LOG_GTE:
				case TokenType.BINARY_OP_LOG_GT:
					SimpleExpression rightHandSide = ParseSimpleExpression (scope, GetNextToken ());

					if (SyntaxTreeBuilt) {
						return new ExpressionTail (token, token.Type, rightHandSide);
					}

					break;
				default:
					bufferedToken = token;
					break;
			}

			return null;
		}

		private SimpleExpression ParseSimpleExpression (Scope scope, Token token)
		{
			bool additiveInverse = false;

			switch (token.Type) {
				case TokenType.SIGN_MINUS:
					additiveInverse = true;
					token = GetNextToken ();
					break;
				case TokenType.SIGN_PLUS:
					token = GetNextToken ();
					break;
				case TokenType.ID:
				case TokenType.STRING_VAL:
				case TokenType.INTEGER_VAL:
				case TokenType.BOOLEAN_VAL_FALSE:
				case TokenType.BOOLEAN_VAL_TRUE:
				case TokenType.REAL_VAL:
				case TokenType.PARENTHESIS_LEFT:
				case TokenType.UNARY_OP_LOG_NEG:
					break;
				default:
					throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_EXPRESSION);
			}

			switch (token.Type) {
			case TokenType.ID:
			case TokenType.STRING_VAL:
			case TokenType.INTEGER_VAL:
			case TokenType.BOOLEAN_VAL_FALSE:
			case TokenType.BOOLEAN_VAL_TRUE:
			case TokenType.REAL_VAL:
			case TokenType.PARENTHESIS_LEFT:
			case TokenType.UNARY_OP_LOG_NEG:
				TermNode term = ParseTerm (scope, token);
				SimpleExpressionTail tail = ParseSimpleExpressionTail (scope);

				if (SyntaxTreeBuilt) {
					return new SimpleExpression (token, term, tail, additiveInverse);
				}
				break;
				default:
					throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_EXPRESSION);
			}
		
			return null;
		}

		private SimpleExpressionTail ParseSimpleExpressionTail (Scope scope)
		{
			Token token = GetNextToken ();
			TokenType operation = TokenType.UNDEFINED;

			switch (token.Type) {
				case TokenType.SIGN_PLUS:
					operation = TokenType.BINARY_OP_ADD;
					break;
				case TokenType.SIGN_MINUS:
					operation = TokenType.BINARY_OP_SUB;
					break;
				case TokenType.BINARY_OP_LOG_OR:
					operation = TokenType.BINARY_OP_LOG_OR;
					break;
				default:
					bufferedToken = token;
					return null;
			}

			TermNode term = ParseTerm(scope, GetNextToken ());
			SimpleExpressionTail tail = ParseSimpleExpressionTail (scope);

			if (SyntaxTreeBuilt) {
				return new SimpleExpressionTail (token, operation, term, tail);
			}

			return null;
		}

		private TermNode ParseTerm (Scope scope, Token token)
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
				Factor factor = ParseFactor (scope, token);
				TermTail tail = ParseTermTail (scope);

				if (SyntaxTreeBuilt) {
					return new TermNode (token, factor, tail);
				}

				break;
			default:
				throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_EXPRESSION);
			}

			return null;
		}

		private TermTail ParseTermTail (Scope scope)
		{
			Token token = GetNextToken ();

			switch (token.Type) {
			case TokenType.BINARY_OP_MUL:
			case TokenType.BINARY_OP_DIV:
			case TokenType.BINARY_OP_MOD:
			case TokenType.BINARY_OP_LOG_AND:
				Factor factor = ParseFactor (scope, GetNextToken ());
				TermTail termTail = ParseTermTail (scope);

				if (SyntaxTreeBuilt) {
					return new TermTail (token, token.Type, factor, termTail);
				}

				break;
			}

			bufferedToken = token;
			return null;
		}

		private Factor ParseFactor (Scope scope, Token token)
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
				FactorMain main = ParseFactorMain (scope, token);
				FactorTail tail = ParseFactorTail (scope);

				if (SyntaxTreeBuilt) {
					return new Factor (token, main, tail);
				}

				break;
			default:
				throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_EXPRESSION);
			}

			return null;
		}

		private FactorTail ParseFactorTail (Scope scope)
		{
			Token token = GetNextToken ();

			if (token.Type == TokenType.SIZE) {
				return nodeBuilder.CreateArraySizeCheckNode (token, scope);
			} else {
				bufferedToken = token;
			}

			return null;
		}

		private FactorMain ParseFactorMain (Scope scope, Token token)
		{
			Evaluee evaluee = null;
			TokenType possibleId = idType (token.Type);

			if (possibleId == TokenType.ID && scope.ContainsKey (token.Value)) {
				token.Type = TokenType.ID;
			}

			switch (token.Type) {
			case TokenType.ID:
				VariableIdNode idNode = ParseVarId (scope, token);
				evaluee = ParseFactorIdTail (idNode, scope);
				break;
			case TokenType.STRING_VAL:
			case TokenType.INTEGER_VAL:
			case TokenType.BOOLEAN_VAL_FALSE:
			case TokenType.BOOLEAN_VAL_TRUE:
			case TokenType.REAL_VAL:
				evaluee = ParseLiteral (scope, token);
				break;
			case TokenType.PARENTHESIS_LEFT:
				evaluee = ParseExpression (scope);
				match (GetNextToken (), TokenType.PARENTHESIS_RIGHT);
				break;
			case TokenType.UNARY_OP_LOG_NEG:
				Factor factor = ParseFactor (scope, GetNextToken ());
				evaluee = new BooleanNegation (token, factor);
				break;
			default:
				throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_EXPRESSION);
			}

			if (SyntaxTreeBuilt) {
				return new FactorMain (token, evaluee);
			}

			return null;
		}

		private Evaluee ParseLiteral (Scope scope, Token token)
		{
			// etsi scopesta mahdolliset avainsaoja vastaavat muuttujat
			switch (token.Type) {
			case TokenType.STRING_VAL:
				return nodeBuilder.CreateStringValueNode (token);
			case TokenType.INTEGER_VAL:
				return ParseIntegerOperand(token);
			case TokenType.REAL_VAL:
				return nodeBuilder.CreateRealValueNode (token);
			case TokenType.BOOLEAN_VAL_FALSE:
			case TokenType.BOOLEAN_VAL_TRUE:
				return nodeBuilder.CreateBoolValueNode (token);
			default:
				throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_LITERAL);
			}
		}

		/// <summary>
		/// Parses an integer operand into an IExpressionContainer
		/// </summary>
		/// <param name="token">Token.</param>
		/// <param name="parent">An IExpressionContainer.</param>
		private IntValueNode ParseIntegerOperand(Token token) {
			try {
				// try to create an IntValueNode for the value
				return nodeBuilder.CreateIntValueNode (token);
			} catch (OverflowException) {
				// In case the token's value is an integer that cannot be represented as a
				// signed 32-bit integer, an OverflowException is thrown.
				// In this case, the parses reports an IntegerOverflowError.
				notifyError (new IntegerOverflowError (token));
				return null;
			}
		}


		private Evaluee ParseFactorIdTail (VariableIdNode idNode, Scope scope)
		{
			Token token = GetNextToken ();
			switch (token.Type) {
			case (TokenType.PARENTHESIS_LEFT):
				return ParseCallTail(idNode, scope, token);
			case (TokenType.BRACKET_LEFT):
				return ParseArrayAccess (idNode, scope, token);
			default:
				bufferedToken = token;
				return idNode;
			}
		}

		private Evaluee ParseArrayAccess (VariableIdNode idNode, Scope scope, Token token)
		{
			ExpressionNode arrayIndexExpression = ParseExpression (scope);
			match (GetNextToken (), TokenType.BRACKET_RIGHT);
			if (SyntaxTreeBuilt) {
				return new ArrayAccessNode (idNode, token, scope, arrayIndexExpression);
			}
			return null;
		}

		private Evaluee ParseCallTail(VariableIdNode idNode, Scope scope, Token token)
		{
			ArgumentsNode arguments = ParseFunctionArguments (scope);
			match (GetNextToken (), TokenType.PARENTHESIS_RIGHT);

			if (SyntaxTreeBuilt) {
				return nodeBuilder.CreateFunctionCallNode (idNode, arguments, token, scope, nameFactory);
			}

			return null;
		}

		private ArgumentsNode ParseFunctionArguments (Scope scope)
		{
			List<ExpressionNode> arguments = new List<ExpressionNode> ();
			return ParseArguments (scope, arguments);
		}

		private ArgumentsNode ParseArguments (Scope scope, List<ExpressionNode> arguments)
		{
			Token token = GetNextToken ();

			switch (token.Type) {
			case TokenType.SIGN_MINUS:
			case TokenType.SIGN_PLUS:
			case TokenType.ID:
			case TokenType.STRING_VAL:
			case TokenType.INTEGER_VAL:
			case TokenType.REAL_VAL:
			case TokenType.BOOLEAN_VAL_FALSE:
			case TokenType.BOOLEAN_VAL_TRUE:
			case TokenType.PARENTHESIS_LEFT:
			case TokenType.UNARY_OP_LOG_NEG:
				bufferedToken = token;
					ExpressionNode argument = ParseExpression (scope);
					arguments.Add (argument);
					return ParseArguments (scope, arguments);
				case TokenType.COMMA:
					return ParseArguments (scope, arguments);
			case TokenType.PARENTHESIS_RIGHT:
				bufferedToken = token;
					if (SyntaxTreeBuilt) {
						return nodeBuilder.CreateArgumentsNode (scope, arguments, token);
					}
					return null;
				default:
					throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_ARGUMENTS);
			}
		}

		private VariableIdNode ParseVarId(Scope scope, Token idToken=null) {
			Token token = idToken == null ? GetNextToken () : idToken;
			TokenType type = idType (token.Type);

			switch (type) {
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

		private void ParseType (VariableIdNode idNode, Scope scope)
		{
			Token token = GetNextToken ();

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
				case TokenType.TYPE_ARRAY:
					ParseArrayPropertyForId (scope, idNode);
					break;
				default:
					throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_DECLARATION_TYPE);
			}
		}

		private void ParseArrayPropertyForId (Scope scope, VariableIdNode idNode)
		{
			idNode.VariableType = TokenType.TYPE_ARRAY;
			match (GetNextToken (), TokenType.BRACKET_LEFT);

			Token token = GetNextToken ();

			switch (token.Type) {
			case TokenType.BRACKET_RIGHT:
				bufferedToken = token;
				break;
			default:
				bufferedToken = token;
				ParseExpression (scope);
				break;
			}

			match (GetNextToken (), TokenType.BRACKET_RIGHT);
			match (GetNextToken (), TokenType.OF);

			TokenType type = ParseType ();

			if (ParserConstants.SIMPLE_TYPES.ContainsKey (type)) {
				idNode.ArrayElementType = type;
			} else {
				idNode.ArrayElementType = TokenType.ERROR;
				notifyError (new IllegalArrayElementTypeError ());
			}
		}

		private TokenType ParseType ()
		{
			Token token = GetNextToken ();

			switch (token.Type) {
				case TokenType.TYPE_INTEGER:
				case TokenType.TYPE_STRING:
				case TokenType.TYPE_BOOLEAN:
				case TokenType.TYPE_REAL:
				case TokenType.TYPE_ARRAY:
					return token.Type;
				default:
					throw new UnexpectedTokenException (token, ParserConstants.EXPECTATION_SET_DECLARATION_TYPE);
			}
		}

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

		private bool expectDeclared (VariableIdNode idNode, Scope scope, bool expected = true, bool searchAncestors = true)
		{
			if (scope.ContainsKey (idNode.ID, searchAncestors) != expected) {
				syntaxTreeBuilt = false;

				if (expected) {
					notifyError (new UndeclaredVariableError (idNode));
				} else {
					notifyError (new DeclarationError (idNode));
				}

				return false;
			}

			return true;
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
				token = GetNextToken ();
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
				token = GetNextToken ();
			}

			bufferedToken = token;
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

				token = GetNextToken ();
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

