using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Compiler
{
	public class CTranslationVisitor : INodeVisitor
	{
		// the code lines of a global scale translation
		private List<string> globalTranslation;

		// the translated code lines of the current scope
		private List<string> scopeTranslation;

		// the program's name
		private string programName;

		/* 
		 * the name factory is responsible for the handling of
		 * all the code labels, identifier names and temporary
		 * variables' names
		 */
		private CNameFactory nameFactory;

		// used to determine the amount of tabs to place before a translated code line
		private int blockDepth;

		private static Dictionary<TokenType, string> typeNames = CTranslatorConstants.SIMPLE_TYPE_NAMES;
		private static Dictionary<TokenType, string> paramTypeNames = CTranslatorConstants.PARAM_SIMPLE_TYPE_NAMES;
		private static Dictionary<TokenType, string> opStrings = CTranslatorConstants.OPERATION_STRINGS;

		/*
		 * a dictionary to hold all the malloc'd identifiers so that they can be
		 * freed after use
		 */
		private Dictionary<Scope, List<string>> allocations;

		/* 
		 * to keep track of all the variables, both temporary and those declared in the source AST
		 * so that they can be declared in the beginning of the scope's translation
		 */
		private Dictionary<Scope, Dictionary<string, Property>> declaredVariables;

		// the program's root node
		private ProgramNode programNode;

		/*
		 * the current scope under to elaborate
		 * 
		 * note! 	this does not refer to the original scope in the AST, since many of the
		 * 			nested scopes will be "flattened" into more generalized scopes 
		 * 			during the translation
		 */
		private Scope currentScope;

		// the code to perform when an error occurs or an assertion fails
		private string[] errorHandlingCode;

		/// <summary>
		/// This visitor class transforms Mini-Pascal AST-nodes into a simplified, "assembly-like" C
		/// </summary>
		/// <param name="programName">The name of the program</param>
		/// <param name="nameFactory">Name factory.</param>
		public CTranslationVisitor (string programName, CNameFactory nameFactory)
		{
			this.globalTranslation = new List<string> ();
			this.scopeTranslation = null;
			this.programName = programName;
			this.nameFactory = nameFactory;
			this.blockDepth = 0;
			this.allocations = new Dictionary<Scope, List<string>> ();
			this.declaredVariables = new Dictionary<Scope, Dictionary<string, Property>> ();
			this.programNode = null;
			this.currentScope = null;
			this.errorHandlingCode = new [] { "" };
		}

		/// <summary>
		/// Get the translation as a list of strings.
		/// </summary>
		/// <value>The translation.</value>
		public List<string> Translation
		{
			get { return this.globalTranslation; }
		}

		/// <summary>
		/// A private helper method to turn a string into a parenthesised version of the same string
		/// </summary>
		/// <param name="str">the string to parenthesize.</param>
		private string parenthesized (string str)
		{
			return "(" + str + ")";
		}

		/// <summary>
		/// A private helper method to turn a label into a label pointer
		/// </summary>
		/// <param name="str">the label.</param>
		private string label(string str) {
			return str + ":";
		}

		public void VisitAssertNode(AssertNode node)
		{
			node.AssertExpression.Accept (this);	// elaborate the expression to assert

			// create a label for a successful assertion
			string successLabel = nameFactory.GetLabel ();
			string loc = node.AssertExpression.Location;

			addEmptyLineToScopeTranslation ();
			// add a conditional jump to code in case of success
			addToScopeTranslation (statement (CTranslatorConstants.IF, parenthesized (loc), CTranslatorConstants.GOTO, successLabel));

			addEmptyLineToScopeTranslation ();
			// create code for the case of failure
			addToScopeTranslation (CreateFailedAssertionPrintCall ());
			addErrorHandlingCodeToScopeTranslation ();

			addEmptyLineToScopeTranslation ();
			// add label for successful assertion
			addToScopeTranslation (statement (label (successLabel)));

			nameFactory.ReturnTempVarId (currentScope, loc, node.AssertExpression.EvaluationType, node.AssertExpression);
		}

		private void addErrorLabelAndCodeToScopeTranslation ()
		{
			foreach (string str in errorHandlingCode) {
				addToScopeTranslation (str);
			}
		}

		private void addErrorHandlingCodeToScopeTranslation ()
		{
			addToScopeTranslation (statement (CTranslatorConstants.GOTO, CTranslatorConstants.ERROR_LABEL));
		}

		private string CreateFailedAssertionPrintCall()
		{
			return statement (CreateFunctionCall (CTranslatorConstants.PRINTING_FUNCTION_CALLS [TokenType.STRING_VAL], CTranslatorConstants.ASSERTION_FAILED_MESSAGE));
		}

		public void VisitArraySizeCheckNode(ArraySizeCheckNode node)
		{
			// get a temp location to save the size into
			setNodeLocation (node, TokenType.INTEGER_VAL, new IntegerProperty());
			string arrayId = nameFactory.GetCName (node.Scope, node.VariableID);

			/*
			 * The size of an array is always saved into an index in the array.
			 * The index that points to the cell that holds the array's size is
			 * defined in the CTranslatorConstants.cs file.
			 */
			SyntaxTreeNode tempVarNode = new TranslatorTempNode ();
			string tempVar = GetTempVarId (TokenType.INTEGER_VAL, new IntegerProperty (), tempVarNode);
			addToScopeTranslation (simpleAssignment (tempVar, CTranslatorConstants.ARRAY_SIZE_INDEX));

			string sizeCast = castToInt () + arrayAccess (arrayId, tempVar);

			// add an assignment to the target code which places the array's size into the temp location
			addToScopeTranslation (simpleAssignment (node.Location, sizeCast));

			nameFactory.ReturnTempVarId (currentScope, tempVar, TokenType.INTEGER_VAL, tempVarNode);
		}

		private string simpleAssignment (string target, string assignee)
		{
			return statement (spaced (target, CTranslatorConstants.ASSIGNMENT, assignee));
		}

		private string arrayAccess (string arrayId, string index)
		{
			// for accessing an array's indexed cell
			return arrayId + indexDelimited (index);
		}

		private string indexDelimited(string index)
		{
			string indexDelimiterLeft = CTranslatorConstants.ARRAY_INDEX_DELIMITERS.Item1;
			string indexDelimiterRight = CTranslatorConstants.ARRAY_INDEX_DELIMITERS.Item2;

			return indexDelimiterLeft + index + indexDelimiterRight;
		}

		private string castTo(TokenType type)
		{
			return parenthesized (typeNames [type]);
		}

		private string castToInt() {
			// get the C symbol of casting a value into an integer
			return castTo(TokenType.INTEGER_VAL);
		}

		public void VisitAssignNode(AssignNode node)
		{
			string idName = nameFactory.GetCName (node.Scope, node.IDNode.IDNode.ID);

			Property prop = declaredVariables [currentScope] [idName];
			// evaluate the value to assign
			node.AssignValueExpression.Accept (this);

			if (prop.Reference) {
				addToScopeTranslation (simpleAssignment (GetPointerToVariable (idName, node.IDNode.EvaluationType), node.AssignValueExpression.Location));
			} else {
				addToScopeTranslation (statement (spaced (idName, "=", node.AssignValueExpression.Location)));
			}

			nameFactory.ReturnTempVarId (currentScope, node.AssignValueExpression.Location, node.AssignValueExpression.EvaluationType, node.AssignValueExpression);
		}

		public string GetPointerToVariable (string id, TokenType type)
		{
			return CTranslatorConstants.POINTER_PREFIXES[type] + id;
		}

		public void VisitArrayAssignNode(ArrayAssignStatement node)
		{
			ExpressionNode index = node.IndexExpression;
			ExpressionNode value = node.AssignValueExpression;

			index.Accept (this);
			value.Accept (this);

			string arrayId = nameFactory.GetCName (node.Scope, node.IDNode.VariableID);
			TokenType type = value.EvaluationType;

			addToScopeTranslation (
				statement (
					CreateFunctionCall (CTranslatorConstants.ARRAY_INSERTION_FUNCTION_CALLS[type], 
						arrayId,
						index.Location, 
						value.Location)));

			addErrorCheckingToScopeTranslation ();

			nameFactory.ReturnTempVarId (currentScope, index.Location, index.EvaluationType, index);
			nameFactory.ReturnTempVarId (currentScope, value.Location, type, value);
		}

		private void addErrorCheckingToScopeTranslation ()
		{
			string lbl = CTranslatorConstants.ERROR_LABEL;
			string errorcodeVar = CTranslatorConstants.ERROR_CODE_VAR;
			string noError = CTranslatorConstants.DEFAULT_ERROR_CODE;

			addConditionalJumpsToScopeTranslation (lbl, neqOperationStrings (errorcodeVar, noError));
		}

		public void VisitArrayAccessNode(ArrayAccessNode node)
		{
			setNodeLocation (node, node.EvaluationType, new ArrayProperty(node.EvaluationType));
			ExpressionNode index = node.ArrayIndexExpression;
			index.Accept (this);
			string arrayId = nameFactory.GetCName (node.Scope, node.VariableID);
			switch (node.EvaluationType) {
				case (TokenType.INTEGER_VAL):
				case (TokenType.BOOLEAN_VAL):
					addToScopeTranslation (statement (CreateFunctionCall ("load_from_int_array", arrayId, index.Location, CTranslatorConstants.MEM_ADDRESS + node.Location)));
					break;
				case (TokenType.REAL_VAL):
					addToScopeTranslation (statement (CreateFunctionCall ("load_from_float_array", arrayId, index.Location, CTranslatorConstants.MEM_ADDRESS + node.Location)));
					break;
				case (TokenType.STRING_VAL):
					addToScopeTranslation (statement (CreateFunctionCall ("load_from_string_array", arrayId, index.Location, node.Location)));
					break;
			}

			addErrorCheckingToScopeTranslation ();

			nameFactory.ReturnTempVarId (currentScope, index.Location, index.EvaluationType, index);
		}

		private string[] neqOperationStrings (string lhs, string rhs)
		{
			return new [] { createBinaryOperationString(TokenType.BINARY_OP_LOG_LT, lhs, rhs), createBinaryOperationString(TokenType.BINARY_OP_LOG_GT, lhs, rhs) };
		}

		private string CreateFunctionCall (string functionId, params string[] arguments)
		{
			StringBuilder sb = new StringBuilder (functionId + "(");

			if (arguments.Length > 0) {
				int i;
				for (i = 0; i < arguments.Length - 1; i++) {
					sb.Append (arguments [i] + ", ");
				}

				sb.Append (arguments [i]);
			}

			sb.Append (")");

			return sb.ToString();
		}

		public void VisitDeclarationNode(DeclarationNode node)
		{
			Scope scope = node.Scope;

			if (node.DeclarationType.PropertyType == TokenType.TYPE_ARRAY) {
				foreach (VariableIdNode idNode in node.IDsToDeclare) {
					string id = nameFactory.GetCName(scope, idNode.ID);
					ExpressionNode sizeExpr = node.DeclarationType.ArraySizeExpression;
					TokenType elemType = node.DeclarationType.ArrayElementType;
					sizeExpr.Accept (this);
					SyntaxTreeNode realSizeTempNode = new TranslatorTempNode ();
					SyntaxTreeNode indexPointerTempNode = new TranslatorTempNode ();
					string realSizeTemp = GetTempVarId (TokenType.INTEGER_VAL, new IntegerProperty (), realSizeTempNode);
					string indexPointer = GetTempVarId (TokenType.INTEGER_VAL, new IntegerProperty (), indexPointerTempNode);
					addToScopeTranslation (simpleAssignment (realSizeTemp, createBinaryOperationString (TokenType.BINARY_OP_ADD, sizeExpr.Location, "1")));
					addToScopeTranslation (simpleAssignment (indexPointer, "0"));
					addAllocation (elemType, sizeExpr.Location, id);
					addToScopeTranslation (simpleAssignment (arrayAccess (id, indexPointer), unspaced (castTo (elemType), sizeExpr.Location)));
					nameFactory.ReturnTempVarId (currentScope, sizeExpr.Location, sizeExpr.EvaluationType, sizeExpr);
					nameFactory.ReturnTempVarId (currentScope, realSizeTemp, TokenType.INTEGER_VAL, realSizeTempNode);
					nameFactory.ReturnTempVarId (currentScope, indexPointer, TokenType.INTEGER_VAL, indexPointerTempNode);
				}
			} else {
				foreach (VariableIdNode idNode in node.IDsToDeclare) {
					string id = nameFactory.GetCName(scope, idNode.ID);
					declaredVariables [currentScope] [id] = getProperty(idNode.EvaluationType);
				}
			}
		}

		public void VisitIntValueNode(IntValueNode node)
		{	
			bool declared = false;
			setNodeLocation (node, node.EvaluationType, new IntegerProperty());
			
			addAssignment (typeNames[node.EvaluationType], node.Location, node.Value, node.Scope, declared: declared);
		}

		public void VisitRealValueNode(RealValueNode node)
		{
			bool declared = false;
			setNodeLocation (node, node.EvaluationType, new RealProperty());

			addAssignment (typeNames[node.EvaluationType], node.Location, node.Value, node.Scope, declared: declared);
		}

		public void VisitBoolValueNode(BoolValueNode node)
		{
			bool declared = false;
			setNodeLocation (node, node.EvaluationType, new IntegerProperty());

			string strVal = node.Value ? "1" : "0";

			addAssignment (typeNames[node.EvaluationType], node.Location, strVal, node.Scope, declared: declared);
		}

		public void VisitStringValueNode(StringValueNode node)
		{
			bool declared = false;
			setNodeLocation (node, node.EvaluationType, new StringProperty());

			string strVal = unspaced (CTranslatorConstants.STRING_DELIMITER, node.Value, CTranslatorConstants.STRING_DELIMITER);

			addAssignment (typeNames[node.EvaluationType], node.Location, strVal, node.Scope, declared: declared);
		}

		public void VisitIOPrintNode(IOPrintNode node)
		{
			string functionCall = "";

			foreach (ExpressionNode expression in node.Arguments.Arguments) {
				expression.Accept (this);

				switch (expression.EvaluationType) {
					case TokenType.INTEGER_VAL:
					case TokenType.BOOLEAN_VAL:
						functionCall = CreateFunctionCall ("print_int", expression.Location);
						break;
					case TokenType.REAL_VAL:
						functionCall = CreateFunctionCall ("print_float", expression.Location);
						break;
					case TokenType.STRING_VAL:
						functionCall = CreateFunctionCall ("print_string", expression.Location);
						break;
				}

				addToScopeTranslation (statement (functionCall));

				nameFactory.ReturnTempVarId (currentScope, expression.Location, expression.EvaluationType, expression);
			}

			functionCall = CreateFunctionCall ("print_linebreak");

			addToScopeTranslation (statement (functionCall));
		}

		public void VisitIOReadNode(IOReadNode node)
		{
			string[] arguments = new string[node.IDNodes.Count + 1];
			SyntaxTreeNode argAmountTempNode = new TranslatorTempNode ();
			SyntaxTreeNode readAmountTempNode = new TranslatorTempNode ();
			string argumentAmountTemp = GetTempVarId (TokenType.INTEGER_VAL, new IntegerProperty (), argAmountTempNode);
			string readAmountTemp = GetTempVarId (TokenType.INTEGER_VAL, new IntegerProperty (), readAmountTempNode);

			string formatString = createScanfFormatString (node.IDNodes);
			arguments [0] = formatString;

			for (int i = 0, j = 1; i < node.IDNodes.Count; i++, j++) {
				Evaluee idNode = node.IDNodes [i];
				string id = nameFactory.GetCName (node.Scope, idNode.VariableID);
				if (currentScope.GetProperty (idNode.VariableID).Reference) {
					arguments [j] = id;
				} else {
					arguments [j] = CTranslatorConstants.ADDRESS_PREFIXES [idNode.EvaluationType] + id;
				}
			}

			string functionCall = CreateFunctionCall ("scanf", arguments);

			addToScopeTranslation (simpleAssignment (argumentAmountTemp, node.IDNodes.Count.ToString ()));
			addToScopeTranslation (simpleAssignment (readAmountTemp, functionCall));
			addEmptyLineToScopeTranslation ();

			addConditionalJumpsToScopeTranslation (CTranslatorConstants.ERROR_LABEL, neqOperationStrings(argumentAmountTemp, readAmountTemp));

			nameFactory.ReturnTempVarId (currentScope, argumentAmountTemp, TokenType.INTEGER_VAL, argAmountTempNode);
			nameFactory.ReturnTempVarId (currentScope, readAmountTemp, TokenType.INTEGER_VAL, readAmountTempNode);
		}

		public string createScanfFormatString (List<Evaluee> idNodes)
		{
			StringBuilder sb = new StringBuilder ("\"");
			int i = 0;

			for (; i < idNodes.Count - 1; i++) {
				sb.Append (CTranslatorConstants.STRING_FORMATTING_SYMBOLS [idNodes [i].EvaluationType] + ' ');
			}

			sb.Append (CTranslatorConstants.STRING_FORMATTING_SYMBOLS [idNodes [i].EvaluationType] + '\"');

			return sb.ToString ();
		}

		public string createBinaryOperationString (TokenType operation, string lhs, string rhs)
		{
			return spaced (lhs, CTranslatorConstants.OPERATION_STRINGS [operation], rhs);
		}


		public void addConditionalJumpsToScopeTranslation (string label, params string[] conditions)
		{
			string ifStr = CTranslatorConstants.IF;
			string gotoStr = CTranslatorConstants.GOTO;

			foreach (string condition in conditions) {
				addToScopeTranslation (statement (ifStr, parenthesized (condition), gotoStr, label));
			}
		}

		public void VisitTypeNode(TypeNode node)
		{}

		public void VisitBlockNode(BlockNode node)
		{
			List<StatementNode> statements = node.Statements;
			int stmntsCount = statements.Count;
			ReturnStatement returnStmnt = statements [stmntsCount - 1].Token.Type == TokenType.RETURN ? (ReturnStatement)statements [stmntsCount - 1] : null;
			int upto = returnStmnt == null ? stmntsCount : stmntsCount - 1;
			int i = 0;
			for (; i < upto; i++) {
				statements[i].Accept (this);
			}

			createAllocationReleases (currentScope, returnStmnt != null ? returnStmnt.ReturnValue.Location : null);

			if (i < stmntsCount) {
				statements [i].Accept (this);
			}
		}

		public void VisitBooleanNegation(BooleanNegation node)
		{
			setNodeLocation (node, TokenType.BOOLEAN_VAL, getProperty (TokenType.BOOLEAN_VAL));
			node.Factor.Location = node.Location;
			node.Factor.Accept (this);
			addAssignment (typeNames [TokenType.BOOLEAN_VAL], node.Location, "!" + node.Location, node.Scope);
		}

		public void VisitExpressionNode(ExpressionNode node)
		{
			node.SimpleExpression.Accept (this);

			if (node.ExpressionTail != null) {
				setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType));
				node.ExpressionTail.Accept (this);
				addRelationalOperation (node);
				nameFactory.ReturnTempVarId (currentScope, node.ExpressionTail.Location, node.ExpressionTail.EvaluationType, node.ExpressionTail);
			} else {
				node.Location = node.SimpleExpression.Location;
			}
		}

		public Property getProperty(TokenType type, TokenType arrayElementType = TokenType.UNDEFINED, ExpressionNode arraySizeExpression = null)
		{
			switch (type) {
				case TokenType.INTEGER_VAL:
				case TokenType.BOOLEAN_VAL:
					return new IntegerProperty ();
				case TokenType.REAL_VAL:
					return new RealProperty ();
				case TokenType.STRING_VAL:
					return new StringProperty ();
				case TokenType.TYPE_ARRAY:
					return new ArrayProperty (arrayElementType, arraySizeExpression);
				default:
					return new VoidProperty ();
			}
		}

		public void addRelationalOperation (ExpressionNode node)
		{
			string label = nameFactory.GetLabel ();
			addAssignment (typeNames [node.EvaluationType], node.Location, "0", node.Scope);

			switch (node.ExpressionTail.Operation) {
				case TokenType.BINARY_OP_LOG_EQ:
					addRelationalGotoOperations (node, label, TokenType.BINARY_OP_LOG_LT, TokenType.BINARY_OP_LOG_GT);
					break;
				case TokenType.BINARY_OP_LOG_GT:
					addRelationalGotoOperations (node, label, TokenType.BINARY_OP_LOG_LT, TokenType.BINARY_OP_LOG_EQ);
					break;
				case TokenType.BINARY_OP_LOG_GTE:
					addRelationalGotoOperations (node, label, TokenType.BINARY_OP_LOG_LT);
					break;
				case TokenType.BINARY_OP_LOG_LT:
					addRelationalGotoOperations (node, label, TokenType.BINARY_OP_LOG_GT, TokenType.BINARY_OP_LOG_EQ);
					break;
				case TokenType.BINARY_OP_LOG_LTE:
					addRelationalGotoOperations (node, label, TokenType.BINARY_OP_LOG_GT);
					break;
				case TokenType.BINARY_OP_LOG_NEQ:
					addRelationalGotoOperations (node, label, TokenType.BINARY_OP_LOG_EQ);
					break;
			}

			addAssignment (typeNames [node.EvaluationType], node.Location, "1", node.Scope);
			addToScopeTranslation (statement (label + ":"));
		}

		public void addRelationalGotoOperations (ExpressionNode node, string label, params TokenType[] operations)
		{
			foreach (TokenType operation in operations) {
				addToScopeTranslation (statement (spaced ("if", "(" + node.SimpleExpression.Location, CTranslatorConstants.OPERATION_STRINGS[operation], node.ExpressionTail.Location + ")", "goto", label)));
			}
		}

		public void VisitSimpleExpression (SimpleExpression node)
		{
			setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType));

			node.Term.Location = node.Location;
			node.Term.Accept (this);

			if (node.AdditiveInverse) {
				addToScopeTranslation (node.Location + " = -1 * " + node.Location + ";");
			}

			if (node.Tail != null) {
				node.Tail.SubTotal = node.Location;
				node.Tail.Accept (this);
				nameFactory.ReturnTempVarId (currentScope, node.Tail.Location, node.Tail.EvaluationType, node.Tail);
			}
		}

		public void VisitSimpleExpressionTail (SimpleExpressionTail node)
		{
			setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType));

			node.Term.Location = node.Location;
			node.Term.Accept (this);
			if (node.EvaluationType == TokenType.STRING_VAL) {
				addToScopeTranslation (simpleAssignment (node.SubTotal, CreateFunctionCall ("string_concatenation", node.SubTotal, node.Location)));
			} else {
				addAssignment (typeNames [node.EvaluationType], node.SubTotal, node.SubTotal, node.Scope, opStrings [node.Operation], node.Location);
			}

			nameFactory.ReturnTempVarId (currentScope, node.Term.Location, node.Term.EvaluationType, node.Term);
			if (node.Tail != null) {
				node.Tail.SubTotal = node.SubTotal;
				node.Tail.Accept (this);
				nameFactory.ReturnTempVarId (currentScope, node.Tail.Location, node.Tail.EvaluationType, node.Tail);
			}
		}

		public void VisitExpressionTail(ExpressionTail node)
		{
			setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType));
			node.RightHandSide.Location = node.Location;
			node.RightHandSide.Accept (this);
		}

		public void VisitFactorNode(Factor node)
		{
			setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType));
			node.FactorMain.Location = node.Location;
			node.FactorMain.Accept (this);
		}

		public void VisitFactorMain(FactorMain node)
		{
			setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType));

			node.Evaluee.Location = node.Location;
			node.Evaluee.Accept (this);
		}

		public void VisitFactorTail(FactorTail node)
		{}

		public void VisitFunctionNode(FunctionNode node)
		{
			string head = createFunctionStart(nameFactory.GetCName(node.Scope, node.IDNode.ID), node.IDNode, nameFactory, node.Parameters.Parameters);
			this.errorHandlingCode = CTranslatorConstants.GetFunctionErrorHandlingCode (node.ReturnType);

			node.Parameters.Accept (this);

			this.blockDepth++;
			node.Block.Accept (this);
			addErrorLabelAndCodeToScopeTranslation ();
			this.blockDepth--;

			addToGlobalTranslation (head);
			addToGlobalTranslation (CTranslatorConstants.BLOCK_DELIMITERS.Item1);

			this.blockDepth++;
			addVariableDeclarationsToScopeTranslation (currentScope, declaredVariables [currentScope]);
			globalTranslation = globalTranslation.Concat (scopeTranslation).ToList ();
			this.blockDepth--;

			addToGlobalTranslation (CTranslatorConstants.BLOCK_DELIMITERS.Item2);

			addEmptyLineToGlobalTranslation ();
		}

		public void VisitProcedureNode(ProcedureNode node)
		{
			VisitFunctionNode (node);
		}

		public void VisitFunctionCallNode(FunctionCallNode node)
		{
			string functionName = nameFactory.GetCName (node.Scope, node.IdNode.ID);
			FunctionNode function = programNode.Functions [node.IdNode.ID];
			List<ExpressionNode> arguments = node.ArgumentsNode.Arguments;
			List<Parameter> parameters = function.Parameters.Parameters;
			string[] argumentStrings = new string[arguments.Count];

			for (int i = 0; i < argumentStrings.Length; i++) {
				if (parameters [i].Reference) {
					VariableEvaluee varEvaluee = (VariableEvaluee)arguments [i].SimpleExpression.Term.Factor.FactorMain.Evaluee;
					string id = nameFactory.GetCName (node.Scope, varEvaluee.IDNode.ID);
					argumentStrings [i] = CTranslatorConstants.ADDRESS_PREFIXES [arguments [i].EvaluationType] + id;
				} else {
					arguments [i].Accept (this);
					argumentStrings [i] = arguments [i].Location;
				}
			}

			StringBuilder sb = new StringBuilder ("(");
			for (int i = 0; i < argumentStrings.Length - 1; i++) {
				sb.Append (argumentStrings [i] + ", ");
			}

			sb.Append(argumentStrings[argumentStrings.Length - 1] + ")");

			if (node.Location != null) {
				addAssignment (typeNames [node.EvaluationType], node.Location, unspaced (functionName, sb.ToString ()), node.Scope);
			} else {
				addToScopeTranslation (statement (unspaced (functionName, sb.ToString ())));
			}

			addErrorCheckingToScopeTranslation ();

			foreach (ExpressionNode expression in arguments) {
				if (!String.IsNullOrEmpty (expression.Location)) {
					nameFactory.ReturnTempVarId (currentScope, expression.Location, expression.EvaluationType, expression);
				}
			}
		}

		public void VisitIfNode(IfNode node)
		{
			node.IfBranch.Label = nameFactory.GetLabel ();

			node.Condition.Accept (this);
			addToScopeTranslation (statement (spaced (node.Condition.Location, CTranslatorConstants.ASSIGNMENT, "!" + node.Condition.Location)));
			addEmptyLineToScopeTranslation ();
			addToScopeTranslation (statement (spaced ("if", "(" + node.Condition.Location + ")", "goto", node.IfBranch.Label)));
			addEmptyLineToScopeTranslation ();
			node.IfBranch.Accept (this);
			addEmptyLineToScopeTranslation ();

			if (node.ElseBranch != null) {
				node.ElseBranch.Label = nameFactory.GetLabel ();
				addToScopeTranslation (statement (spaced ("goto", node.ElseBranch.Label)));
				addEmptyLineToScopeTranslation ();
				addToScopeTranslation (node.IfBranch.Label + ":");
				node.ElseBranch.Accept (this);
				addEmptyLineToScopeTranslation ();
				addToScopeTranslation (node.ElseBranch.Label + ":;");
			} else {
				addToScopeTranslation (node.IfBranch.Label + ":;");
			}
			nameFactory.ReturnTempVarId (currentScope, node.Condition.Location, node.Condition.EvaluationType, node.Condition);
			addEmptyLineToScopeTranslation ();
		}

		public void VisitWhileLoopNode(WhileNode node)
		{
			string loopLabel = nameFactory.GetLabel ();
			string conditionCheckLabel = nameFactory.GetLabel ();

			addToScopeTranslation (statement (spaced ("goto", conditionCheckLabel)));
			addEmptyLineToScopeTranslation ();
			addToScopeTranslation (loopLabel + ":");
			node.Statement.Accept (this);
			addEmptyLineToScopeTranslation ();
			addToScopeTranslation (conditionCheckLabel + ":");
			node.Condition.Accept (this);
			addToScopeTranslation (statement (spaced ("if", "(" + node.Condition.Location + ")", "goto", loopLabel)));
		}

		public void VisitParametersNode(ParametersNode node)
		{
			foreach (Parameter param in node.Parameters) {
				string id = nameFactory.GetCName (node.Scope, param.IdNode.ID);
				Property prop = new ErrorProperty ();

				switch (param.ParameterType) {
					case TokenType.INTEGER_VAL:
						prop = new IntegerProperty (node.Token.Row, true, param.Reference);
						break;
					case TokenType.REAL_VAL:
						prop = new RealProperty (node.Token.Row, true, param.Reference);
						break;
					case TokenType.STRING_VAL:
						prop = new StringProperty (node.Token.Row, true);
						break;
					case TokenType.TYPE_ARRAY:
						prop = new ArrayProperty (param.IdNode.ArrayElementType, declarationRow: node.Token.Row);
						break;
				}

				prop.Declared = true;
				declaredVariables [currentScope] [id] = prop;
			}
		}

		public void VisitArgumentsNode(ArgumentsNode node)
		{}

		public void VisitProgramNode(ProgramNode node)
		{
			this.programNode = node;
			createProgramStart (node);

			foreach (FunctionNode func in node.Functions.Values) {
				changeScope (func.Scope);
				func.Accept (this);
			}

			createProgramFunction (node);
		}

		private void changeScope(Scope scope)
		{
			this.declaredVariables [scope] = new Dictionary<string, Property> ();
			this.currentScope = scope;
			scopeTranslation = new List<string> ();
		}

		private void createProgramFunction(ProgramNode node) {
			Scope scope = node.Scope;
			string head = "int " + nameFactory.GetCName (scope, programName) + "()";
			this.errorHandlingCode = CTranslatorConstants.GetProgramFunctionErrorHandlingCode ();
			changeScope (scope);
			blockDepth++;

			node.MainBlock.Accept (this);

			addToScopeTranslation (statement ("return 0"));
			addEmptyLineToScopeTranslation ();
			addErrorLabelAndCodeToScopeTranslation ();
			this.blockDepth--;
			addToGlobalTranslation (head);
			addToGlobalTranslation (CTranslatorConstants.BLOCK_DELIMITERS.Item1);
			this.blockDepth++;
			addVariableDeclarationsToScopeTranslation (currentScope, declaredVariables [currentScope]);
			globalTranslation = globalTranslation.Concat (scopeTranslation).ToList ();
			blockDepth--;
			addToGlobalTranslation (CTranslatorConstants.BLOCK_DELIMITERS.Item2);
		}

		public void VisitReturnStatement(ReturnStatement node)
		{
			node.ReturnValue.Accept (this);
			addToScopeTranslation (statement ("return", node.ReturnValue.Location));
			nameFactory.ReturnTempVarId (currentScope, node.ReturnValue.Location, node.ReturnValue.EvaluationType, node.ReturnValue);
		}

		public void VisitTermNode(TermNode node)
		{
			Factor factor = node.Factor;
			TermTail tail = node.TermTail;

			setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType));

			factor.Location = node.Location;
			factor.Accept (this);

			if (tail != null) {
				tail.SubTotal = factor.Location;
				tail.Accept (this);
				nameFactory.ReturnTempVarId (currentScope, tail.Location, tail.EvaluationType, tail);
			}
		}

		public void VisitTermTailNode(TermTail node)
		{
			setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType));
			Factor factor = node.Factor;
			TermTail tail = node.ChildTermTail;

			factor.Location = node.Location;
			factor.Accept (this);

			addAssignment (typeNames[factor.EvaluationType], node.SubTotal, node.SubTotal, node.Scope, opStrings [node.Operation], factor.Location);

			if (tail != null) {
				tail.SubTotal = node.Location;
				tail.Accept (this);
				nameFactory.ReturnTempVarId (currentScope, tail.Location, tail.EvaluationType, tail);
			}
		}

		public void VisitVariableIdNode(VariableIdNode node)
		{
			string id = nameFactory.GetCName (node.Scope, node.ID);
			bool reference = false;

			if (!declaredVariables [currentScope].ContainsKey (id)) {
				declaredVariables [currentScope] [id] = node.Scope.GetProperty (node.ID);
			} else {
				reference = declaredVariables [currentScope] [id].Reference;
			}

			addToCurrentScope (id, node.Scope.GetProperty (node.ID));
			setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType));

			if (typeNames.ContainsKey (node.EvaluationType)) {
				if (reference) {
					addAssignment (typeNames [node.EvaluationType], node.Location, "*" + id, node.Scope);
				} else {
					addAssignment (typeNames [node.EvaluationType], node.Location, id, node.Scope);
				}
			}
		}

		public void addToCurrentScope (string id, Property property)
		{
			if (!declaredVariables [currentScope].ContainsKey (id)) {
				declaredVariables [currentScope] [id] = property;
			}
		}

		private void createProgramStart (ProgramNode programNode)
		{
			includeLibraries ();
			addEmptyLineToGlobalTranslation ();
			addGlobalVariablesToTranslation ();
			addEmptyLineToGlobalTranslation ();
			createHelperFunctionDeclarations ();
			addEmptyLineToGlobalTranslation ();
			createFunctionDeclarations (programNode);
			addEmptyLineToGlobalTranslation ();
			createHelperFunctions ();
			addEmptyLineToGlobalTranslation ();
			createMainFunction (programName, programNode.Scope);
			addEmptyLineToGlobalTranslation ();
		}

		private void addGlobalVariablesToTranslation ()
		{
			addComment ("globally used variables");

			foreach (string str in CTranslatorConstants.GLOBAL_VARIABLE_ASSIGNMENTS) {
				addToGlobalTranslation (statement (str));
			}
		}

		private void includeLibraries ()
		{
			foreach (string library in CTranslatorConstants.LIBRARIES) {
				addToGlobalTranslation (LibraryInclusion (library));
			}
		}
			
		private void createFunctionDeclarations (ProgramNode programNode)
		{
			addComment("program functions");

			createProgramFunctionDeclaration(programName, programNode.Scope);

			foreach (string func in programNode.Functions.Keys) {
				FunctionNode functionNode = programNode.Functions [func];
				addToGlobalTranslation (createFunctionDeclaration (func, functionNode.IDNode, functionNode.Parameters.Parameters));
			}
		}

		private void createHelperFunctionDeclarations ()
		{
			addComment("helper functions");

			foreach (string str in CTranslatorConstants.HELPER_FUNCTION_DECLARATIONS) {
				addToGlobalTranslation (statement (str));
			}
		}

		private void addComment(string line)
		{
			addToGlobalTranslation (spaced (CTranslatorConstants.COMMENT_DELIMITERS.Item1, line, CTranslatorConstants.COMMENT_DELIMITERS.Item2));
		}

		private void createHelperFunctions ()
		{
			foreach (string[] helperFunction in CTranslatorConstants.HELPER_FUNCTIONS) {
				foreach (string str in helperFunction) {
					addToGlobalTranslation (str);
				}

				addEmptyLineToGlobalTranslation ();
			}
		}

		private string createFunctionDeclaration (string functionName, VariableIdNode idNode, List<Parameter> parameters = null)
		{
			return createFunctionDeclaration (nameFactory.GetCName (idNode.Scope, functionName), idNode, nameFactory, parameters);
		}

		private void createProgramFunctionDeclaration(string programName, Scope scope)
		{
			addToGlobalTranslation ("int " + nameFactory.GetCName (scope, programName) + "();");
		}

		private void createMainFunction (string programName, Scope scope)
		{
			addToGlobalTranslation ("int main()");
			addToGlobalTranslation (CTranslatorConstants.BLOCK_DELIMITERS.Item1);
			this.blockDepth++;
			addToGlobalTranslation ("return " + nameFactory.GetCName (scope, programName) + "();");
			this.blockDepth--;
			addToGlobalTranslation (CTranslatorConstants.BLOCK_DELIMITERS.Item2);
		}

		private void addEmptyLineToGlobalTranslation () {
			addToTranslation (globalTranslation, "");
		}

		private void addEmptyLineToScopeTranslation () {
			addToTranslation (scopeTranslation, "");
		}

		private void addEmptyLineToTranslation (List<string> translation) {
			addToTranslation (translation, "");
		}

		private void addToScopeTranslation(string line) {
			addToTranslation (scopeTranslation, line);
		}

		private void addToGlobalTranslation(string line) {
			addToTranslation (globalTranslation, line);
		}

		private void addToTranslation (List<string> translation, string line)
		{
			StringBuilder sb = new StringBuilder ();

			for (int i = 0; i < this.blockDepth; i++) {
				sb.Append ('\t');
			}

			sb.Append (line);
			sb.Append ("\n");

			translation.Add (sb.ToString ());
		}

		public static string LibraryInclusion (string library)
		{
			return spaced (CTranslatorConstants.INCLUSION, LibraryDelimit(library));
		}

		private static string LibraryDelimit (string library)
		{
			string leftDelimiter = CTranslatorConstants.LIBRARY_INCLUSION_DELIMITERS.Item1;
			string rightDelimiter = CTranslatorConstants.LIBRARY_INCLUSION_DELIMITERS.Item2;

			return unspaced(leftDelimiter, library, rightDelimiter);
		}

		private static string unspaced (params string[] strings)
		{
			StringBuilder sb = new StringBuilder ();

			for (int i = 0; i < strings.Length; i++) {
				sb.Append (strings [i]);
			}

			return sb.ToString ();
		}

		private static string spaced (params string[] strings)
		{
			StringBuilder sb = new StringBuilder ();

			for (int i = 0; i < strings.Length - 1; i++) {
				if (strings [i] != null) {
					sb.Append (strings [i] + " ");
				}
			}

			sb.Append (strings [strings.Length - 1]);

			return sb.ToString ();
		}

		public static string GetDeclarationType (TypeNode typeNode)
		{
			return CTranslatorConstants.SIMPLE_TYPE_NAMES [typeNode.PropertyType];
		}

		public static string createFunctionDeclaration (string functionName, VariableIdNode idNode, CNameFactory nameFactory, List<Parameter> parameters = null)
		{
			string returnType = null;
			Dictionary<TokenType, string> types = CTranslatorConstants.SIMPLE_TYPE_NAMES;

			if (types.ContainsKey(idNode.VariableType)) {
				returnType = typeNames [idNode.VariableType];
			} else {
				returnType = typeNames [idNode.ArrayElementType] + '*';
			}

			string parameterString = createParameters (nameFactory, idNode.Scope, parameters);

			return statement(spaced(returnType, functionName, parameterString));
		}

		public static string createFunctionStart (string functionName, VariableIdNode idNode, CNameFactory nameFactory, List<Parameter> parameters = null)
		{
			string returnType = null;

			if (typeNames.ContainsKey(idNode.VariableType)) {
				returnType = typeNames [idNode.VariableType];
			} else {
				returnType = typeNames [idNode.ArrayElementType] + '*';
			}

			string parameterString = createParameters (nameFactory, idNode.Scope, parameters);

			return spaced(returnType, functionName, parameterString);
		}

		private static string createParameters (CNameFactory nameFactory, Scope scope, List<Parameter> parameters = null)
		{
			StringBuilder sb = new StringBuilder (CTranslatorConstants.CALL_TAIL_DELIMITERS.Item1);

			if (parameters != null && parameters.Count > 0) {
				for (int i = 0; i < parameters.Count - 1; i++) {
					sb.Append (parameterToString (nameFactory, parameters [i], scope) + ", ");
				}

				sb.Append (parameterToString (nameFactory, parameters [parameters.Count - 1], scope));
			}

			sb.Append (CTranslatorConstants.CALL_TAIL_DELIMITERS.Item2);

			return sb.ToString ();
		}

		private static string parameterToString (CNameFactory nameFactory, Parameter param, Scope scope)
		{
			StringBuilder sb = new StringBuilder ("");

			if (paramTypeNames.ContainsKey (param.ParameterType)) {
				sb.Append (paramTypeNames [param.ParameterType]);
				if (param.Reference) {
					sb.Append ('*');
				}

				sb.Append (' ');
				sb.Append (nameFactory.GetCName (scope, param.IdNode.ID));
			} else {
				sb.Append (typeNames [param.IdNode.ArrayElementType]);
				sb.Append ('*');

				sb.Append (' ');
				sb.Append (nameFactory.GetCName (scope, param.IdNode.ID));
			}

			return sb.ToString ();
		}

		public static string statement (params string[] str)
		{
			return spaced(str) + ';';
		}

		private void addAllocation (TokenType type, string times, string id)
		{
			if (!this.allocations.ContainsKey (currentScope)){
				this.allocations[currentScope] = new List<string> ();
			}

			this.allocations[currentScope].Add (id);

			SyntaxTreeNode tempNode = new TranslatorTempNode ();
			string tempLoc = GetTempVarId (TokenType.INTEGER_VAL, new IntegerProperty (), tempNode);
			addToScopeTranslation(statement (spaced (tempLoc, CTranslatorConstants.ASSIGNMENT, times, opStrings[TokenType.BINARY_OP_MUL], sizeOfString(typeNames[type]))));

			string typePointer = unspaced(typeNames[type], CTranslatorConstants.MEM_POINTER);
			string malloc = unspaced (CTranslatorConstants.MEM_ALLOCATION, CTranslatorConstants.CALL_TAIL_DELIMITERS.Item1, tempLoc, CTranslatorConstants.CALL_TAIL_DELIMITERS.Item2);
			string mallocCall = statement(spaced(typePointer, id, CTranslatorConstants.ASSIGNMENT, malloc));
			addToScopeTranslation (mallocCall);

			nameFactory.ReturnTempVarId (currentScope, tempLoc, type, tempNode);
		}

		private void createAllocationReleases (Scope scope, string returnId = null)
		{
			if (!allocations.ContainsKey (scope)) {
				return;
			}

			foreach (string id in allocations[scope]) {
				if (returnId == null || returnId != id) {
					addAllocationRelease (id);
				}
			}
		}

		private void addAllocationRelease (string id)
		{
			addToScopeTranslation(statement (unspaced (CTranslatorConstants.MEM_RELEASE, CTranslatorConstants.CALL_TAIL_DELIMITERS.Item1, id, CTranslatorConstants.CALL_TAIL_DELIMITERS.Item2)));
		}

		private string sizeOfString(string type)
		{
			string leftDelimiter = CTranslatorConstants.CALL_TAIL_DELIMITERS.Item1;
			string rightDelimiter = CTranslatorConstants.CALL_TAIL_DELIMITERS.Item2;

			return unspaced (CTranslatorConstants.SIZE_OF, leftDelimiter, type, rightDelimiter);
		}

		private void addAssignment(string type, string target, string firstOperand, Scope scope, string operation = null, string secondOperand = null, bool declared = false)
		{
			string assignment = CTranslatorConstants.ASSIGNMENT;
			bool alreadyDeclared = declaredVariables [currentScope].ContainsKey (target);
			string typeAndTarget = target;

			if (operation != null) {
				addToScopeTranslation (statement (spaced (typeAndTarget, assignment, firstOperand, operation, secondOperand)));
			} else {
				addToScopeTranslation (statement (spaced (typeAndTarget, assignment, firstOperand)));
			}

			if (!alreadyDeclared) {
				declaredVariables [currentScope] [target] = null;
			}
		}

		private void setNodeLocation(SyntaxTreeNode node, TokenType type, Property property) {
			if (node.Location == null) {
				string location = GetTempVarId (type, property, node);
				node.Location = location;
			} else {
				nameFactory.updateLocationUsage (currentScope, node.Location, type, node);
			}
		}

		private string GetTempVarId (TokenType type, Property property, SyntaxTreeNode node)
		{
			string tempVarId = nameFactory.GetTempVarId (currentScope, type, node);

			if (!declaredVariables [currentScope].ContainsKey (tempVarId)) {
				declaredVariables [currentScope] [tempVarId] = property;
			}

			return tempVarId;
		}

		private void addVariableDeclarationsToScopeTranslation (Scope scope, Dictionary<string, Property> variables)
		{
			List<string> scopeDeclarations = new List<string> ();

			foreach (string key in variables.Keys) {
				if (scope.ContainsKey(key)) {
					continue;
				}

				Property prop = variables [key];

				if (prop.Declared) {
					continue;
				}

				TokenType type = prop.GetTokenType ();

				switch (type) {
					case TokenType.TYPE_ARRAY:
						ArrayProperty aProp = (ArrayProperty)prop;
						TokenType elementType = aProp.ArrayElementType;
						addToTranslation (scopeDeclarations, simpleAssignment(spaced (typeNames [elementType] + CTranslatorConstants.MEM_POINTER, key), CTranslatorConstants.TEMP_VARIABLES[elementType]));
						break;
					case TokenType.STRING_VAL:
						addToTranslation (scopeDeclarations, simpleAssignment (spaced (typeNames [type] + CTranslatorConstants.MEM_POINTER, key), CTranslatorConstants.TEMP_VARIABLES[type]));
						break;
					default:
						addToTranslation (scopeDeclarations, simpleAssignment (spaced (typeNames [type], key), "0"));
						break;
				}
			}

			scopeTranslation = scopeDeclarations.Concat (scopeTranslation).ToList ();
			addEmptyLineToScopeTranslation ();
		}
	}
}