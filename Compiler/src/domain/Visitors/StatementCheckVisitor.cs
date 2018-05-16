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
		private List<ReturnStatement> returnStatements;
		private TypeCheckingVisitor typeChecker;

		/// <summary>
		/// Initializes a new instance of the <see cref="Compiler.StatementCheckVisitor"/> class.
		/// </summary>
		/// <param name="analyzer">Analyzer.</param>
		public StatementCheckVisitor (SemanticAnalyzer analyzer)
		{
			this.analyzer = analyzer;
			this.returnStatements = new List<ReturnStatement> ();
			this.typeChecker = new TypeCheckingVisitor (analyzer);
		}

		public void VisitArraySizeCheckNode(ArraySizeCheckNode node)
		{
			
		}

		/// <summary>
		/// Checks the static semantic constraints of an AssignNode.
		/// </summary>
		/// <returns>An void.</returns>
		/// <param name="node">Node.</param>
		public void VisitAssignNode(AssignNode node)
		{
			node.IDNode.Accept (this);
			node.AssignValueExpression.Accept (this);

			TokenType varEval = node.IDNode.EvaluationType;
			TokenType exprEval = node.AssignValueExpression.EvaluationType;

			bool assignable = node.IDNode.EvaluationType != TokenType.ERROR;
			Property varProperty = node.Scope.GetProperty (node.IDNode.IDNode.ID);

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
		}

		/// <summary>
		/// Checks the static semantic constraints of a BinOpNode.
		/// </summary>
		/// 
		/// <returns>An void.</returns>
		/// <param name="node">Node.</param>
		public void VisitDeclarationNode(DeclarationNode node)
		{
			foreach (VariableIdNode idNode in node.IDsToDeclare) {
				idNode.Accept (this);
			}

			node.DeclarationType.Accept (this);
		}

		/// <summary>
		/// Checks the static semantic constraints of an IntValueNode.
		/// </summary>
		/// <returns>An void.</returns>
		/// <param name="node">Node.</param>
		public void VisitIntValueNode(IntValueNode node)
		{
			node.Accept (this.typeChecker);
			// This is not a statement so it needs not to be actually checked here.
			// So, we pass it to the TypeCheckerVisitor instead.
		}

		/// <summary>
		/// Checks the static semantic constraints of a BoolValueNode.
		/// </summary>
		/// <returns>An void.</returns>
		/// <param name="node">Node.</param>
		public void VisitBoolValueNode(BoolValueNode node)
		{
			// This is not a statement so it needs not to be actually checked here.
			// So, we pass it to the TypeCheckerVisitor instead.
			node.Accept (this.typeChecker);
			
		}

		/// <summary>
		/// Checks the static semantic constraints of a StringValueNode.
		/// </summary>
		/// <returns>An void.</returns>
		/// <param name="node">Node.</param>
		public void VisitStringValueNode(StringValueNode node)
		{
			// This is not a statement so it needs not to be actually checked here.
			// So, we pass it to the TypeCheckerVisitor instead.
			node.Accept (this.typeChecker);
			
		}

		/// <summary>
		/// Checks the static semantic constraints of a VariableIdNode.
		/// </summary>
		/// <returns>The variable identifier node.</returns>
		/// <param name="node">An void.</param>
		public void VisitVariableIdNode(VariableIdNode node)
		{
			node.Accept (this.typeChecker);
			Property prop = node.Scope.GetProperty (node.ID);

			if (prop.GetTokenType () == TokenType.ERROR) {
				analyzer.notifyError (new UninitializedVariableError (node));
				node.EvaluationType = TokenType.ERROR;
			}
		}

		/// <summary>
		/// Checks the static semantic constraints of an AssertNode.
		/// </summary>
		/// <returns>An void.</returns>
		/// <param name="node">Node.</param>
		public void VisitAssertNode(AssertNode node)
		{
			node.AssertExpression.Accept (this);

			TokenType exprEval = node.AssertExpression.EvaluationType;

			// check that the evaluation is a boolean value
			if (exprEval != TokenType.BOOLEAN_VAL) {
				analyzer.notifyError (new IllegalTypeError (node));
			}

			
		}

		/// <summary>
		/// Checks the static semantic constraints of an IOPrintNode.
		/// </summary>
		/// <returns>An void.</returns>
		/// <param name="node">Node.</param>
		public void VisitIOPrintNode(IOPrintNode node)
		{
			node.Arguments.Accept (this);
			// check the expression of this node
			
		}

		/// <summary>
		/// Checks the static semantic constraints of an IOReadNode.
		/// </summary>
		/// <returns>An void.</returns>
		/// <param name="node">Node.</param>
		public void VisitIOReadNode(IOReadNode node) 
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

			
		}

		public void VisitArrayAssignNode(ArrayAssignStatement node)
		{
			node.IndexExpression.Accept (this);
			node.AssignValueExpression.Accept (this);
			node.IDNode.Accept (this);

			Property prop = node.IDNode.Scope.GetProperty (node.IDNode.IDNode.ID);

			if (node.IDNode.Scope.GetProperty (node.IDNode.IDNode.ID).GetTokenType () != TokenType.TYPE_ARRAY) {
				analyzer.notifyError (new IllegalAssignmentError (node));
				
			}

			ArrayProperty arrProp = (ArrayProperty) prop;

			if (arrProp.ArrayElementType != node.AssignValueExpression.EvaluationType) {
				analyzer.notifyError (new IllegalTypeError (node));
			}

			
		}

		public void VisitArrayAccessNode(ArrayAccessNode node)
		{
			// NOT IN USE, AT LEAST NOT YET
			node.Accept (this.typeChecker);
			node.ArrayIdNode.Accept (this);
			node.ArrayIndexExpression.Accept (this);
			TokenType eh = node.EvaluationType;
			
		}

		public void VisitRealValueNode(RealValueNode node)
		{
			node.Accept (this.typeChecker);
			
		}

		public void VisitTypeNode(TypeNode node)
		{
			ExpressionNode sizeExpr = node.ArraySizeExpression;

			if (sizeExpr != null) {
				sizeExpr.Accept (this);

				if (sizeExpr.EvaluationType != TokenType.INTEGER_VAL) {
					analyzer.notifyError (new IllegalTypeError (sizeExpr));
				}
			}
			
			
		}

		public void VisitBlockNode(BlockNode node)
		{
			foreach (StatementNode statement in node.Statements) {
				CheckStatement (statement, node);
			}

			
		}

		public void VisitBooleanNegation(BooleanNegation node)
		{
			node.Accept (this.typeChecker);
			node.Factor.Accept (this);

			if (node.EvaluationType != TokenType.BOOLEAN_VAL) {
				analyzer.notifyError (new IllegalTypeError (node.Factor));
			}

			
		}

		public void VisitExpressionNode(ExpressionNode node)
		{
			node.Accept (this.typeChecker);
			node.SimpleExpression.Accept (this);

			if (node.ExpressionTail != null) {
				node.ExpressionTail.Accept (this);
				if (!LegitOperationChecker.IsLegitOperationForEvaluations (node.ExpressionTail.Operation, node.SimpleExpression.EvaluationType, node.ExpressionTail.RightHandSide.EvaluationType)) {
					node.EvaluationType = TokenType.ERROR;
				} else {
					node.EvaluationType = TokenType.BOOLEAN_VAL;
				}
			}

			if (node.EvaluationType == TokenType.ERROR) {
				analyzer.notifyError (new IllegalTypeError (node));
			}

			
		}

		public void VisitExpressionTail(ExpressionTail node)
		{
			node.Accept (this.typeChecker);
			node.RightHandSide.Accept (this);
		}

		public void VisitFactorNode(Factor node)
		{
			node.Accept (this.typeChecker);
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
			}
		}

		public void VisitFactorMain(FactorMain node)
		{
			node.Accept (this.typeChecker);
			node.Evaluee.Accept (this);
		}

		public void VisitFactorTail(FactorTail node)
		{
			node.Accept (this.typeChecker);
		}

		public void VisitFunctionNode(FunctionNode node)
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

			
		}

		public void VisitProcedureNode(ProcedureNode node)
		{
			node.Parameters.Accept (this);
			node.Block.Accept (this);

			CheckReturnTypes (node);

			
		}

		public void VisitFunctionCallNode(FunctionCallNode node)
		{
			node.Accept (this.typeChecker);
			VariableIdNode idNode = node.IdNode;
			ArgumentsNode arguments = node.ArgumentsNode;

			idNode.Accept (this);

			if (arguments != null) {
				arguments.Accept (this);

				FunctionNode function = analyzer.SyntaxTree.Root.Functions [idNode.ID];

				if (function == null) {
					analyzer.notifyError(new NotAValidFunctionError(idNode));
					
				}

				CompareParamsAndArgs (node, function.Parameters.Parameters, arguments.Arguments); 
			}

			
		}

		public void VisitIfNode(IfNode node)
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

			
		}

		public void VisitWhileLoopNode(WhileNode node)
		{
			ExpressionNode condition = node.Condition;
			StatementNode statement = node.Statement;

			condition.Accept (this);
			statement.Accept (this);

			if (condition.EvaluationType != TokenType.BOOLEAN_VAL) {
				analyzer.notifyError (new IllegalTypeError (condition));
			}

			
		}

		public void VisitParametersNode(ParametersNode node)
		{
			
		}

		public void VisitArgumentsNode(ArgumentsNode node)
		{
			List<ExpressionNode> arguments = node.Arguments;

			foreach (ExpressionNode arg in arguments) {
				arg.Accept (this);
			}

			
		}

		public void VisitProgramNode(ProgramNode node)
		{
			foreach (FunctionNode func in node.Functions.Values) {
				func.Accept (this);
			}

			node.MainBlock.Accept (this);

			
		}

		public void VisitReturnStatement(ReturnStatement node)
		{
			node.ReturnValue.Accept (this);

			returnStatements.Add (node);

			
		}

		public void VisitTermNode(TermNode node)
		{
			node.Accept (this.typeChecker);
			Factor factor = node.Factor;
			TermTail tail = node.TermTail;

			factor.Accept (this);

			if (tail != null) {
				tail.Accept (this);
			}

			if (node.EvaluationType == TokenType.ERROR) {
				analyzer.notifyError (new IllegalTypeError (node));
			}

			
		}

		public void VisitTermTailNode(TermTail node)
		{
			node.Accept (this.typeChecker);
			Factor factor = node.Factor;
			TermTail tail = node.ChildTermTail;

			factor.Accept (this);

			if (tail != null) {
				tail.Accept (this);
			}

			if (node.EvaluationType == TokenType.ERROR) {
				analyzer.notifyError (new IllegalTypeError (node));
			}

			
		}

		public void VisitSimpleExpression(SimpleExpression node)
		{
			node.Accept (this.typeChecker);
			node.Term.Accept (this);

			if (node.Tail != null) {
				node.Tail.Accept (this);
			}

			if (node.AdditiveInverse) {
				if (node.EvaluationType != TokenType.INTEGER_VAL && node.EvaluationType != TokenType.REAL_VAL) {
					analyzer.notifyError (new IllegalTypeError (node));
				}
			}
		}

		public void VisitSimpleExpressionTail(SimpleExpressionTail node)
		{
			node.Accept (this.typeChecker);
			node.Term.Accept (this);

		}

		private void CompareParamsAndArgs (FunctionCallNode callNode, List<Parameter> parameters, List<ExpressionNode> arguments) {
			if (parameters.Count != arguments.Count) {
				analyzer.notifyError (new InvalidArgumentCountError (callNode));
				return;
			}

			TokenType errorType = TokenType.ERROR;

			for (int i = 0; i < parameters.Count; i++) {
				if (parameters [i].Reference && !isAddressable(arguments[i])) {
					analyzer.notifyError (new InvalidArgumentError (callNode, i + 1));
				} else { TokenType paramEval = parameters [i].ParameterType;
					TokenType argEval = arguments [i].EvaluationType;

					if (paramEval != argEval || paramEval == errorType || argEval == errorType) {
						analyzer.notifyError (new InvalidArgumentError (callNode, i + 1, paramEval, argEval));
					}
				}
			}
		}

		private bool isAddressable(ExpressionNode expressionNode)
		{
			if (!expressionNode.SimpleExpression.Term.Factor.FactorMain.Evaluee.Variable) {
				return false;
			}

			if (expressionNode.SimpleExpression.Term.Factor.FactorTail != null) {
				return false;
			}

			if (expressionNode.SimpleExpression.Term.TermTail != null) {
				return false;
			}

			if (expressionNode.SimpleExpression.Tail != null) {
				return false;
			}

			if (expressionNode.SimpleExpression.AdditiveInverse) {
				return false;
			}

			if (expressionNode.ExpressionTail != null) {
				return false;
			}

			return true;
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

