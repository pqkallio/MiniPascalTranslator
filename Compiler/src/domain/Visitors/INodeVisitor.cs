using System;

namespace Compiler
{
	public interface INodeVisitor
	{
		ISemanticCheckValue VisitAssertNode(AssertNode node);
		ISemanticCheckValue VisitAssignNode(AssignNode node);
		ISemanticCheckValue VisitBinOpNode(BinOpNode node);
		ISemanticCheckValue VisitDeclarationNode(DeclarationNode node);
		ISemanticCheckValue VisitIntValueNode(IntValueNode node);
		ISemanticCheckValue VisitIOPrintNode(IOPrintNode node);
		ISemanticCheckValue VisitIOReadNode(IOReadNode node);
		ISemanticCheckValue VisitRootNode(RootNode node);
		ISemanticCheckValue VisitStatementsNode(StatementsNode node);
		ISemanticCheckValue VisitStringValueNode(StringValueNode node);
		ISemanticCheckValue VisitUnOpNode(UnOpNode node);
		ISemanticCheckValue VisitVariableIdNode(VariableIdNode node);
		ISemanticCheckValue VisitBoolValueNode(BoolValueNode node);
	}
}

