using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler
{
	/// <summary>
	/// A visitor class used to evaluate an ISyntaxNode
	/// </summary>
	public class EvaluationVisitor : INodeVisitor
	{
		private Dictionary<string, IProperty> symbolTable;
		private VoidProperty voidProperty;

		public EvaluationVisitor (Dictionary<string, IProperty> symbolTable)
		{
			this.symbolTable = symbolTable;
			this.voidProperty = new VoidProperty ();
		}

		/// <summary>
		/// Visits the assert node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitAssertNode(AssertNode node)
		{
			// evaluate the expression and return it
			return node.Expression.Accept (this);
		}

		/// <summary>
		/// Visits the assign node.
		/// </summary>
		/// <returns>a VoidProperty.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitAssignNode(AssignNode node)
		{
			// nothing to evaluate here, it's done from the ExecutionVisitor
			return voidProperty;
		}

		/// <summary>
		/// Visits the bin op node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitBinOpNode(BinOpNode node)
		{
			// we start by evaluating the left hand side
			IProperty leftHandEval = getEvaluation (node.LeftOperand);

			// if it's a "no operation", return the left hand side evaluation
			if (node.Operation == TokenType.BINARY_OP_NO_OP) {
				return leftHandEval;
			}

			// otherwise, evaluate the right hand side
			IProperty rightHandEval = getEvaluation (node.RightOperand);
			// and then evaluate the operation
			IProperty evaluation = binaryOperation (node.Operation, leftHandEval, rightHandEval, node.Token);

			return evaluation;
		}

		/// <summary>
		/// Visits the declaration node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitDeclarationNode(DeclarationNode node)
		{
			// nothing to evaluate here, it's done from the ExecutionVisitor
			return voidProperty;
		}

		/// <summary>
		/// Visits the int value node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitIntValueNode(IntValueNode node)
		{
			// return a copy of the node
			IntValueNode newNode = new IntValueNode (node.Value);

			return newNode;
		}

		/// <summary>
		/// Visits the bool value node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitBoolValueNode(BoolValueNode node)
		{
			// return a copy of the node
			BoolValueNode newNode = new BoolValueNode (node.Value);

			return newNode;
		}

		/// <summary>
		/// Visits the IO print node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitIOPrintNode(IOPrintNode node)
		{
			// nothing to evaluate here, it's done from the ExecutionVisitor
			return voidProperty;
		}

		/// <summary>
		/// Visits the IO read node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitIOReadNode(IOReadNode node)
		{
			// nothing to evaluate here, it's done from the ExecutionVisitor
			return voidProperty;
		}

		/// <summary>
		/// Visits the root node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitRootNode(RootNode node)
		{
			// nothing to evaluate here, it's done from the ExecutionVisitor
			return voidProperty;
		}

		/// <summary>
		/// Visits the statements node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitStatementsNode(StatementsNode node)
		{
			// nothing to evaluate here, it's done from the ExecutionVisitor
			return voidProperty;
		}

		/// <summary>
		/// Visits the string value node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitStringValueNode(StringValueNode node)
		{
			// return a copy of the node
			StringValueNode newNode = new StringValueNode (node.Value);

			return newNode;
		}

		/// <summary>
		/// Visits the un op node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitUnOpNode(UnOpNode node)
		{
			// evaluate the operand and then perform the unary operation to it
			IProperty operandEval = getEvaluation (node.Operand);

			return unaryOperation (node.Operation, operandEval);
		}

		/// <summary>
		/// Visits the variable identifier node.
		/// </summary>
		/// <returns>An ISemanticCheckValue.</returns>
		/// <param name="node">Node.</param>
		public ISemanticCheckValue VisitVariableIdNode(VariableIdNode node)
		{
			// return a copy of the variable's current value in the symbol table
			IProperty idProperty = symbolTable [node.ID];

			switch (idProperty.GetTokenType ()) {
				case TokenType.INTEGER_VAL:
					return new IntegerProperty (idProperty.asInteger ());
				case TokenType.STRING_VAL:
					return new StringProperty (idProperty.asString ());
				case TokenType.BOOLEAN_VAL_FALSE:
					return new BooleanProperty (idProperty.asBoolean ());
				default:
					throw new ArgumentException ();
			}
		}

		/// <summary>
		/// Gets the evaluation of an expression.
		/// </summary>
		/// <returns>The evaluation.</returns>
		/// <param name="expressions">Expressions.</param>
		private IProperty getEvaluation(IExpressionNode expression)
		{
			IProperty evaluatedType = expression.Accept (this).asProperty ();

			return evaluatedType;
		}

		/// <summary>
		/// Executes a binary operation
		/// </summary>
		/// <returns>The operation.</returns>
		/// <param name="operation">Operation.</param>
		/// <param name="firstOperand">First operand.</param>
		/// <param name="secondOperand">Second operand.</param>
		/// <param name="token">Token.</param>
		public IProperty binaryOperation (TokenType operation, IProperty firstOperand, IProperty secondOperand, Token token) {
			// if it's a logical operation, evaluate it to a boolean
			if (SemanticAnalysisConstants.LOGICAL_OPERATIONS.ContainsKey (operation)) {
				return booleanBinOp (operation, firstOperand, secondOperand);
			}

			// based on the type, execute the operation
			switch (firstOperand.GetTokenType ()) {
				case TokenType.INTEGER_VAL:
					return integerBinOp (operation, firstOperand, secondOperand, token);
				case TokenType.STRING_VAL:
					return stringBinOp (operation, firstOperand, secondOperand);
				case TokenType.BOOLEAN_VAL_FALSE:
					return booleanBinOp (operation, firstOperand, secondOperand);
				default:
					throw new ArgumentException ();
			}
		}

		/// <summary>
		/// Perform a binary operation on integers.
		/// </summary>
		/// <returns>An IProperty.</returns>
		/// <param name="operation">Operation.</param>
		/// <param name="firstOperand">First operand.</param>
		/// <param name="secondOperand">Second operand.</param>
		/// <param name="token">Token.</param>
		public IProperty integerBinOp (TokenType operation, IProperty firstOperand, IProperty secondOperand, Token token)
		{
			int evaluation;

			switch (operation) {
				case TokenType.UNARY_OP_POSITIVE:
					// if it's an addition, try to evaluate, but check for overflow
					try {
						evaluation = checked(firstOperand.asInteger () + secondOperand.asInteger ());
					} catch (OverflowException) {
						throw new RuntimeException (ErrorConstants.INTEGER_OVERFLOW_ERROR_MESSAGE, token);
					}
					break;
				case TokenType.UNARY_OP_NEGATIVE:
					// if it's a subtraction, try to evaluate, but check for overflow
					try {	
						evaluation = checked(firstOperand.asInteger () - secondOperand.asInteger ());
					} catch (OverflowException) {
						throw new RuntimeException (ErrorConstants.INTEGER_OVERFLOW_ERROR_MESSAGE, token);
					}
					break;
				case TokenType.BINARY_OP_MUL:
					// if it's a multiplication, try to evaluate, but check for overflow
					try {	
						evaluation = checked(firstOperand.asInteger () * secondOperand.asInteger ());
					} catch (OverflowException) {
						throw new RuntimeException (ErrorConstants.INTEGER_OVERFLOW_ERROR_MESSAGE, token);
					}
					break;
				case TokenType.BINARY_OP_DIV:
					// if it's a division, try to evaluate, but check first for division by zero
					if (secondOperand.asInteger () == 0) {
						throw new RuntimeException (ErrorConstants.DIVISION_BY_ZERO_MESSAGE, token);
					}
					evaluation = checked(firstOperand.asInteger () / secondOperand.asInteger ());
					break;
				default:
					throw new ArgumentException (String.Format ("operation {0} not defined for integer values", operation));
			}

			firstOperand.setInteger (evaluation);

			return firstOperand;
		}

		/// <summary>
		/// Perform a binary operation on integers.
		/// </summary>
		/// <returns>An IProperty.</returns>
		/// <param name="operation">Operation.</param>
		/// <param name="firstOperand">First operand.</param>
		/// <param name="secondOperand">Second operand.</param>
		public IProperty stringBinOp (TokenType operation, IProperty firstOperand, IProperty secondOperand)
		{
			StringBuilder sb = new StringBuilder (firstOperand.asString ());

			// only operation allowed for string is concatenation, so try that
			switch (operation) {
				case TokenType.UNARY_OP_POSITIVE:
					sb.Append (secondOperand.asString ());
					break;
				default:
					throw new ArgumentException (String.Format ("operation {0} not defined for string values", operation));
			}

			firstOperand.setString (sb.ToString());

			return firstOperand;
		}

		/// <summary>
		/// Perform a binary operation that evaluates to boolean.
		/// </summary>
		/// <returns>An IProperty.</returns>
		/// <param name="operation">Operation.</param>
		/// <param name="firstOperand">First operand.</param>
		/// <param name="secondOperand">Second operand.</param>
		public IProperty booleanBinOp(TokenType operation, IProperty firstOperand, IProperty secondOperand)
		{
			// the operands themselves don't have to be booleans, comparison evaluate to boolean as well
			switch (firstOperand.GetTokenType ()) {
				case TokenType.INTEGER_VAL:
					return integerComparison (operation, firstOperand, secondOperand);
				case TokenType.STRING_VAL:
					return stringComparison (operation, firstOperand, secondOperand);
				case TokenType.BOOLEAN_VAL_FALSE:
					return booleanComparison (operation, firstOperand, secondOperand);
				default:
					throw new ArgumentException ();
			}

		}

		/// <summary>
		/// Compare two integers.
		/// </summary>
		/// <returns>An IProperty.</returns>
		/// <param name="operation">Operation.</param>
		/// <param name="firstOperand">First operand.</param>
		/// <param name="secondOperand">Second operand.</param>
		private IProperty integerComparison (TokenType operation, IProperty firstOperand, IProperty secondOperand)
		{
			BooleanProperty evaluation = new BooleanProperty ();

			switch (operation) {
				case TokenType.BINARY_OP_LOG_EQ:
					evaluation.setBoolean (firstOperand.asInteger () == secondOperand.asInteger ());
					break;
				case TokenType.BINARY_OP_LOG_LT:
					evaluation.setBoolean (firstOperand.asInteger () < secondOperand.asInteger ());
					break;
				default:
					throw new ArgumentException ();
			}

			return evaluation;
		}

		/// <summary>
		/// Compare two strings.
		/// </summary>
		/// <returns>An IProperty.</returns>
		/// <param name="operation">Operation.</param>
		/// <param name="firstOperand">First operand.</param>
		/// <param name="secondOperand">Second operand.</param>
		private IProperty stringComparison (TokenType operation, IProperty firstOperand, IProperty secondOperand)
		{
			BooleanProperty evaluation = new BooleanProperty ();

			switch (operation) {
				case TokenType.BINARY_OP_LOG_EQ:
					evaluation.setBoolean (firstOperand.asString ().CompareTo(secondOperand.asString ()) == 0);
					break;
				case TokenType.BINARY_OP_LOG_LT:
					evaluation.setBoolean (firstOperand.asString ().CompareTo(secondOperand.asString ()) < 0);
					break;
				default:
					throw new ArgumentException ();
			}

			return evaluation;
		}

		/// <summary>
		/// Compare two booleans.
		/// </summary>
		/// <returns>An IProperty.</returns>
		/// <param name="operation">Operation.</param>
		/// <param name="firstOperand">First operand.</param>
		/// <param name="secondOperand">Second operand.</param>
		private IProperty booleanComparison (TokenType operation, IProperty firstOperand, IProperty secondOperand)
		{
			switch (operation) {
				case TokenType.BINARY_OP_LOG_EQ:
					firstOperand.setBoolean (firstOperand.asBoolean () == secondOperand.asBoolean ());
					break;
				case TokenType.BINARY_OP_LOG_LT:
					firstOperand.setBoolean (firstOperand.asBoolean ().CompareTo(secondOperand.asBoolean ()) < 0);
					break;
				case TokenType.BINARY_OP_LOG_AND:
					firstOperand.setBoolean (firstOperand.asBoolean () && secondOperand.asBoolean ());
					break;
				default:
					throw new ArgumentException ();
			}

			return firstOperand;
		}

		/// <summary>
		/// Perform a unary operation on the operand.
		/// </summary>
		/// <returns>An IProperty.</returns>
		/// <param name="operation">Operation.</param>
		/// <param name="operandEvaluation">Operand evaluation.</param>
		private IProperty unaryOperation (TokenType operation, IProperty operandEvaluation)
		{
			switch (operandEvaluation.GetTokenType ()) {
				case TokenType.BOOLEAN_VAL_FALSE:
					return booleanUnOp (operation, operandEvaluation);
				case TokenType.INTEGER_VAL:
					return integerUnOp (operation, operandEvaluation);
				case TokenType.STRING_VAL:
					return stringUnOp (operation, operandEvaluation);
				default:
					throw new ArgumentException();
			}	
		}

		/// <summary>
		/// Perform a unary operation on a boolean operand.
		/// </summary>
		/// <returns>An IProperty.</returns>
		/// <param name="operation">Operation.</param>
		/// <param name="operandEvaluation">Operand evaluation.</param>
		private IProperty booleanUnOp (TokenType operation, IProperty operandEvaluation)
		{
			switch (operation) {
				case TokenType.UNARY_OP_LOG_NEG:
					operandEvaluation.setBoolean (!operandEvaluation.asBoolean ());
					return operandEvaluation;
				default:
					throw new ArgumentException ();
			}
		}

		/// <summary>
		/// Perform a unary operation on a integer operand.
		/// </summary>
		/// <returns>An IProperty.</returns>
		/// <param name="operation">Operation.</param>
		/// <param name="operandEvaluation">Operand evaluation.</param>
		private IProperty integerUnOp (TokenType operation, IProperty operandEvaluation)
		{
			// no unary operation's are defined for integers, YET, so this is a placeholder
			switch (operation) {
				default:
					throw new ArgumentException ();
			}
		}

		/// <summary>
		/// Perform a unary operation on a string operand.
		/// </summary>
		/// <returns>An IProperty.</returns>
		/// <param name="operation">Operation.</param>
		/// <param name="operandEvaluation">Operand evaluation.</param>
		private IProperty stringUnOp (TokenType operation, IProperty operandEvaluation)
		{
			// no unary operation's are defined for strings, YET, so this is a placeholder
			switch (operation) {
				default:
					throw new ArgumentException ();
			}
		}
	}
}