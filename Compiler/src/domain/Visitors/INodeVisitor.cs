using System;

namespace Compiler
{
	public interface INodeVisitor
	{
		ISemanticCheckValue VisitAssertNode(AssertNode node);
		ISemanticCheckValue VisitAssignNode(AssignNode node);
		ISemanticCheckValue VisitArrayAssignNode(ArrayAssignStatement node);
		ISemanticCheckValue VisitArrayAccessNode(ArrayAccessNode node);
		ISemanticCheckValue VisitDeclarationNode(DeclarationNode node);
		ISemanticCheckValue VisitIntValueNode(IntValueNode node);
		ISemanticCheckValue VisitRealValueNode(RealValueNode node);
		ISemanticCheckValue VisitBoolValueNode(BoolValueNode node);
		ISemanticCheckValue VisitStringValueNode(StringValueNode node);
		ISemanticCheckValue VisitIOPrintNode(IOPrintNode node);
		ISemanticCheckValue VisitIOReadNode(IOReadNode node);
		ISemanticCheckValue VisitTypeNode(TypeNode node);
		ISemanticCheckValue VisitBlockNode(BlockNode node);
		ISemanticCheckValue VisitBooleanNegation(BooleanNegation node);
		ISemanticCheckValue VisitExpressionNode(ExpressionNode node);
		ISemanticCheckValue VisitExpressionTail(ExpressionTail node);
		ISemanticCheckValue VisitFactorNode(Factor node);
		ISemanticCheckValue VisitFactorMain(FactorMain node);
		ISemanticCheckValue VisitFactorTail(FactorTail node);
		ISemanticCheckValue VisitFunctionNode(FunctionNode node);
		ISemanticCheckValue VisitFunctionCallNode(FunctionCallNode node);
		ISemanticCheckValue VisitIfNode(IfNode node);
		ISemanticCheckValue VisitWhileLoopNode(WhileNode node);
		ISemanticCheckValue VisitParametersNode(ParametersNode node);
		ISemanticCheckValue VisitArgumentsNode(ArgumentsNode node);
		ISemanticCheckValue VisitProgramNode(ProgramNode node);
		ISemanticCheckValue VisitReturnStatement(ReturnStatement node);
		ISemanticCheckValue VisitTermNode(TermNode node);
		ISemanticCheckValue VisitTermTailNode(TermTail node);
		ISemanticCheckValue VisitVariableIdNode(VariableIdNode node);
	}
}

