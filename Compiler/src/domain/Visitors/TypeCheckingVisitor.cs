using System;

namespace Compiler
{
	public class TypeCheckingVisitor : INodeVisitor
	{
		public void VisitAssertNode(AssertNode node)
		{}

		public void VisitArraySizeCheckNode(ArraySizeCheckNode node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return;
			}

			Property prop = node.Scope.GetProperty (node.VariableID);

			if (prop.GetTokenType () != TokenType.TYPE_ARRAY) {
				node.EvaluationType = TokenType.ERROR;
			} else {
				node.EvaluationType = TokenType.INTEGER_VAL;
			}
		}

		public void VisitAssignNode(AssignNode node)
		{}

		public void VisitArrayAssignNode(ArrayAssignStatement node)
		{}

		public void VisitArrayAccessNode(ArrayAccessNode node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return;
			}

			Property prop = node.Scope.GetProperty (node.ArrayIdNode.ID);

			if (prop.GetTokenType () != TokenType.TYPE_ARRAY) {
				node.EvaluationType = TokenType.ERROR;
			} else {
				ArrayProperty arrayProp = (ArrayProperty)prop;
				node.EvaluationType = arrayProp.ArrayElementType;
			}
		}

		public void VisitDeclarationNode(DeclarationNode node)
		{}

		public void VisitIntValueNode(IntValueNode node)
		{
			node.EvaluationType = TokenType.INTEGER_VAL;
		}

		public void VisitRealValueNode(RealValueNode node)
		{
			node.EvaluationType = TokenType.REAL_VAL;
		}

		public void VisitBoolValueNode(BoolValueNode node)
		{
			node.EvaluationType = TokenType.BOOLEAN_VAL;
		}

		public void VisitStringValueNode(StringValueNode node)
		{
			node.EvaluationType = TokenType.STRING_VAL;
		}

		public void VisitIOPrintNode(IOPrintNode node)
		{}

		public void VisitIOReadNode(IOReadNode node)
		{}

		public void VisitTypeNode(TypeNode node)
		{}

		public void VisitBlockNode(BlockNode node)
		{}

		public void VisitBooleanNegation(BooleanNegation node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return;
			}

			node.Factor.Accept (this);
			TokenType factorEval = node.Factor.EvaluationType;

			node.EvaluationType = factorEval == TokenType.BOOLEAN_VAL ? factorEval : TokenType.ERROR;
		}

		public void VisitExpressionNode(ExpressionNode node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return;
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
		}

		public void VisitExpressionTail(ExpressionTail node)
		{
			node.RightHandSide.Accept (this);

			node.EvaluationType = node.RightHandSide.EvaluationType;
		}

		public void VisitFactorNode(Factor node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return;
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

			return;
		}

		public void VisitFactorMain(FactorMain node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return;
			}

			node.Evaluee.Accept (this);
			node.EvaluationType = node.Evaluee.EvaluationType;
		}

		public void VisitFactorTail(FactorTail node)
		{}

		public void VisitFunctionNode(FunctionNode node)
		{}

		public void VisitProcedureNode(ProcedureNode node)
		{}

		public void VisitFunctionCallNode(FunctionCallNode node)
		{
			node.IdNode.Accept (this);

			node.EvaluationType = node.IdNode.EvaluationType;
		}

		public void VisitIfNode(IfNode node)
		{}

		public void VisitWhileLoopNode(WhileNode node)
		{}

		public void VisitParametersNode(ParametersNode node)
		{}

		public void VisitArgumentsNode(ArgumentsNode node)
		{}

		public void VisitProgramNode(ProgramNode node)
		{}

		public void VisitReturnStatement(ReturnStatement node)
		{}

		public void VisitTermNode(TermNode node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return;
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
		}

		public void VisitTermTailNode(TermTail node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return;
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
		}

		public void VisitVariableIdNode(VariableIdNode node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return;
			}

			Property prop = node.Scope.GetProperty (node.ID);

			node.EvaluationType = prop.GetTokenType ();
		}

		public void VisitSimpleExpression(SimpleExpression node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return;
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

			if (node.AdditiveInverse && !LegitOperationChecker.IsLegitOperationForEvaluations(TokenType.UNARY_OP_NEGATIVE, termEval)) {
				termEval = TokenType.ERROR;
			}

			node.EvaluationType = termEval;
		}

		public void VisitSimpleExpressionTail(SimpleExpressionTail node)
		{
			if (node.HasAlreadyBeenEvaluated) {
				return;
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
		}
	}
}