using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
	public class CTranslationVisitor : INodeVisitor
	{
		private List<string> translation;
		private string programName;
		private CNameFactory nameFactory;
		private int blockDepth;
		private static Dictionary<TokenType, string> typeNames = CTranslatorConstants.SIMPLE_TYPE_NAMES;
		private static Dictionary<TokenType, string> paramTypeNames = CTranslatorConstants.PARAM_SIMPLE_TYPE_NAMES;
		private static Dictionary<TokenType, string> opStrings = CTranslatorConstants.OPERATION_STRINGS;
		private Dictionary<Scope, List<string>> allocations;

		public CTranslationVisitor (string programName, CNameFactory nameFactory)
		{
			this.translation = new List<string> ();
			this.programName = programName;
			this.nameFactory = nameFactory;
			this.blockDepth = 0;
			this.allocations = new Dictionary<Scope, List<string>> ();
		}

		public List<string> Translation
		{
			get { return this.translation; }
		}

		public void VisitAssertNode(AssertNode node)
		{}

		public void VisitArraySizeCheckNode(ArraySizeCheckNode node)
		{}

		public void VisitAssignNode(AssignNode node)
		{
			node.AssignValueExpression.Accept (this);
			addToTranslation (statement (spaced (nameFactory.GetCName (node.Scope, node.IDNode.IDNode.ID), "=", node.AssignValueExpression.Location)));
			
		}

		public void VisitArrayAssignNode(ArrayAssignStatement node)
		{}

		public void VisitArrayAccessNode(ArrayAccessNode node)
		{}

		public void VisitDeclarationNode(DeclarationNode node)
		{
			if (node.DeclarationType.PropertyType == TokenType.TYPE_ARRAY) {
				node.DeclarationType.ArraySizeExpression.Accept (this);

				foreach (VariableIdNode idNode in node.IDsToDeclare) {
					string id = nameFactory.GetCName(node.Scope, idNode.ID);
					addAllocation(node.DeclarationType.ArrayElementType, node.DeclarationType.Location, id, node.Scope);
				}
			} else {
				foreach (VariableIdNode idNode in node.IDsToDeclare) {
					addToTranslation (GetDeclarationType (node.DeclarationType) + " " + nameFactory.GetCName(node.Scope, idNode.ID) + ";");
				}
			}
			
			
		}

		public void VisitIntValueNode(IntValueNode node)
		{	
			setNodeLocation (node, node.EvaluationType);

			addAssignment (typeNames[node.EvaluationType], node.Location, node.Value);
		}

		public void VisitRealValueNode(RealValueNode node)
		{
			setNodeLocation (node, node.EvaluationType);

			addAssignment (typeNames[node.EvaluationType], node.Location, node.Value);
		}

		public void VisitBoolValueNode(BoolValueNode node)
		{
			setNodeLocation (node, node.EvaluationType);

			string strVal = node.Value ? "1" : "0";

			addAssignment (typeNames[node.EvaluationType], node.Location, strVal);
		}

		public void VisitStringValueNode(StringValueNode node)
		{
			setNodeLocation (node, node.EvaluationType);

			string strVal = unspaced (CTranslatorConstants.STRING_DELIMITER, node.Value, CTranslatorConstants.STRING_DELIMITER);

			addAssignment (typeNames[node.EvaluationType], node.Location, strVal);
		}

		public void VisitIOPrintNode(IOPrintNode node)
		{}

		public void VisitIOReadNode(IOReadNode node)
		{}

		public void VisitTypeNode(TypeNode node)
		{}

		public void VisitBlockNode(BlockNode node)
		{
			addToTranslation (CTranslatorConstants.BLOCK_DELIMITERS.Item1);
			blockDepth++;

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

			blockDepth--;
			addToTranslation (CTranslatorConstants.BLOCK_DELIMITERS.Item2);
		}

		public void VisitBooleanNegation(BooleanNegation node)
		{}

		public void VisitExpressionNode(ExpressionNode node)
		{
			setNodeLocation (node, node.EvaluationType);

			node.SimpleExpression.Location = node.Location;
			node.SimpleExpression.Accept (this);

			if (node.ExpressionTail != null) {
				node.ExpressionTail.SubTotal = node.SimpleExpression.Location;
				node.ExpressionTail.Accept (this);
			}
		}

		public void VisitSimpleExpression (SimpleExpression node)
		{
			setNodeLocation (node, node.EvaluationType);

			node.Term.Location = node.Location;
			node.Term.Accept (this);

			if (node.AdditiveInverse) {
				addToTranslation (node.Location + " = -1 * " + node.Location + ";");
			}

			if (node.Tail != null) {
				node.Tail.SubTotal = node.Location;
				node.Tail.Accept (this);
			}
		}

		public void VisitSimpleExpressionTail (SimpleExpressionTail node)
		{
			setNodeLocation (node, node.EvaluationType);

			node.Term.Location = node.Location;
			node.Term.Accept (this);

			addAssignment (typeNames[node.EvaluationType], node.SubTotal, node.SubTotal, opStrings [node.Operation], node.Location);

			if (node.Tail != null) {
				node.Tail.SubTotal = node.SubTotal;
				node.Tail.Accept (this);
			}
		}

		public void VisitExpressionTail(ExpressionTail node)
		{
			setNodeLocation (node, node.EvaluationType);
			node.RightHandSide.Accept (this);
			addAssignment (typeNames[node.EvaluationType], node.Location, node.Location, opStrings[node.Operation], node.RightHandSide.Location);
		}

		public void VisitFactorNode(Factor node)
		{
			setNodeLocation (node, node.EvaluationType);
			node.FactorMain.Location = node.Location;
			node.FactorMain.Accept (this);

			if (node.FactorTail != null) {
				// DO SUMTHIN!!!!
			}
		}

		public void VisitFactorMain(FactorMain node)
		{
			setNodeLocation (node, node.EvaluationType);

			node.Evaluee.Location = node.Location;
			node.Evaluee.Accept (this);
		}

		public void VisitFactorTail(FactorTail node)
		{}

		public void VisitFunctionNode(FunctionNode node)
		{
			addToTranslation (createFunctionStart(nameFactory.GetCName(node.Scope, node.IDNode.ID), node.IDNode, nameFactory, node.Parameters.Parameters));
			node.Block.Accept (this);
			addEmptyLineToTranslation ();
		}

		public void VisitProcedureNode(ProcedureNode node)
		{
			VisitFunctionNode (node);
		}

		public void VisitFunctionCallNode(FunctionCallNode node)
		{}

		public void VisitIfNode(IfNode node)
		{}

		public void VisitWhileLoopNode(WhileNode node)
		{}

		public void VisitParametersNode(ParametersNode node)
		{}

		public void VisitArgumentsNode(ArgumentsNode node)
		{}

		public void VisitProgramNode(ProgramNode node)
		{
			createProgramStart (node);

			foreach (FunctionNode func in node.Functions.Values) {
				func.Accept (this);
			}
		}

		public void VisitReturnStatement(ReturnStatement node)
		{}

		public void VisitTermNode(TermNode node)
		{
			Factor factor = node.Factor;
			TermTail tail = node.TermTail;

			setNodeLocation (node, node.EvaluationType);

			factor.Location = node.Location;
			factor.Accept (this);

			if (tail != null) {
				tail.SubTotal = factor.Location;
				tail.Accept (this);
			}
		}

		public void VisitTermTailNode(TermTail node)
		{
			setNodeLocation (node, node.EvaluationType);
			Factor factor = node.Factor;
			TermTail tail = node.ChildTermTail;

			factor.Location = node.Location;
			factor.Accept (this);

			addAssignment (null, node.SubTotal, node.SubTotal, opStrings [node.Operation], factor.Location);

			if (tail != null) {
				tail.SubTotal = node.Location;
				tail.Accept (this);
			}
		}

		public void VisitVariableIdNode(VariableIdNode node)
		{}

		private void createProgramStart (ProgramNode programNode)
		{
			includeLibraries ();
			addEmptyLineToTranslation ();
			createHelperFunctionDeclarations ();
			addEmptyLineToTranslation ();
			createFunctionDeclarations (programNode);
			addEmptyLineToTranslation ();
			createHelperFunctions ();
			addEmptyLineToTranslation ();
			createMainFunction (programName, programNode.Scope);
			addEmptyLineToTranslation ();
		}

		private void includeLibraries ()
		{
			foreach (string library in CTranslatorConstants.LIBRARIES) {
				addToTranslation (LibraryInclusion (library));
			}
		}
			
		private void createFunctionDeclarations (ProgramNode programNode)
		{
			addComment("program functions");

			createProgramFunctionDeclaration(programName, programNode.Scope);

			foreach (string func in programNode.Functions.Keys) {
				FunctionNode functionNode = programNode.Functions [func];
				addToTranslation (createFunctionDeclaration (func, functionNode.IDNode, functionNode.Parameters.Parameters));
			}
		}

		private void createHelperFunctionDeclarations ()
		{
			addComment("helper functions");

			foreach (string str in CTranslatorConstants.HELPER_FUNCTION_DECLARATIONS) {
				addToTranslation (statement (str));
			}
		}

		private void addComment(string line)
		{
			addToTranslation (spaced (CTranslatorConstants.COMMENT_DELIMITERS.Item1, line, CTranslatorConstants.COMMENT_DELIMITERS.Item2));
		}

		private void createHelperFunctions ()
		{
			foreach (string[] helperFunction in CTranslatorConstants.HELPER_FUNCTIONS) {
				foreach (string str in helperFunction) {
					addToTranslation (str);
				}

				addEmptyLineToTranslation ();
			}
		}

		private string createFunctionDeclaration (string functionName, VariableIdNode idNode, List<Parameter> parameters = null)
		{
			return createFunctionDeclaration (nameFactory.GetCName (idNode.Scope, functionName), idNode, nameFactory, parameters);
		}

		private void createProgramFunctionDeclaration(string programName, Scope scope)
		{
			addToTranslation ("int " + nameFactory.GetCName (scope, programName) + "();");
		}

		private void createMainFunction (string programName, Scope scope)
		{
			addToTranslation ("int main()");
			addToTranslation (CTranslatorConstants.BLOCK_DELIMITERS.Item1);
			this.blockDepth++;
			addToTranslation ("return " + nameFactory.GetCName (scope, programName) + "();");
			this.blockDepth--;
			addToTranslation (CTranslatorConstants.BLOCK_DELIMITERS.Item2);
		}

		private void addEmptyLineToTranslation () {
			addToTranslation ("");
		}

		private void addToTranslation(string line) {
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
			addToTranslation(statement (spaced (typeNames[TokenType.INTEGER_VAL], tempLoc)));
			addToTranslation(statement (spaced (tempLoc, CTranslatorConstants.ASSIGNMENT, times, opStrings[TokenType.BINARY_OP_MUL], sizeOfString(typeNames[type]))));

			string typePointer = unspaced(typeNames[type], CTranslatorConstants.MEM_POINTER);
			string malloc = unspaced (CTranslatorConstants.MEM_ALLOCATION, CTranslatorConstants.CALL_TAIL_DELIMITERS.Item1, tempLoc, CTranslatorConstants.CALL_TAIL_DELIMITERS.Item2);
			string mallocCall = statement(spaced(typePointer, id, CTranslatorConstants.ASSIGNMENT, malloc));
			addToTranslation (mallocCall);
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
			addToTranslation(statement (unspaced (CTranslatorConstants.MEM_RELEASE, CTranslatorConstants.CALL_TAIL_DELIMITERS.Item1, id, CTranslatorConstants.CALL_TAIL_DELIMITERS.Item2)));
		}

		private string sizeOfString(string type)
		{
			string leftDelimiter = CTranslatorConstants.CALL_TAIL_DELIMITERS.Item1;
			string rightDelimiter = CTranslatorConstants.CALL_TAIL_DELIMITERS.Item2;

			return unspaced (CTranslatorConstants.SIZE_OF, leftDelimiter, type, rightDelimiter);
		}

		private void addAssignment(string type, string target, string firstOperand, string operation = null, string secondOperand = null)
		{
			string assignment = CTranslatorConstants.ASSIGNMENT;

			if (operation != null) {
				addToTranslation (statement (spaced (type, target, assignment, firstOperand, operation, secondOperand)));
			} else {
				addToTranslation (statement (spaced (type, target, assignment, firstOperand)));
			}
		}

		private void setNodeLocation(SyntaxTreeNode node, TokenType type) {
			if (node.Location == null) {
				bool declared = false;
				node.Location = nameFactory.GetTempVarId (node.Scope, type, ref declared);
			}
		}
	}
}