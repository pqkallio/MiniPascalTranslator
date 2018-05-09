using System;

namespace Compiler
{
	public interface INodeVisitor
	{
		void VisitAssertNode(AssertNode node);
		void VisitAssignNode(AssignNode node);
		void VisitArrayAssignNode(ArrayAssignStatement node);
		void VisitArrayAccessNode(ArrayAccessNode node);
		void VisitDeclarationNode(DeclarationNode node);
		void VisitIntValueNode(IntValueNode node);
		void VisitRealValueNode(RealValueNode node);
		void VisitBoolValueNode(BoolValueNode node);
		void VisitStringValueNode(StringValueNode node);
		void VisitIOPrintNode(IOPrintNode node);
		void VisitIOReadNode(IOReadNode node);
		void VisitTypeNode(TypeNode node);
		void VisitBlockNode(BlockNode node);
		void VisitBooleanNegation(BooleanNegation node);
		void VisitExpressionNode(ExpressionNode node);
		void VisitExpressionTail(ExpressionTail node);
		void VisitFactorNode(Factor node);
		void VisitFactorMain(FactorMain node);
		void VisitFactorTail(FactorTail node);
		void VisitFunctionNode(FunctionNode node);
		void VisitProcedureNode(ProcedureNode node);
		void VisitFunctionCallNode(FunctionCallNode node);
		void VisitIfNode(IfNode node);
		void VisitWhileLoopNode(WhileNode node);
		void VisitParametersNode(ParametersNode node);
		void VisitArgumentsNode(ArgumentsNode node);
		void VisitProgramNode(ProgramNode node);
		void VisitReturnStatement(ReturnStatement node);
		void VisitTermNode(TermNode node);
		void VisitTermTailNode(TermTail node);
		void VisitVariableIdNode(VariableIdNode node);
		void VisitSimpleExpression(SimpleExpression node);
		void VisitSimpleExpressionTail(SimpleExpressionTail node);
		void VisitArraySizeCheckNode (ArraySizeCheckNode node);
	}
}

