using System;

namespace Compiler
{
	public class TypeCheckingVisitor : INodeVisitor
	{
		private SemanticAnalyzer analyzer;

		public TypeCheckingVisitor (SemanticAnalyzer analyzer)
		{
			this.analyzer = analyzer;
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
			if (node.HasAlreadyBeenEvaluated) {
				return null;
			}

			Property prop = node.Scope.GetProperty (node.ArrayIdNode.ID);

			if (prop.GetTokenType () != TokenType.TYPE_ARRAY) {
				node.EvaluationType = TokenType.ERROR;
			} else {
				ArrayProperty arrayProp = (ArrayProperty)prop;
				node.EvaluationType = arrayProp.ArrayElementType;
			}

			return null;
		}

		public ISemanticCheckValue VisitDeclarationNode(DeclarationNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitIntValueNode(IntValueNode node)
		{
			node.EvaluationType = TokenType.INTEGER_VAL;

			return null;
		}

		public ISemanticCheckValue VisitRealValueNode(RealValueNode node)
		{
			node.EvaluationType = TokenType.REAL_VAL;

			return null;
		}

		public ISemanticCheckValue VisitBoolValueNode(BoolValueNode node)
		{
			node.EvaluationType = TokenType.BOOLEAN_VAL;

			return null;
		}

		public ISemanticCheckValue VisitStringValueNode(StringValueNode node)
		{
			node.EvaluationType = TokenType.STRING_VAL;

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
			return null;
		}

		public ISemanticCheckValue VisitBooleanNegation(BooleanNegation node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return null;
			}
			node.Factor.Accept (this);
			TokenType factorEval = node.Factor.EvaluationType;

			node.EvaluationType = factorEval != TokenType.BOOLEAN_VAL ? factorEval : TokenType.ERROR;

			return null;
		}

		public ISemanticCheckValue VisitExpressionNode(ExpressionNode node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return null;
			}

			node.SimpleExpression.Accept (this);
			TokenType expressionEvaluation = node.SimpleExpression.EvaluationType;

			if (node.ExpressionTail != null) {
				node.ExpressionTail.Accept (this);
				TokenType tailEvaluation = node.ExpressionTail.RightHandSide.EvaluationType;

				if (!LegitOperationChecker.IsLegitOperationForEvaluations(node.ExpressionTail.Operation, expressionEvaluation, tailEvaluation)) {
					expressionEvaluation = TokenType.ERROR;
				}
			}

			node.EvaluationType = expressionEvaluation;

			return null;
		}

		public ISemanticCheckValue VisitExpressionTail(ExpressionTail node)
		{
			node.RightHandSide.Accept (this);

			node.EvaluationType = TokenType.BOOLEAN_VAL;

			return null;
		}

		public ISemanticCheckValue VisitFactorNode(Factor node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return null;
			}

			node.FactorMain.Accept (this);

			if (node.FactorTail != null) {
				if (node.FactorTail.Token.Type == TokenType.SIZE && node.FactorMain.EvaluationType == TokenType.TYPE_ARRAY) {
					node.EvaluationType = TokenType.INTEGER_VAL;
				} else {
					node.EvaluationType = TokenType.ERROR;
				}
			} else {
				node.EvaluationType = node.FactorMain.EvaluationType;
			}

			return null;
		}

		public ISemanticCheckValue VisitFactorMain(FactorMain node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return null;
			}

			node.Evaluee.Accept (this);
			node.EvaluationType = node.Evaluee.EvaluationType;

			return null;
		}

		public ISemanticCheckValue VisitFactorTail(FactorTail node)
		{
			return null;
		}

		public ISemanticCheckValue VisitFunctionNode(FunctionNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitProcedureNode(ProcedureNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitFunctionCallNode(FunctionCallNode node)
		{
			node.IdNode.Accept (this);

			node.EvaluationType = node.IdNode.EvaluationType;

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
			return null;
		}

		public ISemanticCheckValue VisitReturnStatement(ReturnStatement node)
		{
			return null;
		}

		public ISemanticCheckValue VisitTermNode(TermNode node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return null;
			}
			node.Factor.Accept (this);
			TokenType factorEval = node.Factor.EvaluationType;

			if (node.TermTail != null) {
				node.TermTail.Accept (this);
				TokenType tailEval = node.TermTail.EvaluationType;

				if (!LegitOperationChecker.IsLegitOperationForEvaluations(node.TermTail.Operation, factorEval, tailEval)) {
					factorEval = TokenType.ERROR;
				}

				// MUST HANDLE CASES WHERE INT + REAL EVALUATED
			}

			node.EvaluationType = factorEval;

			return null;
		}

		public ISemanticCheckValue VisitTermTailNode(TermTail node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return null;
			}

			node.Factor.Accept (this);
			TokenType factorEval = node.Factor.EvaluationType;

			if (node.ChildTermTail != null) {
				node.ChildTermTail.Accept (this);
				TokenType tailEval = node.ChildTermTail.EvaluationType;

				if (!LegitOperationChecker.IsLegitOperationForEvaluations (node.ChildTermTail.Operation, factorEval, tailEval)) {
					factorEval = TokenType.ERROR;
				}
			}

			node.EvaluationType = factorEval;

			return null;
		}

		public ISemanticCheckValue VisitVariableIdNode(VariableIdNode node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return null;
			}

			Property prop = node.Scope.GetProperty (node.ID);

			if (node.ArrayRequestSize) {
				node.EvaluationType = TokenType.INTEGER_VAL;
			} else {
				node.EvaluationType = prop.GetTokenType ();
			}
				
			return null;
		}

		public ISemanticCheckValue VisitSimpleExpression(SimpleExpression node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return null;
			}

			node.Term.Accept (this);
			TokenType termEval = node.Term.EvaluationType;

			if (node.Tail != null) {
				node.Tail.Accept (this);
				TokenType tailEval = node.Tail.EvaluationType;

				if (!LegitOperationChecker.IsLegitOperationForEvaluations (node.Tail.Operation, termEval, tailEval)) {
					termEval = TokenType.ERROR;
				}
			}

			if (node.AdditiveInverse && 
				!LegitOperationChecker.IsLegitOperationForEvaluations(TokenType.UNARY_OP_NEGATIVE, termEval)) {
				termEval = TokenType.ERROR;
			}

			node.EvaluationType = termEval;

			return null;
		}

		public ISemanticCheckValue VisitSimpleExpressionTail(SimpleExpressionTail node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return null;
			}

			node.Term.Accept (this);
			TokenType termEval = node.Term.EvaluationType;

			if (node.Tail != null) {
				node.Tail.Accept (this);
				TokenType tailEval = node.Tail.EvaluationType;

				if (!LegitOperationChecker.IsLegitOperationForEvaluations(node.Tail.Operation, termEval, tailEval)) {
					termEval = TokenType.ERROR;
				}
			}

			node.EvaluationType = termEval;

			return null;
		}
	}
}