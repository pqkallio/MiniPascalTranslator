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

		public ISemanticCheckValue VisitAssertNode(AssertNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitAssignNode(AssignNode node)
		{
			node.AssignValueExpression.Accept (this);
			addToTranslation (statement (spaced (nameFactory.GetCName (node.IDNode.ID), "=", node.AssignValueExpression.Location)));
			return null;
		}

		public ISemanticCheckValue VisitArrayAssignNode(ArrayAssignStatement node)
		{
			return null;
		}

		public ISemanticCheckValue VisitArrayAccessNode(ArrayAccessNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitDeclarationNode(DeclarationNode node)
		{
			if (node.DeclarationType.PropertyType == TokenType.TYPE_ARRAY) {
				node.DeclarationType.ArraySizeExpression.Accept (this);

				foreach (VariableIdNode idNode in node.IDsToDeclare) {
					string id = nameFactory.GetCName(idNode.ID);
					addAllocation(typeNames[node.DeclarationType.ArrayElementType], node.DeclarationType.Location, id, node.Scope);
				}
			} else {
				foreach (VariableIdNode idNode in node.IDsToDeclare) {
					addToTranslation (GetDeclarationType (node.DeclarationType) + " " + nameFactory.GetCName(idNode.ID) + ";");
				}
			}
			
			return null;
		}

		public ISemanticCheckValue VisitIntValueNode(IntValueNode node)
		{	
			setNodeLocation (node);

			addAssignment (typeNames[node.EvaluationType], node.Location, node.Value);

			return null;
		}

		public ISemanticCheckValue VisitRealValueNode(RealValueNode node)
		{
			setNodeLocation (node);

			addAssignment (typeNames[node.EvaluationType], node.Location, node.Value);

			return null;
		}

		public ISemanticCheckValue VisitBoolValueNode(BoolValueNode node)
		{
			setNodeLocation (node);

			string strVal = node.Value ? "1" : "0";

			addAssignment (typeNames[node.EvaluationType], node.Location, strVal);

			return null;
		}

		public ISemanticCheckValue VisitStringValueNode(StringValueNode node)
		{
			setNodeLocation (node);

			string strVal = unspaced (CTranslatorConstants.STRING_DELIMITER, node.Value, CTranslatorConstants.STRING_DELIMITER);

			addAssignment (typeNames[node.EvaluationType], node.Location, strVal);

			return null;
		}

		public ISemanticCheckValue VisitIOPrintNode(IOPrintNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitIOReadNode(IOReadNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitTypeNode(TypeNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitBlockNode(BlockNode node)
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
			return null;
		}

		public ISemanticCheckValue VisitBooleanNegation(BooleanNegation node)
		{
			return null;
		}

		public ISemanticCheckValue VisitExpressionNode(ExpressionNode node)
		{
			setNodeLocation (node);

			node.SimpleExpression.Location = node.Location;
			node.SimpleExpression.Accept (this);

			if (node.ExpressionTail != null) {
				node.ExpressionTail.SubTotal = node.SimpleExpression.Location;
				node.ExpressionTail.Accept (this);
			}

			return null;
		}

		public ISemanticCheckValue VisitSimpleExpression (SimpleExpression node)
		{
			setNodeLocation (node);

			node.Term.Location = node.Location;
			node.Term.Accept (this);

			if (node.AdditiveInverse) {
				addToTranslation (node.Location + " = -1 * " + node.Location + ";");
			}

			if (node.Tail != null) {
				node.Tail.SubTotal = node.Location;
				node.Tail.Accept (this);
			}

			return null;
		}

		public ISemanticCheckValue VisitSimpleExpressionTail (SimpleExpressionTail node)
		{
			setNodeLocation (node);

			node.Term.Location = node.Location;
			node.Term.Accept (this);

			addAssignment (typeNames[node.EvaluationType], node.SubTotal, node.SubTotal, opStrings [node.Operation], node.Location);

			if (node.Tail != null) {
				node.Tail.SubTotal = node.SubTotal;
				node.Tail.Accept (this);
			}

			return null;
		}

		public ISemanticCheckValue VisitExpressionTail(ExpressionTail node)
		{
			setNodeLocation (node);
			node.RightHandSide.Accept (this);
			addAssignment (typeNames[node.EvaluationType], node.Location, node.Location, opStrings[node.Operation], node.RightHandSide.Location);

			return null;
		}

		public ISemanticCheckValue VisitFactorNode(Factor node)
		{
			setNodeLocation (node);
			node.FactorMain.Location = node.Location;
			node.FactorMain.Accept (this);

			if (node.FactorTail != null) {
				// DO SUMTHIN!!!!
			}

			return null;
		}

		public ISemanticCheckValue VisitFactorMain(FactorMain node)
		{
			setNodeLocation (node);

			node.Evaluee.Location = node.Location;
			node.Evaluee.Accept (this);

			return null;
		}

		public ISemanticCheckValue VisitFactorTail(FactorTail node)
		{
			return null;
		}

		public ISemanticCheckValue VisitFunctionNode(FunctionNode node)
		{
			addToTranslation (createFunctionStart(nameFactory.GetCName(node.IDNode.ID), node.IDNode, nameFactory, node.Parameters.Parameters));
			node.Block.Accept (this);
			addEmptyLineToTranslation ();

			return null;
		}

		public ISemanticCheckValue VisitProcedureNode(ProcedureNode node)
		{
			VisitFunctionNode (node);

			return null;
		}

		public ISemanticCheckValue VisitFunctionCallNode(FunctionCallNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitIfNode(IfNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitWhileLoopNode(WhileNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitParametersNode(ParametersNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitArgumentsNode(ArgumentsNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitProgramNode(ProgramNode node)
		{
			createProgramStart (node);

			foreach (FunctionNode func in node.Functions.Values) {
				func.Accept (this);
			}

			return null;
		}

		public ISemanticCheckValue VisitReturnStatement(ReturnStatement node)
		{
			return null;
		}

		public ISemanticCheckValue VisitTermNode(TermNode node)
		{
			Factor factor = node.Factor;
			TermTail tail = node.TermTail;

			setNodeLocation (node);

			factor.Location = node.Location;
			factor.Accept (this);

			if (tail != null) {
				tail.SubTotal = factor.Location;
				tail.Accept (this);
			}
			return null;
		}

		public ISemanticCheckValue VisitTermTailNode(TermTail node)
		{
			setNodeLocation (node);
			Factor factor = node.Factor;
			TermTail tail = node.ChildTermTail;

			factor.Location = node.Location;
			factor.Accept (this);

			addAssignment (null, node.SubTotal, node.SubTotal, opStrings [node.Operation], factor.Location);

			if (tail != null) {
				tail.SubTotal = node.Location;
				tail.Accept (this);
			}
			return null;
		}

		public ISemanticCheckValue VisitVariableIdNode(VariableIdNode node)
		{
			return null;
		}

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
			createMainFunction (programName);
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

			createProgramFunctionDeclaration(programName);

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
			return createFunctionDeclaration (nameFactory.GetCName (functionName), idNode, nameFactory, parameters);
		}

		private void createProgramFunctionDeclaration(string programName)
		{
			addToTranslation ("int " + nameFactory.GetCName (programName) + "();");
		}

		private void createMainFunction (string programName)
		{
			addToTranslation ("int main()");
			addToTranslation (CTranslatorConstants.BLOCK_DELIMITERS.Item1);
			this.blockDepth++;
			addToTranslation ("return " + nameFactory.GetCName (programName) + "();");
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

			string parameterString = createParameters (nameFactory, parameters);

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

			string parameterString = createParameters (nameFactory, parameters);

			return spaced(returnType, functionName, parameterString);
		}

		private static string createParameters (CNameFactory nameFactory, List<Parameter> parameters = null)
		{
			StringBuilder sb = new StringBuilder (CTranslatorConstants.CALL_TAIL_DELIMITERS.Item1);

			if (parameters != null && parameters.Count > 0) {
				for (int i = 0; i < parameters.Count - 1; i++) {
					sb.Append (parameterToString (nameFactory, parameters [i]) + ", ");
				}

				sb.Append (parameterToString (nameFactory, parameters [parameters.Count - 1]));
			}

			sb.Append (CTranslatorConstants.CALL_TAIL_DELIMITERS.Item2);

			return sb.ToString ();
		}

		private static string parameterToString (CNameFactory nameFactory, Parameter param)
		{
			StringBuilder sb = new StringBuilder ("");

			if (paramTypeNames.ContainsKey (param.ParameterType)) {
				sb.Append (paramTypeNames [param.ParameterType]);
				if (param.Reference) {
					sb.Append ('*');
				}

				sb.Append (' ');
				sb.Append (nameFactory.GetCName (param.IdNode.ID));
			} else {
				sb.Append (typeNames [param.IdNode.ArrayElementType]);
				sb.Append ('*');

				sb.Append (' ');
				sb.Append (nameFactory.GetCName (param.IdNode.ID));
			}

			return sb.ToString ();
		}

		public static string statement (string str)
		{
			return str + ';';
		}

		private void addAllocation (string type, string times, string id, Scope scope)
		{
			if (!this.allocations.ContainsKey (scope)){
				this.allocations[scope] = new List<string> ();
			}

			this.allocations[scope].Add (id);

			string tempLoc = nameFactory.GetTempVarId ();
			addToTranslation(statement (spaced (typeNames[TokenType.INTEGER_VAL], tempLoc)));
			addToTranslation(statement (spaced (tempLoc, CTranslatorConstants.ASSIGNMENT, times, opStrings[TokenType.BINARY_OP_MUL], sizeOfString(type))));

			string typePointer = unspaced(type, CTranslatorConstants.MEM_POINTER);
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

		private void setNodeLocation(SyntaxTreeNode node) {
			if (node.Location == null) {
				node.Location = nameFactory.GetTempVarId ();
			}
		}
	}
}