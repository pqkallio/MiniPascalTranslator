using System;
using System.Text;
using System.Collections.Generic;

namespace Compiler
{
	public class CTranslationVisitor : INodeVisitor
	{
		private List<string> translation;
		private string programName;
		private CNameFactory nameFactory;
		private int blockDepth;

		public CTranslationVisitor (string programName, CNameFactory nameFactory)
		{
			this.translation = new List<string> ();
			this.programName = programName;
			this.nameFactory = nameFactory;
			this.blockDepth = 0;
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
					addToTranslation (CTranslatorConstants.SIMPLE_TYPE_NAMES[node.DeclarationType.ArrayElementType] + "[" + node.DeclarationType.Location + "] " +  nameFactory.GetCName(idNode.ID) + ";");
				}
			} else {
				foreach (VariableIdNode idNode in node.IDsToDeclare) {
					addToTranslation (CTranslatorConstants.GetDeclarationType (node.DeclarationType) + " " + nameFactory.GetCName(idNode.ID) + ";");
				}
			}
			
			return null;
		}

		public ISemanticCheckValue VisitIntValueNode(IntValueNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitRealValueNode(RealValueNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitBoolValueNode(BoolValueNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitStringValueNode(StringValueNode node)
		{
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

			foreach (StatementNode statement in node.Statements) {
				statement.Accept (this);
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
			if (node.Location == null) {
				node.Location = nameFactory.GetTempVarId ();
			}

			node.SimpleExpression.SubTotal = node.Location;
			node.SimpleExpression.Accept (this);

			if (node.ExpressionTail != null) {
				node.ExpressionTail.SubTotal = node.SimpleExpression.Location;
				node.ExpressionTail.Accept (this);
			}

			return null;
		}

		public ISemanticCheckValue VisitExpressionTail(ExpressionTail node)
		{
			return null;
		}

		public ISemanticCheckValue VisitFactorNode(Factor node)
		{
			return null;
		}

		public ISemanticCheckValue VisitFactorMain(FactorMain node)
		{
			return null;
		}

		public ISemanticCheckValue VisitFactorTail(FactorTail node)
		{
			return null;
		}

		public ISemanticCheckValue VisitFunctionNode(FunctionNode node)
		{
			addToTranslation (CTranslatorConstants.createFunctionStart(nameFactory.GetCName(node.IDNode.ID), node.IDNode, nameFactory, node.Parameters.Parameters));
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
			return null;
		}

		public ISemanticCheckValue VisitTermTailNode(TermTail node)
		{
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
			createFunctionDeclarations (programNode);
			addEmptyLineToTranslation ();
			createMainFunction (programName);
			addEmptyLineToTranslation ();
		}

		private void includeLibraries ()
		{
			foreach (string library in CTranslatorConstants.LIBRARIES) {
				addToTranslation (CTranslatorConstants.LibraryInclusion (library));
			}
		}

		private void createFunctionDeclarations (ProgramNode programNode)
		{
			createProgramFunctionDeclaration(programName);

			foreach (string func in programNode.Functions.Keys) {
				FunctionNode functionNode = programNode.Functions [func];
				addToTranslation (createFunctionDeclaration (func, functionNode.IDNode, functionNode.Parameters.Parameters));
			}
		}

		private string createFunctionDeclaration (string functionName, VariableIdNode idNode, List<Parameter> parameters = null)
		{
			return CTranslatorConstants.createFunctionDeclaration (nameFactory.GetCName (functionName), idNode, nameFactory, parameters);
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
	}
}