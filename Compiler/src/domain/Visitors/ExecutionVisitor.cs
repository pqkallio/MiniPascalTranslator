using System;
using System.Collections.Generic;

namespace Compiler
{
	/// <summary>
	/// A visitor class used to execute the AST's nodes' code.
	/// </summary>
	public class ExecutionVisitor : INodeVisitor
	{
		private Dictionary<string, IProperty> symbolTable;
		private EvaluationVisitor evaluator;
		private VoidProperty voidProperty;
		private IPrinter printer;
		private IReader reader;

		public ExecutionVisitor (Dictionary<string, IProperty> symbolicTable, IPrinter printer, IReader reader)
		{
			this.symbolTable = symbolicTable;
			this.evaluator = new EvaluationVisitor (this.symbolTable); // used for evaluating expressions
			this.voidProperty = new VoidProperty ();
			this.printer = printer;
			this.reader = reader;
		}

		/// <summary>
		/// Visits the root node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitRootNode(RootNode node)
		{
			// call the first statement in the program, if any
			if (node.Sequitor != null) {
				return node.Sequitor.Accept (this);
			}

			return voidProperty;
		}

		/// <summary>
		/// Visits the statements node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitStatementsNode (StatementsNode node)
		{
			// first execute the statement and then its follower
			if (node.Statement != null) {
				node.Statement.Accept (this);
			}

			if (node.Sequitor != null) {
				return node.Sequitor.Accept (this);
			}

			return voidProperty;
		}

		/// <summary>
		/// Visits the assign node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitAssignNode(AssignNode node)
		{
			// assign the idNode's value to the one in the expression node
			VariableIdNode idNode = node.IDNode;
			IProperty assignValue = node.ExprNode.Accept (this).asProperty ();
			symbolTable [idNode.ID] = assignValue;

			return voidProperty;
		}

		/// <summary>
		/// Visits the bin op node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitBinOpNode(BinOpNode node)
		{
			// let the evaluator evaluate this node
			return node.Accept (evaluator);
		}

		/// <summary>
		/// Visits the declaration node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitDeclarationNode(DeclarationNode node)
		{
			// adds the id into the symbolic table and executes the assign node
			addNewId (node.IDNode);
			node.AssignNode.Accept (this);

			return voidProperty;
		}

		/// <summary>
		/// Visits the int value node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitIntValueNode(IntValueNode node)
		{
			// let the evaluator evaluate this node
			return node.Accept (evaluator);
		}

		/// <summary>
		/// Visits the bool value node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitBoolValueNode(BoolValueNode node)
		{
			// let the evaluator evaluate this node
			return node.Accept (evaluator);
		}

		/// <summary>
		/// Visits the string value node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitStringValueNode(StringValueNode node)
		{
			// let the evaluator evaluate this node
			return node.Accept (evaluator);
		}

		/// <summary>
		/// Visits the un op node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitUnOpNode(UnOpNode node)
		{
			// let the evaluator evaluate this node
			return node.Accept (evaluator);
		}

		/// <summary>
		/// Visits the variable identifier node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitVariableIdNode(VariableIdNode node)
		{
			// let the evaluator evaluate this node
			return node.Accept (evaluator);
		}

		/// <summary>
		/// Visits the assert node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitAssertNode(AssertNode node)
		{
			// Evaluate the expression and if the result is false, execute the IOPrintNode
			// to inform the user about the failed assertion.
			// Note that this does not halt the execution!
			IProperty evaluation = node.Accept(this.evaluator).asProperty();
			if (!evaluation.asBoolean ()) {
				node.IOPrintNode.Accept(this);
			}

			return voidProperty;
		}

		/// <summary>
		/// Visits the IO print node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitIOPrintNode(IOPrintNode node)
		{
			// evaluate the expression and the print it
			IProperty evaluation = node.Expression.Accept (this).asProperty ();
			printer.print (evaluation.asString ());

			return voidProperty;
		}

		/// <summary>
		/// Visits the IO read node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitIOReadNode(IOReadNode node) 
		{
			// read an input
			string input = reader.readLine ();
			// select the first word of the input
			input = input.Split (new[] {' ', '\t', '\n'})[0];

			// set the input to the ioreadnode's assignment
			AssignNode assignNode = node.AssignNode;
			setAssignValue (input, assignNode);
			// execute the assign node
			assignNode.Accept (this);

			return voidProperty;
		}

		/// <summary>
		/// Adds a new identifier to the symbol table.
		/// </summary>
		/// <param name="idNode">Identifier node.</param>
		private void addNewId (VariableIdNode idNode)
		{
			// add the id to the symbol table and set its value to default
			switch (idNode.VariableType) {
				case TokenType.INTEGER_VAL:
					symbolTable [idNode.ID] = new IntegerProperty (SemanticAnalysisConstants.DEFAULT_INTEGER_VALUE);
					break;
				case TokenType.STRING_VAL:
					symbolTable [idNode.ID] = new StringProperty (SemanticAnalysisConstants.DEFAULT_STRING_VALUE);
					break;
				case TokenType.BOOLEAN_VAL_FALSE:
					symbolTable [idNode.ID] = new BooleanProperty (SemanticAnalysisConstants.DEFAULT_BOOL_VALUE);
					break;
				default:
					throw new RuntimeException (ErrorConstants.RUNTIME_ERROR_MESSAGE, idNode.Token);
			}
		}

		/// <summary>
		/// Sets the value of a variable read from user.
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="assignNode">Assign node.</param>
		private void setAssignValue(string input, AssignNode assignNode)
		{
			TokenType expectedType = assignNode.IDNode.EvaluationType;

			// based on the variable's type, we try to parse the user's input
			switch (expectedType) {
				case TokenType.INTEGER_VAL:
					// if its supposed to be an integer but its not, throw a runtime exception
					if (!StringUtils.isNumeral (input)) {
						throw new RuntimeException (ErrorConstants.NOT_AN_INTEGER_MESSAGE, assignNode.Token);
					}
					assignNode.AddExpression (new IntValueNode (StringUtils.parseToInt (input)));
					break;
				case TokenType.STRING_VAL:
					assignNode.AddExpression (new StringValueNode (input));
					break;
				default:
					throw new RuntimeException (ErrorConstants.RUNTIME_ERROR_MESSAGE, assignNode.Token);
			}
		}
	}
}

