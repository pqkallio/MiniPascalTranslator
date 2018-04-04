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

		/// <summary>
		/// Initializes a new instance of the <see cref="MiniPLInterpreter.StatementCheckVisitor"/> class.
		/// </summary>
		/// <param name="analyzer">Analyzer.</param>
		public StatementCheckVisitor (SemanticAnalyzer analyzer)
		{
			this.analyzer = analyzer;
			this.typeChecker = new TypeCheckingVisitor (analyzer);

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
			// Check that the id property in this AssignNode is ok.
			IProperty property = getVariableProperty (node);

			// if the property was voidProperty, return at this point,
			// an error has been reported already
			if (property == voidProperty) {
				return voidProperty;
			}

			VariableIdNode idNode = node.IDNode;

			// check that the id variable is declared
			checkPropertyDeclared (idNode, property, true);

			// check that the id property we should be assigning to is not constant
			// at this point of its lifecycle (i.e. not a current block's control variable)
			if (property.Constant) {
				analyzer.notifyError (new IllegalAssignmentError (node));
			}

			// evaluate the type of this property
			IProperty evaluated = node.Accept (this.typeChecker).asProperty ();

			// check the type of the expression in the assign node matches the one in the symbol table
			if (!checkPropertyType(property, evaluated.GetTokenType ())) {
				analyzer.notifyError (new IllegalTypeError(idNode));
			}

			return voidProperty;
		}

		/// <summary>
		/// Checks the static semantic constraints of a BinOpNode.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitBinOpNode(BinOpNode node)
		{
			// This is not a statement so it needs not to be actually checked here.
			// So, we pass it to the TypeCheckerVisitor instead.
			return node.Accept(this.typeChecker);
		}

		/// <summary>
		/// Checks the static semantic constraints of a BinOpNode.
		/// </summary>
		/// 
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitDeclarationNode(DeclarationNode node)
		{
			// check that the id property is ok
			IProperty property = getVariableProperty (node);

			if (property == voidProperty) {
				return voidProperty;
			}

			VariableIdNode idNode = node.IDNode;

			// if the property is declared already, it's an error
			if (property.Declared) {
				analyzer.notifyError (new DeclarationError (idNode));
			} else {
				// if it wasn't declared, now it is.
				property.Declared = true;
				// check the AssignNode
				node.AssignNode.Accept (this);
			}

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
		/// Checks the static semantic constraints of a RootNode.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitRootNode(RootNode node)
		{
			// checks the static semantic constraints of the program's first statement,
			// if any
			if (node.Sequitor != null) {
				node.Sequitor.Accept (this);
			}

			return voidProperty;
		}

		/// <summary>
		/// Checks the static semantic constraints of a StatementsNode.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitStatementsNode(StatementsNode node)
		{
			// first check the node's statement, then the following StatementsNode
			if (node.Statement != null) {
				node.Statement.Accept (this);
			}

			if (node.Sequitor != null) {
				node.Sequitor.Accept (this);
			}

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
			return node.Accept(this.typeChecker);
		}

		/// <summary>
		/// Checks the static semantic constraints of an UnOpNode.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitUnOpNode(UnOpNode node)
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
			IProperty evaluation = node.Accept(this.typeChecker).asProperty();

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
			node.Expression.Accept (this.typeChecker).asProperty ();

			return voidProperty;
		}

		/// <summary>
		/// Checks the static semantic constraints of an IOReadNode.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitIOReadNode(IOReadNode node) 
		{
			// check that the id property is ok
			IProperty property = getVariableProperty (node);

			if (property == voidProperty) {
				return voidProperty;
			}

			VariableIdNode idNode = node.IDNode;

			// check that the variable we're about to read into is declared
			checkPropertyDeclared(node, property, true);

			// check that we don't try to read a boolean value
			if (checkPropertyType(property, TokenType.BOOLEAN_VAL_FALSE)) {
				analyzer.notifyError (new IllegalTypeError(idNode));
			}

			return voidProperty;
		}

		/// <summary>
		/// A private helper method to check for an IIdentifierContainer's
		/// VariableIdNode.
		/// </summary>
		/// <returns>An IProperty.</returns>
		/// <param name="node">Node.</param>
		private IProperty getVariableProperty(IIdentifierContainer node)
		{
			// if the variable node is not set, report an error and return
			// a VoidProperty to signal this one doesn't evaluate
			if (node.IDNode == null) {
				analyzer.notifyError(new UninitializedVariableError(node));
				return voidProperty;
			}

			VariableIdNode idNode = node.IDNode;

			// check the evaluation type of the id node
			IProperty property = idNode.Accept(this.typeChecker).asProperty();

			// if it wasn't declared, report an error
			if (property.GetTokenType () == TokenType.ERROR) {
				analyzer.notifyError(new UninitializedVariableError(node));
				return voidProperty;
			}

			return property;
		}

		/// <summary>
		/// A private helper method to check a property's evaluation type. 
		/// </summary>
		/// <returns><c>true</c>, if property's type matches the expectation, <c>false</c> otherwise.</returns>
		/// <param name="property">Property.</param>
		/// <param name="expected">Expected type.</param>
		private bool checkPropertyType (IProperty property, TokenType expected)
		{
			TokenType tokenType = property.GetTokenType ();

			// if the type is error, it will be handle by the calling method,
			// so it's ok
			return tokenType == TokenType.ERROR || tokenType == expected;
		}

		/// <summary>
		/// A private helper method to help check the declaration state of a property.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="property">Property.</param>
		/// <param name="declarationExpected">If set to <c>true</c> declaration expected.</param>
		private void checkPropertyDeclared(ISyntaxTreeNode node, IProperty property, bool declarationExpected)
		{
			if (declarationExpected) {
				if (!property.Declared) {
					// if the declaration was expected, report an error about an uninitialized variable
					analyzer.notifyError (new UninitializedVariableError (node));
				}
			} else {
				if (property.Declared) {
					// if the declaration was not expected, report an error about a repeated declaration of a variable
					analyzer.notifyError (new DeclarationError (node));
				}
			}
		}
	}
}

