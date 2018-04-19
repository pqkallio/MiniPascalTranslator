using System;
using System.Collections.Generic;

namespace Compiler
{
	public class NodeBuilder
	{
		public ProgramNode CreateProgramNode (Token token, Dictionary<string, FunctionNode> functions, BlockNode mainBlock, Scope scope)
		{
			return new ProgramNode (token, functions, mainBlock, scope);
		}

		public ParametersNode CreateParametersNode (Token token, List<Parameter> parameters)
		{
			return new ParametersNode (token, parameters);
		}

		public FunctionNode CreateFunctionNode(Token token, ILabelFactory labelFactory, VariableIdNode idNode, ParametersNode parameters, BlockNode blockNode, Scope scope)
		{
			return new FunctionNode (token, labelFactory, idNode, parameters, blockNode, scope);
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

		public VariableIdNode CreateIdNode (Scope scope)
		{
			return new VariableIdNode (scope);
		}

		public VariableIdNode CreateIdNode(Token t, Scope scope)
		{
			string value = t.Value;
			return new VariableIdNode (value, scope, t);
		}

		public DeclarationNode CreateDeclarationNode (VariableIdNode idNode, Scope scope, StatementsNode statementsNode, Token t)
		{
			return null;
		}

		public AssignNode CreateAssignNode (VariableIdNode idNode, Scope scope, Token token, ExpressionNode expression=null)
		{
			if (idNode.Token == null) {
				idNode.Token = token;
			}

			return new AssignNode (idNode, scope, token, expression);
		}

		public ArrayAssignStatement CreateAssignToArrayNode (VariableIdNode idNode, Scope scope, Token token, ExpressionNode arrayIndexExpression, ExpressionNode assignValueExpression)
		{
			return new ArrayAssignStatement (idNode, scope, token, arrayIndexExpression, assignValueExpression);
		}

		public AssignNode CreateAssignNode (VariableIdNode idNode, StatementsNode statementsNode, Scope scope, Token t)
		{
			AssignNode assignNode = new AssignNode (idNode, scope, t);
			statementsNode.Statement = assignNode;

			return assignNode;
		}

		public ReturnStatement CreateReturnStatement (Token token, ExpressionNode expression)
		{
			return new ReturnStatement (token, expression);
		}

		public IOReadNode CreateIOReadNode (VariableIdNode idNode, StatementsNode statementsNode, Scope scope, Token t)
		{
			return null;
		}

		public IOPrintNode CreateIOPrintNode (StatementsNode statementsNode, Token t)
		{
			return null;
		}

		public AssertNode CreateAssertNode (StatementsNode statementsNode, Token t)
		{
			AssertNode assertNode = new AssertNode (t);
			statementsNode.Statement = assertNode;

			return assertNode;
		}

		public ArgumentsNode CreateArgumentsNode (Scope scope, List<IExpressionNode> arguments, Token token)
		{
			return new ArgumentsNode (token, scope, arguments);
		}

		public FunctionCallNode CreateFunctionCallNode (VariableIdNode idNode, ArgumentsNode arguments, Token token, Scope scope)
		{
			return new FunctionCallNode (token, idNode, arguments);
		}

		public FactorTail CreateArraySizeCheckNode (Token token, Scope scope)
		{
			return new ArraySizeCheckNode (token, scope);
		}

		public IntValueNode CreateIntValueNode(Token token)
		{
			int value = StringUtils.parseToInt (token.Value);
			return new IntValueNode (value, token);
		}

		public StringValueNode CreateStringValueNode (Token token)
		{
			string value = token.Value;
			return new StringValueNode (value, token);
		}

		public BoolValueNode CreateBoolValueNode (Token t)
		{
			bool value = t.Type == TokenType.BOOLEAN_VAL_TRUE ? true : false;

			return new BoolValueNode (value, t);
		}

		public RealValueNode CreateRealValueNode (Token t)
		{
			float value = float.Parse(t.Value);

			return new RealValueNode (value, t);
		}
	}
}

