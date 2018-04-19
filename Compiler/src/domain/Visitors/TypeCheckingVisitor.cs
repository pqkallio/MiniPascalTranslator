using System;
using System.Collections.Generic;

namespace Compiler
{
	/// <summary>
	/// This visitor-class checks and returns the evaluation types of the AST nodes
	/// </summary>
	public class TypeCheckingVisitor : INodeVisitor
	{
		private SemanticAnalyzer analyzer; 	// the analyzer to notify in case of errors
		private VoidProperty voidProperty;  // as with the StatementCheckVisitor, use this to pass all voids

		public TypeCheckingVisitor (SemanticAnalyzer analyzer)
		{
			this.analyzer = analyzer;
			this.voidProperty = new VoidProperty ();
		}

		/// <summary>
		/// Visits the assert node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitAssertNode(AssertNode node)
		{
			// return the evaluation of the node's expression
			return getEvaluation (node.Expression);
		}

		/// <summary>
		/// Visits the assign node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitAssignNode(AssignNode node)
		{
			// return the evaluation of the node's expression
			return null;
		}

		/// <summary>
		/// Visits the bin op node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitBinOpNode(BinOpNode node)
		{
			// return the evaluation of the node using a helper method
			return VisitOperationNode (node);
		}

		/// <summary>
		/// Visits the declaration node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitDeclarationNode(DeclarationNode node)
		{
			// return the evaluation of the node's assignment
			return null;
		}

		/// <summary>
		/// Visits the int value node.
		/// </summary>
		/// <returns>the node itself.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitIntValueNode(IntValueNode node)
		{
			// Here, the node itself is the evaluation we're looking for.
			// Value nodes implement the ISemanticCheckValue interface.
			return node;
		}

		/// <summary>
		/// Visits the bool value node.
		/// </summary>
		/// <returns>the node itself.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitBoolValueNode(BoolValueNode node)
		{
			// Here, the node itself is the evaluation we're looking for.
			// Value nodes implement the ISemanticCheckValue interface.
			return node;
		}

		/// <summary>
		/// Visits the IO print node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitIOPrintNode(IOPrintNode node)
		{
			// return the node's expression's evaluation
			return null;
		}

		/// <summary>
		/// Visits the IO read node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitIOReadNode(IOReadNode node)
		{
			// return the node's variable's evaluation
			return null;
		}

		/// <summary>
		/// Visits the root node.
		/// </summary>
		/// <returns>An ISemanticValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitRootNode(RootNode node)
		{
			// nothing to evaluate here
			return voidProperty;
		}

		/// <summary>
		/// Visits the statements node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitStatementsNode(StatementsNode node)
		{
			// nothing to evaluate here
			return voidProperty;
		}

		/// <summary>
		/// Visits the string value node.
		/// </summary>
		/// <returns>the node itself.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitStringValueNode(StringValueNode node)
		{
			// Here, the node itself is the evaluation we're looking for.
			// Value nodes implement the ISemanticCheckValue interface.
			return node;
		}

		/// <summary>
		/// Visits the un op node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitUnOpNode(UnOpNode node)
		{
			// return the evaluation of the node using a helper method
			return VisitOperationNode (node);
		}

		/// <summary>
		/// Visits the variable identifier node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitVariableIdNode(VariableIdNode node)
		{
			// The IProperty interface that all the different Property classes
			// implement implements itself the ISemanticValue interface and can
			// returned themselves.
			// Primarily, return the property found in the symbolic table
			if (analyzer.SymbolicTable.ContainsKey (node.ID)) {
				return analyzer.SymbolicTable[node.ID];
			}

			// otherwise, return an error property
			analyzer.notifyError (new UninitializedVariableError (node));
			return new ErrorProperty ();
		}

		/// <summary>
		/// A private helper method to check the type of an IExpressionNode.
		/// </summary>
		/// <returns>An IProperty of the operation's type.</returns>
		/// <param name="node">Node.</param>
		private Property VisitOperationNode (IExpressionNode node)
		{
			// evaluate the IExpressionNode's expressions
			Property evaluationType = getEvaluation(node.GetExpressions());

			// check that the operation is legit for this type
			if (SemanticAnalysisConstants.LEGIT_OPERATIONS.ContainsKey(evaluationType.GetTokenType ()) &&
				SemanticAnalysisConstants.LEGIT_OPERATIONS [evaluationType.GetTokenType ()].ContainsKey (node.Operation)) {
				// if it's a logical operation, never mind the original type, the evaluation type is boolean
				if (SemanticAnalysisConstants.LOGICAL_OPERATIONS.ContainsKey (node.Operation)) {
					return new BooleanProperty ();
				}
				return evaluationType;
			}
			return new ErrorProperty ();
		}

		/// <summary>
		/// A private helper method to evaluate the type of a set of IExpressionNodes.
		/// </summary>
		/// <returns>The evaluation.</returns>
		/// <param name="expressions">Expressions.</param>
		private Property getEvaluation(params IExpressionNode[] expressions)
		{
			return null;
		}
	}
}
