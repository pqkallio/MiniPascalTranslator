using System;
using System.Collections.Generic;

namespace Compiler
{
	public class NodeBuilder
	{
		private Dictionary<string, IProperty> symbolTable;

		public NodeBuilder (Dictionary<string, IProperty> symbolTable)
		{
			this.symbolTable = symbolTable;
		}

		public RootNode CreateRootNode ()
		{
			return new RootNode ();
		}

		public StatementsNode CreateStatementsNode (Token token)
		{
			return new StatementsNode (token);
		}

		public StatementsNode CreateStatementsNode (IStatementsContainer parentNode, Token token)
		{
			StatementsNode statementsNode = new StatementsNode (token);
			parentNode.Sequitor = statementsNode;

			return statementsNode;
		}

		public VariableIdNode CreateIdNode ()
		{
			return new VariableIdNode (symbolTable);
		}

		public IExpressionNode CreateIdNode(Token t)
		{
			string value = t.Value;
			return new VariableIdNode (value, symbolTable, t);
		}

		public IExpressionNode CreateIdNode(Token t, IExpressionContainer parent)
		{
			VariableIdNode node = new VariableIdNode (t.Value, symbolTable, t);
			parent.AddExpression (node);

			return node;
		}

		public DeclarationNode CreateDeclarationNode (VariableIdNode idNode, StatementsNode statementsNode, Token t)
		{
			DeclarationNode declarationNode = new DeclarationNode (idNode, symbolTable, t);
			declarationNode.AssignNode = CreateAssignNode (idNode, t);
			statementsNode.Statement = declarationNode;

			return declarationNode;
		}

		public AssignNode CreateAssignNode (VariableIdNode idNode, Token t)
		{
			if (idNode.Token == null) {
				idNode.Token = t;
			}
			return new AssignNode (idNode, symbolTable, t);
		}

		public AssignNode CreateAssignNode (VariableIdNode idNode, StatementsNode statementsNode, Token t)
		{
			AssignNode assignNode = new AssignNode (idNode, symbolTable, t);
			statementsNode.Statement = assignNode;

			return assignNode;
		}

		public IOReadNode CreateIOReadNode (VariableIdNode idNode, StatementsNode statementsNode, Token t)
		{
			IOReadNode ioReadNode = new IOReadNode (idNode, symbolTable, t);
			statementsNode.Statement = ioReadNode;

			return ioReadNode;
		}

		public IOPrintNode CreateIOPrintNode (StatementsNode statementsNode, Token t)
		{
			IOPrintNode ioPrintNode = new IOPrintNode (t);
			statementsNode.Statement = ioPrintNode;

			return ioPrintNode;
		}

		public IOPrintNode CreateIOPrintNodeForAssertNode (AssertNode assertNode)
		{
			assertNode.IOPrintNode = new IOPrintNode (assertNode.Expression.Token);
			assertNode.IOPrintNode.AddExpression (new StringValueNode(StringFormatter.formatFailedAssertion(assertNode)));

			return assertNode.IOPrintNode;
		}

		public AssertNode CreateAssertNode (StatementsNode statementsNode, Token t)
		{
			AssertNode assertNode = new AssertNode (t);
			statementsNode.Statement = assertNode;

			return assertNode;
		}

		public BinOpNode CreateBinOpNode(IExpressionContainer parent, IExpressionNode leftHandSide, Token operation) {
			BinOpNode binOp = new BinOpNode (parent.Token);
			binOp.AddExpression (leftHandSide);
			binOp.Operation = operation.Type;
			parent.AddExpression (binOp);

			return binOp;
		}

		public BinOpNode CreateBinOpNode (IExpressionContainer parent, Token t)
		{
			BinOpNode binOp = new BinOpNode (t);
			parent.AddExpression (binOp);

			return binOp;
		}

		public UnOpNode CreateUnOpNode (IExpressionContainer parent, Token t)
		{
			UnOpNode unOp = new UnOpNode (t);
			parent.AddExpression (unOp);

			return unOp;
		}

		public IExpressionNode CreateValueNode (Token t, IExpressionContainer node, TokenType valueType)
		{
			switch (valueType) {
				case TokenType.INTEGER_VAL:
					return CreateIntValueNode (t, node);
				case TokenType.STRING_VAL:
					return CreateStringValueNode (t, node);
				case TokenType.BOOLEAN_VAL_FALSE:
					return CreateBoolValueNode (t, node);
				default:
					throw new ArgumentException();
			}
		}

		public IExpressionNode CreateIntValueNode(Token t, IExpressionContainer parent)
		{
			int value = StringUtils.parseToInt (t.Value);
			IntValueNode node = new IntValueNode (value, t);
			parent.AddExpression (node);

			return node;
		}

		public IExpressionNode CreateStringValueNode (Token t, IExpressionContainer parent)
		{
			string value = t.Value;
			StringValueNode node = new StringValueNode (value, t);
			parent.AddExpression (node);

			return node;
		}

		public IExpressionNode CreateBoolValueNode (Token t, IExpressionContainer parent)
		{
			bool value = StringUtils.parseToBoolean (t.Value);
			BoolValueNode node = new BoolValueNode (value, t);
			parent.AddExpression (node);

			return node;
		}

		public IExpressionNode CreateIntValueNode(Token t)
		{
			int value = StringUtils.parseToInt (t.Value);
			return new IntValueNode (value, t);
		}

		public IExpressionNode CreateStringValueNode (Token t)
		{
			string value = t.Value;
			return new StringValueNode (value, t);
		}

		public IExpressionNode CreateBoolValueNode (Token t)
		{
			bool value = StringUtils.parseToBoolean (t.Value);
			return new BoolValueNode (value, t);
		}

		public IExpressionNode CreateDefaultIntValueNode(Token t, IExpressionContainer parent)
		{
			IntValueNode node = new IntValueNode (SemanticAnalysisConstants.DEFAULT_INTEGER_VALUE, t);
			parent.AddExpression (node);

			return node;
		}

		public IExpressionNode CreateDefaultStringValueNode (Token t, IExpressionContainer parent)
		{
			StringValueNode node = new StringValueNode (SemanticAnalysisConstants.DEFAULT_STRING_VALUE, t);
			parent.AddExpression (node);

			return node;
		}

		public IExpressionNode CreateDefaultBoolValueNode (Token t, IExpressionContainer parent)
		{
			BoolValueNode node = new BoolValueNode (SemanticAnalysisConstants.DEFAULT_BOOL_VALUE, t);
			parent.AddExpression (node);

			return node;
		}

		public IExpressionNode CreateDefaultIntValueNode(Token t)
		{
			return new IntValueNode (SemanticAnalysisConstants.DEFAULT_INTEGER_VALUE, t);
		}

		public IExpressionNode CreateDefaultStringValueNode (Token t)
		{
			return new StringValueNode (SemanticAnalysisConstants.DEFAULT_STRING_VALUE, t);
		}

		public IExpressionNode CreateDefaultBoolValueNode (Token t)
		{
			return new BoolValueNode (SemanticAnalysisConstants.DEFAULT_BOOL_VALUE, t);
		}
	}
}

