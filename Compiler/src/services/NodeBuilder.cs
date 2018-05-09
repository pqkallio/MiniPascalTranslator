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

		public FunctionNode CreateFunctionNode(Token token, INameFactory labelFactory, VariableIdNode idNode, ParametersNode parameters, BlockNode blockNode, Scope scope)
		{
			return new FunctionNode (token, idNode, parameters, blockNode, scope);
		}

		public VariableIdNode CreateIdNode (Scope scope)
		{
			return new VariableIdNode (scope);
		}

		public VariableIdNode CreateIdNode(Token token, Scope scope)
		{
			string value = token.Value;
			return new VariableIdNode (value, scope, token);
		}

		public AssignNode CreateAssignNode (VariableIdNode idNode, Scope scope, Token token, ExpressionNode expression=null)
		{
			if (idNode.Token == null) {
				idNode.Token = token;
			}

			return new AssignNode (idNode, scope, token, expression);
		}

		public ArrayAssignStatement CreateAssignToArrayNode (ArrayAccessNode arrayAccessNode, Scope scope, Token token, ExpressionNode arrayIndexExpression, ExpressionNode assignValueExpression)
		{
			return new ArrayAssignStatement (arrayAccessNode, scope, token, assignValueExpression);
		}

		public ReturnStatement CreateReturnStatement (Token token, ExpressionNode expression)
		{
			return new ReturnStatement (token, expression);
		}

		public ArgumentsNode CreateArgumentsNode (Scope scope, List<ExpressionNode> arguments, Token token)
		{
			return new ArgumentsNode (token, scope, arguments);
		}

		public FunctionCallNode CreateFunctionCallNode (VariableIdNode idNode, ArgumentsNode arguments, Token token, Scope scope)
		{
			return new FunctionCallNode (token, scope, idNode, arguments);
		}

		public FactorTail CreateArraySizeCheckNode (Token token, Scope scope)
		{
			return new ArraySizeCheckNode (token, scope);
		}

		public IntValueNode CreateIntValueNode(Token token, Scope scope)
		{
			return new IntValueNode (token.Value, token, scope);
		}

		public StringValueNode CreateStringValueNode (Token token, Scope scope)
		{
			string value = token.Value;
			return new StringValueNode (value, token, scope);
		}

		public BoolValueNode CreateBoolValueNode (Token token, Scope scope)
		{
			bool value = token.Type == TokenType.BOOLEAN_VAL_TRUE ? true : false;

			return new BoolValueNode (value, token, scope);
		}

		public RealValueNode CreateRealValueNode (Token token, Scope scope)
		{
			return new RealValueNode (token.Value, token, scope);
		}
	}
}

