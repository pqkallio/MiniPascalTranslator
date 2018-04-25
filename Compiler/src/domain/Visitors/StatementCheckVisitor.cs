using System;
using System.Collections.Generic;

namespace Compiler
{
	/// <summary>
	/// Performs a depth first static semantic checking to an ISyntaxTreeNode 
	/// </summary>
	public class StatementCheckVisitor : INodeVisitor
	{
		private SemanticAnalyzer analyzer;			// the parent analyzer to notify the errors to
		private TypeCheckingVisitor typeChecker;	// checks a given nodes evaluation type
		private VoidProperty voidProperty;
		private List<ReturnStatement> returnStatements;

		/// <summary>
		/// Initializes a new instance of the <see cref="Compiler.StatementCheckVisitor"/> class.
		/// </summary>
		/// <param name="analyzer">Analyzer.</param>
		public StatementCheckVisitor (SemanticAnalyzer analyzer)
		{
			this.analyzer = analyzer;
			this.typeChecker = new TypeCheckingVisitor (analyzer);
			this.returnStatements = new List<ReturnStatement> ();

			// We tend to return a lot of VoidProperties here,
			// but since we don't do anything with them, we
			// use one global VoidProperty object and pass it
			// anytime we need to pass one.
			this.voidProperty = new VoidProperty ();
		}

		/// <summary>
		/// Checks the static semantic constraints of an AssignNode.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitAssignNode(AssignNode node)
		{
			return voidProperty;
		}

		/// <summary>
		/// Checks the static semantic constraints of a BinOpNode.
		/// </summary>
		/// 
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitDeclarationNode(DeclarationNode node)
		{
			return null;
		}

		/// <summary>
		/// Checks the static semantic constraints of an IntValueNode.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitIntValueNode(IntValueNode node)
		{
			// This is not a statement so it needs not to be actually checked here.
			// So, we pass it to the TypeCheckerVisitor instead.
			return node.Accept(this.typeChecker);
		}

		/// <summary>
		/// Checks the static semantic constraints of a BoolValueNode.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitBoolValueNode(BoolValueNode node)
		{
			// This is not a statement so it needs not to be actually checked here.
			// So, we pass it to the TypeCheckerVisitor instead.
			return node.Accept(this.typeChecker);
		}

		/// <summary>
		/// Checks the static semantic constraints of a StringValueNode.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitStringValueNode(StringValueNode node)
		{
			// This is not a statement so it needs not to be actually checked here.
			// So, we pass it to the TypeCheckerVisitor instead.
			return node.Accept(this.typeChecker);
		}

		/// <summary>
		/// Checks the static semantic constraints of a VariableIdNode.
		/// </summary>
		/// <returns>The variable identifier node.</returns>
		/// <param name="node">An ISemanticCheckValue.</param>
		public ISemanticCheckValue VisitVariableIdNode(VariableIdNode node)
		{
			// This is not a statement so it needs not to be actually checked here.
			// So, we pass it to the TypeCheckerVisitor instead.
			return voidProperty;
		}

		/// <summary>
		/// Checks the static semantic constraints of an AssertNode.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitAssertNode(AssertNode node)
		{
			// get the evaluation of this node
			Property evaluation = node.Accept(this.typeChecker).asProperty();

			// check that the evaluation is a boolean value
			if (!checkPropertyType(evaluation, TokenType.BOOLEAN_VAL_FALSE)) {
				analyzer.notifyError (new IllegalTypeError (node));
			}

			return voidProperty;
		}

		/// <summary>
		/// Checks the static semantic constraints of an IOPrintNode.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitIOPrintNode(IOPrintNode node)
		{
			// check the expression of this node
			return null;
		}

		/// <summary>
		/// Checks the static semantic constraints of an IOReadNode.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitIOReadNode(IOReadNode node) 
		{
			return null;
		}

		public ISemanticCheckValue VisitArrayAssignNode(ArrayAssignStatement node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitArrayAccessNode(ArrayAccessNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitRealValueNode(RealValueNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitTypeNode(TypeNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitBlockNode(BlockNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitBooleanNegation(BooleanNegation node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitExpressionNode(ExpressionNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitExpressionTail(ExpressionTail node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitFactorNode(Factor node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitFactorMain(FactorMain node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitFactorTail(FactorTail node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitFunctionNode(FunctionNode node)
		{
			node.Parameters.Accept (this);
			node.Block.Accept (this);

			foreach (ReturnStatement returnStatement in returnStatements) {
				if (returnStatement.EvaluationType != node.ReturnType) {
					analyzer.notifyError(new InvalidReturnValueError (returnStatement, node.ReturnType));
				}
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitFunctionCallNode(FunctionCallNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitIfNode(IfNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitWhileLoopNode(WhileNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitParametersNode(ParametersNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitArgumentsNode(ArgumentsNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitProgramNode(ProgramNode node)
		{
			foreach (FunctionNode func in node.Functions.Values) {
				func.Accept (this);
			}

			node.MainBlock.Accept (this);

			return voidProperty;
		}

		public ISemanticCheckValue VisitReturnStatement(ReturnStatement node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitTermNode(TermNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitTermTailNode(TermTail node)
		{
			return voidProperty;
		}

		/// <summary>
		/// A private helper method to check a property's evaluation type. 
		/// </summary>
		/// <returns><c>true</c>, if property's type matches the expectation, <c>false</c> otherwise.</returns>
		/// <param name="property">Property.</param>
		/// <param name="expected">Expected type.</param>
		private bool checkPropertyType (Property property, TokenType expected)
		{
			TokenType tokenType = property.GetTokenType ();

			// if the type is error, it will be handle by the calling method,
			// so it's ok
			return tokenType == TokenType.ERROR || tokenType == expected;
		}
	}
}

