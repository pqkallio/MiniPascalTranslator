using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Compiler
{
	public class CTranslationVisitor : INodeVisitor
	{
		private List<string> globalTranslation;
		private List<string> scopeTranslation;
		private string programName;
		private CNameFactory nameFactory;
		private int blockDepth;
		private static Dictionary<TokenType, string> typeNames = CTranslatorConstants.SIMPLE_TYPE_NAMES;
		private static Dictionary<TokenType, string> paramTypeNames = CTranslatorConstants.PARAM_SIMPLE_TYPE_NAMES;
		private static Dictionary<TokenType, string> opStrings = CTranslatorConstants.OPERATION_STRINGS;
		private Dictionary<Scope, List<string>> allocations;
		private Dictionary<Scope, Dictionary<string, Property>> declaredVariables;
		private ProgramNode programNode;
		private Scope currentScope;

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
		}

		public List<string> Translation
		{
			get { return this.globalTranslation; }
		}

		public void VisitAssertNode(AssertNode node)
		{}

		public void VisitArraySizeCheckNode(ArraySizeCheckNode node)
		{
			bool declared = false;
			setNodeLocation (node, TokenType.INTEGER_VAL, new IntegerProperty(), ref declared);
			string arrayId = nameFactory.GetCName (node.Scope, node.VariableID);
			addToScopeTranslation (statement (spaced (node.Location, CTranslatorConstants.ASSIGNMENT, "(int)", unspaced (arrayId, CTranslatorConstants.ARRAY_INDEX_DELIMITERS.Item1, "0", CTranslatorConstants.ARRAY_INDEX_DELIMITERS.Item2))));
		}

		public void VisitAssignNode(AssignNode node)
		{
			string idName = nameFactory.GetCName (node.Scope, node.IDNode.IDNode.ID);
			node.AssignValueExpression.Accept (this);
			addToScopeTranslation (statement (spaced (idName, "=", node.AssignValueExpression.Location)));
			nameFactory.ReturnTempVarId (node.AssignValueExpression.Scope, node.AssignValueExpression.Location, node.AssignValueExpression.EvaluationType);
		}

		public void VisitArrayAssignNode(ArrayAssignStatement node)
		{}

		public void VisitArrayAccessNode(ArrayAccessNode node)
		{
			bool declared = false;
			setNodeLocation (node, node.EvaluationType, new ArrayProperty(node.EvaluationType), ref declared);
			node.ArrayIndexExpression.Accept (this);
			string arrayId = nameFactory.GetCName (node.Scope, node.VariableID);
			switch (node.EvaluationType) {
				case (TokenType.INTEGER_VAL):
				case (TokenType.BOOLEAN_VAL):
					addToScopeTranslation (statement (CreateFunctionCall ("load_from_int_array", arrayId, node.ArrayIndexExpression.Location, CTranslatorConstants.MEM_ADDRESS + node.Location)));
					break;
				case (TokenType.REAL_VAL):
					addToScopeTranslation (statement (CreateFunctionCall ("load_from_float_array", arrayId, node.ArrayIndexExpression.Location, CTranslatorConstants.MEM_ADDRESS + node.Location)));
					break;
				case (TokenType.STRING_VAL):
					addToScopeTranslation (statement (CreateFunctionCall ("load_from_string_array", arrayId, node.ArrayIndexExpression.Location, CTranslatorConstants.MEM_ADDRESS + node.Location)));
					break;
			}
		}

		private string CreateFunctionCall (string functionId, params string[] arguments)
		{
			StringBuilder sb = new StringBuilder (functionId + "(");

			int i;
			for (i = 0; i < arguments.Length - 1; i++) {
				sb.Append (arguments [i] + ", ");
			}

			sb.Append (arguments [i] + ")");

			return sb.ToString();
		}

		public void VisitDeclarationNode(DeclarationNode node)
		{
			Scope scope = node.Scope;

			if (node.DeclarationType.PropertyType == TokenType.TYPE_ARRAY) {
				foreach (VariableIdNode idNode in node.IDsToDeclare) {
					string id = nameFactory.GetCName(scope, idNode.ID);
					declaredVariables [currentScope] [id] = getProperty(TokenType.TYPE_ARRAY, idNode.ArrayElementType, node.DeclarationType.ArraySizeExpression);
				}
			} else {
				foreach (VariableIdNode idNode in node.IDsToDeclare) {
					string id = nameFactory.GetCName(scope, idNode.ID);
					declaredVariables [currentScope] [id] = getProperty(node.Token.Type);
				}
			}
		}

		public void VisitIntValueNode(IntValueNode node)
		{	
			bool declared = false;
			setNodeLocation (node, node.EvaluationType, new IntegerProperty(), ref declared);

			addAssignment (typeNames[node.EvaluationType], node.Location, node.Value, node.Scope, declared: declared);
		}

		public void VisitRealValueNode(RealValueNode node)
		{
			bool declared = false;
			setNodeLocation (node, node.EvaluationType, new RealProperty(), ref declared);

			addAssignment (typeNames[node.EvaluationType], node.Location, node.Value, node.Scope, declared: declared);
		}

		public void VisitBoolValueNode(BoolValueNode node)
		{
			bool declared = false;
			setNodeLocation (node, node.EvaluationType, new IntegerProperty(), ref declared);

			string strVal = node.Value ? "1" : "0";

			addAssignment (typeNames[node.EvaluationType], node.Location, strVal, node.Scope, declared: declared);
		}

		public void VisitStringValueNode(StringValueNode node)
		{
			bool declared = false;
			setNodeLocation (node, node.EvaluationType, new StringProperty(), ref declared);

			string strVal = unspaced (CTranslatorConstants.STRING_DELIMITER, node.Value, CTranslatorConstants.STRING_DELIMITER);

			addAssignment (typeNames[node.EvaluationType], node.Location, strVal, node.Scope, declared: declared);
		}

		public void VisitIOPrintNode(IOPrintNode node)
		{}

		public void VisitIOReadNode(IOReadNode node)
		{}

		public void VisitTypeNode(TypeNode node)
		{}

		public void VisitBlockNode(BlockNode node)
		{
			// addToTranslation (CTranslatorConstants.BLOCK_DELIMITERS.Item1);

			List<StatementNode> statements = node.Statements;
			int stmntsCount = statements.Count;
			ReturnStatement returnStmnt = statements [stmntsCount - 1].Token.Type == TokenType.RETURN ? (ReturnStatement)statements [stmntsCount - 1] : null;
			int upto = returnStmnt == null ? stmntsCount : stmntsCount - 1;
			int i = 0;
			for (; i < upto; i++) {
				statements[i].Accept (this);
			}

			createAllocationReleases (node.Scope, returnStmnt != null ? returnStmnt.ReturnValue.Location : null);

			if (i < stmntsCount) {
				statements [i].Accept (this);
			}

			// addToTranslation (CTranslatorConstants.BLOCK_DELIMITERS.Item2);
		}

		public void VisitBooleanNegation(BooleanNegation node)
		{}

		public void VisitExpressionNode(ExpressionNode node)
		{
			bool declared = false;
			node.SimpleExpression.Accept (this);

			if (node.ExpressionTail != null) {
				setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType), ref declared);
				node.ExpressionTail.Accept (this);
				addRelationalOperation (node);
				nameFactory.ReturnTempVarId (node.ExpressionTail.Scope, node.ExpressionTail.Location, node.ExpressionTail.EvaluationType);
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
			bool declared = false;
			setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType), ref declared);

			node.Term.Location = node.Location;
			node.Term.Accept (this);

			if (node.AdditiveInverse) {
				addToScopeTranslation (node.Location + " = -1 * " + node.Location + ";");
			}

			if (node.Tail != null) {
				node.Tail.SubTotal = node.Location;
				node.Tail.Accept (this);
			}
		}

		public void VisitSimpleExpressionTail (SimpleExpressionTail node)
		{
			bool declared = false;
			setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType), ref declared);

			node.Term.Location = node.Location;
			node.Term.Accept (this);

			addAssignment (typeNames[node.EvaluationType], node.SubTotal, node.SubTotal, node.Scope, opStrings [node.Operation], node.Location);

			if (node.Tail != null) {
				node.Tail.SubTotal = node.SubTotal;
				node.Tail.Accept (this);
			}
		}

		public void VisitExpressionTail(ExpressionTail node)
		{
			bool declared = false;
			setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType), ref declared);
			node.RightHandSide.Location = node.Location;
			node.RightHandSide.Accept (this);
		}

		public void VisitFactorNode(Factor node)
		{
			bool declared = false;
			setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType), ref declared);
			node.FactorMain.Location = node.Location;
			node.FactorMain.Accept (this);

			if (node.FactorTail != null) {
				// DO SUMTHIN!!!!
			}
		}

		public void VisitFactorMain(FactorMain node)
		{
			bool declared = false;
			setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType), ref declared);

			node.Evaluee.Location = node.Location;
			node.Evaluee.Accept (this);
		}

		public void VisitFactorTail(FactorTail node)
		{}

		public void VisitFunctionNode(FunctionNode node)
		{
			string head = createFunctionStart(nameFactory.GetCName(node.Scope, node.IDNode.ID), node.IDNode, nameFactory, node.Parameters.Parameters);
			this.blockDepth++;
			node.Block.Accept (this);
			blockDepth--;
			addToGlobalTranslation (head);
			addToGlobalTranslation (CTranslatorConstants.BLOCK_DELIMITERS.Item1);
			addVariableDeclarationsToScopeTranslation (currentScope, declaredVariables [currentScope]);
			globalTranslation = globalTranslation.Concat (scopeTranslation).ToList ();
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

			foreach (ExpressionNode expression in arguments) {
				nameFactory.ReturnTempVarId (expression.Scope, expression.Location, expression.EvaluationType);
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
			nameFactory.ReturnTempVarId (node.Condition.Scope, node.Condition.Location, node.Condition.EvaluationType);
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
		{}

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
			changeScope (scope);
			addToScopeTranslation ("int " + nameFactory.GetCName (scope, programName) + "()");
			addToScopeTranslation (CTranslatorConstants.BLOCK_DELIMITERS.Item1);
			blockDepth++;

			foreach (StatementNode statement in node.MainBlock.Statements) {
				statement.Accept (this);
			}

			addToScopeTranslation (statement ("return 0"));
			addEmptyLineToScopeTranslation ();
			addToScopeTranslation ("error:");
			addToScopeTranslation (statement ("return 1"));
			blockDepth--;
			addToScopeTranslation (CTranslatorConstants.BLOCK_DELIMITERS.Item2);
		}

		public void VisitReturnStatement(ReturnStatement node)
		{}

		public void VisitTermNode(TermNode node)
		{
			Factor factor = node.Factor;
			TermTail tail = node.TermTail;

			bool declared = false;
			setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType), ref declared);

			factor.Location = node.Location;
			factor.Accept (this);

			if (tail != null) {
				tail.SubTotal = factor.Location;
				tail.Accept (this);
				nameFactory.ReturnTempVarId (tail.Scope, tail.Location, tail.EvaluationType);
			}
		}

		public void VisitTermTailNode(TermTail node)
		{
			bool declared = false;
			setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType), ref declared);
			Factor factor = node.Factor;
			TermTail tail = node.ChildTermTail;

			factor.Location = node.Location;
			factor.Accept (this);

			addAssignment (typeNames[factor.EvaluationType], node.SubTotal, node.SubTotal, node.Scope, opStrings [node.Operation], factor.Location);

			if (tail != null) {
				tail.SubTotal = node.Location;
				tail.Accept (this);
			}
		}

		public void VisitVariableIdNode(VariableIdNode node)
		{
			bool declared = false;
			string id = nameFactory.GetCName (node.Scope, node.ID);

			addToCurrentScope (id, node.Scope.GetProperty (node.ID));
			setNodeLocation (node, node.EvaluationType, getProperty(node.EvaluationType), ref declared);

			if (typeNames.ContainsKey (node.EvaluationType)) {
				addAssignment (typeNames [node.EvaluationType], node.Location, id, node.Scope);
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
			createHelperFunctionDeclarations ();
			addEmptyLineToGlobalTranslation ();
			createFunctionDeclarations (programNode);
			addEmptyLineToGlobalTranslation ();
			createHelperFunctions ();
			addEmptyLineToGlobalTranslation ();
			createMainFunction (programName, programNode.Scope);
			addEmptyLineToGlobalTranslation ();
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

		public static string statement (string str)
		{
			return str + ';';
		}

		private void addAllocation (TokenType type, string times, string id, Scope scope)
		{
			if (!this.allocations.ContainsKey (scope)){
				this.allocations[scope] = new List<string> ();
			}

			this.allocations[scope].Add (id);

			bool declared = false;
			string tempLoc = nameFactory.GetTempVarId (scope, type, ref declared);
			addToScopeTranslation(statement (spaced (typeNames[TokenType.INTEGER_VAL], tempLoc)));
			addToScopeTranslation(statement (spaced (tempLoc, CTranslatorConstants.ASSIGNMENT, times, opStrings[TokenType.BINARY_OP_MUL], sizeOfString(typeNames[type]))));

			string typePointer = unspaced(typeNames[type], CTranslatorConstants.MEM_POINTER);
			string malloc = unspaced (CTranslatorConstants.MEM_ALLOCATION, CTranslatorConstants.CALL_TAIL_DELIMITERS.Item1, tempLoc, CTranslatorConstants.CALL_TAIL_DELIMITERS.Item2);
			string mallocCall = statement(spaced(typePointer, id, CTranslatorConstants.ASSIGNMENT, malloc));
			addToScopeTranslation (mallocCall);
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

		private void setNodeLocation(SyntaxTreeNode node, TokenType type, Property property, ref bool declared) {
			if (node.Location == null) {
				string location = nameFactory.GetTempVarId (node.Scope, type, ref declared);
				node.Location = location;

				if (!declaredVariables [currentScope].ContainsKey (location)) {
					declaredVariables [currentScope] [location] = property;
				}
			}
		}

		private void addVariableDeclarationsToScopeTranslation (Scope scope, Dictionary<string, Property> variables)
		{
			List<string> scopeDecalarations = new List<string> ();

			foreach (string key in variables.Keys) {
				Property prop = variables [key];
				TokenType type = prop.GetTokenType ();

				switch (type) {
				case TokenType.TYPE_ARRAY:
					ArrayProperty aProp = (ArrayProperty)prop;
					TokenType elementType = aProp.ArrayElementType;
					addToTranslation (scopeDecalarations, statement (spaced (typeNames [elementType] + CTranslatorConstants.MEM_POINTER, key)));
					break;
				default:
					addToTranslation (scopeDecalarations, statement (spaced (typeNames [type], key)));
					break;
				}
			}

			scopeTranslation = scopeDecalarations.Concat (scopeTranslation).ToList ();
		}
	}
}