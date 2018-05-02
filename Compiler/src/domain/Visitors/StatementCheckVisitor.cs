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
		private VoidProperty voidProperty;
		private List<ReturnStatement> returnStatements;

		/// <summary>
		/// Initializes a new instance of the <see cref="Compiler.StatementCheckVisitor"/> class.
		/// </summary>
		/// <param name="analyzer">Analyzer.</param>
		public StatementCheckVisitor (SemanticAnalyzer analyzer)
		{
			this.analyzer = analyzer;
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
			node.IDNode.Accept (this);
			node.AssignValueExpression.Accept (this);

			TokenType varEval = node.IDNode.EvaluationType;
			TokenType exprEval = node.AssignValueExpression.EvaluationType;

			bool assignable = node.IDNode.EvaluationType != TokenType.ERROR;
			Property varProperty = node.Scope.GetProperty (node.IDNode.ID);

			if (varProperty.DeclarationRow > node.Token.Row) {
				analyzer.notifyError(new UndeclaredVariableError(node.IDNode));
				assignable = false;
			}

			if (!LegitOperationChecker.isAssignCompatible (varEval, exprEval)) {
				analyzer.notifyError(new IllegalAssignmentError(node));
				assignable = false;
			}

			if (assignable) {
				varProperty.Assigned = true;
			}

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
			foreach (VariableIdNode idNode in node.IDsToDeclare) {
				idNode.Accept (this);
			}

			node.DeclarationType.Accept (this);

			return voidProperty;
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
			return voidProperty;
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
			return voidProperty;
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
			return voidProperty;
		}

		/// <summary>
		/// Checks the static semantic constraints of a VariableIdNode.
		/// </summary>
		/// <returns>The variable identifier node.</returns>
		/// <param name="node">An ISemanticCheckValue.</param>
		public ISemanticCheckValue VisitVariableIdNode(VariableIdNode node)
		{
			Property prop = node.Scope.GetProperty (node.ID);

			if (prop.GetTokenType () == TokenType.ERROR) {
				analyzer.notifyError (new UninitializedVariableError (node));
				node.SetEvaluationType (TokenType.ERROR);
				return voidProperty;
			}

			if (node.ArrayIndex != null) {
				if (prop.GetTokenType () != TokenType.TYPE_ARRAY) {
					analyzer.notifyError (new IllegalArrayAccessError (node));
				}

				TokenType arrayIndexEval = node.ArrayIndex.EvaluationType;

				if (arrayIndexEval != TokenType.INTEGER_VAL) {
					analyzer.notifyError (new IllegalArrayIndexError (node));
				}
			}

			return voidProperty;
		}

		/// <summary>
		/// Checks the static semantic constraints of an AssertNode.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitAssertNode(AssertNode node)
		{
			node.AssertExpression.Accept (this);

			TokenType exprEval = node.AssertExpression.EvaluationType;

			// check that the evaluation is a boolean value
			if (exprEval != TokenType.BOOLEAN_VAL) {
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
			node.Arguments.Accept (this);
			// check the expression of this node
			return voidProperty;
		}

		/// <summary>
		/// Checks the static semantic constraints of an IOReadNode.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitIOReadNode(IOReadNode node) 
		{
			foreach (VariableIdNode idNode in node.IDNodes) {
				idNode.Accept (this);
				TokenType type = node.Scope.GetProperty (idNode.ID).GetTokenType ();

				if (type == TokenType.ERROR) {
					analyzer.notifyError (new UninitializedVariableError (node));
				} else if (type == TokenType.TYPE_ARRAY || type == TokenType.BOOLEAN_VAL) {
					analyzer.notifyError (new IllegalTypeError (idNode));
				}
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitArrayAssignNode(ArrayAssignStatement node)
		{
			node.IndexExpression.Accept (this);
			node.AssignValueExpression.Accept (this);
			node.IDNode.Accept (this);

			Property prop = node.IDNode.Scope.GetProperty (node.IDNode.ID);

			if (node.IDNode.Scope.GetProperty (node.IDNode.ID).GetTokenType () != TokenType.TYPE_ARRAY) {
				analyzer.notifyError (new IllegalAssignmentError (node));
				return voidProperty;
			}

			ArrayProperty arrProp = (ArrayProperty) prop;

			if (arrProp.ArrayElementType != node.AssignValueExpression.EvaluationType) {
				analyzer.notifyError (new IllegalTypeError (node));
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitArrayAccessNode(ArrayAccessNode node)
		{
			// NOT IN USE, AT LEAST NOT YET
			return voidProperty;
		}

		public ISemanticCheckValue VisitRealValueNode(RealValueNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitTypeNode(TypeNode node)
		{
			ExpressionNode sizeExpr = node.ArraySizeExpression;

			if (sizeExpr != null) {
				sizeExpr.Accept (this);

				if (sizeExpr.EvaluationType != TokenType.INTEGER_VAL) {
					analyzer.notifyError (new IllegalTypeError (sizeExpr));
				}
			}
			
			return voidProperty;
		}

		public ISemanticCheckValue VisitBlockNode(BlockNode node)
		{
			foreach (StatementNode statement in node.Statements) {
				CheckStatement (statement, node);
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitBooleanNegation(BooleanNegation node)
		{
			node.Factor.Accept (this);

			if (node.EvaluationType != TokenType.BOOLEAN_VAL) {
				analyzer.notifyError (new IllegalTypeError (node.Factor));
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitExpressionNode(ExpressionNode node)
		{
			node.SimpleExpression.Accept (this);

			if (node.ExpressionTail != null) {
				node.ExpressionTail.Accept (this);
			}

			if (node.EvaluationType == TokenType.ERROR) {
				analyzer.notifyError (new IllegalTypeError (node));
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitExpressionTail(ExpressionTail node)
		{
			node.RightHandSide.Accept (this);

			return voidProperty;
		}

		public ISemanticCheckValue VisitFactorNode(Factor node)
		{
			FactorMain main = node.FactorMain;
			FactorTail tail = node.FactorTail;

			main.Accept (this);

			if (tail != null) {
				tail.Accept (this);

				if (tail.Token.Type == TokenType.SIZE && main.Variable) {
					VariableIdNode idNode = (VariableIdNode) main.Evaluee;

					TokenType idEval = idNode.Scope.GetProperty (idNode.ID).GetTokenType ();

					if (idEval != TokenType.TYPE_ARRAY) {
						analyzer.notifyError (new IllegalTypeError (tail));
					}
				}
			} else if (main.EvaluationType == TokenType.TYPE_ARRAY) {
				analyzer.notifyError(new IllegalTypeError (main));
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitFactorMain(FactorMain node)
		{
			node.Evaluee.Accept (this);

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

			if (returnStatements.Count == 0) {
				analyzer.notifyError (new FunctionDoesntReturnError (node));
			}

			CheckReturnTypes (node);

			int blockStatementCount = node.Block.Statements.Count;

			if (blockStatementCount == 0 
				|| !node.Block.Statements[blockStatementCount - 1].Returns) {
				analyzer.notifyError(new AllCodePathsDontReturnError (node));
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitProcedureNode(ProcedureNode node)
		{
			node.Parameters.Accept (this);
			node.Block.Accept (this);

			CheckReturnTypes (node);

			return voidProperty;
		}

		public ISemanticCheckValue VisitFunctionCallNode(FunctionCallNode node)
		{
			VariableIdNode idNode = node.IdNode;
			ArgumentsNode arguments = node.ArgumentsNode;

			idNode.Accept (this);

			if (arguments != null) {
				arguments.Accept (this);

				FunctionNode function = analyzer.SyntaxTree.Root.Functions [idNode.ID];

				if (function == null) {
					analyzer.notifyError(new NotAValidFunctionError(idNode));
					return voidProperty;
				}

				CompareParamsAndArgs (node, function.Parameters.Parameters, arguments.Arguments); 
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitIfNode(IfNode node)
		{
			ExpressionNode condition = node.Condition;
			StatementNode ifBranch = node.IfBranch;
			StatementNode elseBranch = node.ElseBranch;

			condition.Accept (this);
			ifBranch.Accept (this);

			if (condition.EvaluationType != TokenType.BOOLEAN_VAL) {
				analyzer.notifyError (new IllegalTypeError (condition));
			}

			if (elseBranch != null) {
				elseBranch.Accept (this);

				if (ifBranch.Returns && elseBranch.Returns) {
					node.Returns = true;
				}
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitWhileLoopNode(WhileNode node)
		{
			ExpressionNode condition = node.Condition;
			StatementNode statement = node.Statement;

			condition.Accept (this);
			statement.Accept (this);

			if (condition.EvaluationType != TokenType.BOOLEAN_VAL) {
				analyzer.notifyError (new IllegalTypeError (condition));
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitParametersNode(ParametersNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitArgumentsNode(ArgumentsNode node)
		{
			List<ExpressionNode> arguments = node.Arguments;

			foreach (ExpressionNode arg in arguments) {
				arg.Accept (this);
			}

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
			node.ReturnValue.Accept (this);

			returnStatements.Add (node);

			return voidProperty;
		}

		public ISemanticCheckValue VisitTermNode(TermNode node)
		{
			Factor factor = node.Factor;
			TermTail tail = node.TermTail;

			factor.Accept (this);

			if (tail != null) {
				tail.Accept (this);
			}

			if (node.EvaluationType == TokenType.ERROR) {
				analyzer.notifyError (new IllegalTypeError (node));
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitTermTailNode(TermTail node)
		{
			Factor factor = node.Factor;
			TermTail tail = node.ChildTermTail;

			factor.Accept (this);

			if (tail != null) {
				tail.Accept (this);
			}

			if (node.EvaluationType == TokenType.ERROR) {
				analyzer.notifyError (new IllegalTypeError (node));
			}

			return voidProperty;
		}

		private void CompareParamsAndArgs (FunctionCallNode callNode, List<Parameter> parameters, List<ExpressionNode> arguments) {
			if (parameters.Count != arguments.Count) {
				analyzer.notifyError (new InvalidArgumentCountError (callNode));
				return;
			}

			TokenType errorType = TokenType.ERROR;

			for (int i = 0; i < parameters.Count; i++) {
				if (parameters [i].Reference && !arguments[i].Variable) {
					analyzer.notifyError (new InvalidArgumentError (callNode, i + 1));
				} else { TokenType paramEval = parameters [i].ParameterType;
					TokenType argEval = arguments [i].EvaluationType;

					if (paramEval != argEval || paramEval == errorType || argEval == errorType) {
						analyzer.notifyError (new InvalidArgumentError (callNode, i + 1, paramEval, argEval));
					}
				}
			}
		}

		private void CheckStatement (StatementNode statement, StatementNode parentNode = null, bool parentCanReturn = true)
		{
			statement.Accept (this);

			if (parentNode != null && statement.Returns && parentCanReturn) {
				parentNode.Returns = true;
			}
		}

		private void CheckReturnTypes (FunctionNode function)
		{
			foreach (ReturnStatement returnStatement in returnStatements) {
				if (returnStatement.EvaluationType != function.ReturnType) {
					analyzer.notifyError(new InvalidReturnValueError (returnStatement, function.ReturnType));
				}
			}

			returnStatements.Clear ();
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

