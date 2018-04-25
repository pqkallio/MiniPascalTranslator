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
			return new FunctionNode (token, labelFactory, idNode, parameters, blockNode, scope);
		}

		public VariableIdNode CreateIdNode (Scope scope)
		{
			return new VariableIdNode (scope);
		}

		public VariableIdNode CreateIdNode(Token token, Scope scope, ExpressionNode arraySizeExpression = null, TokenType arrayElementType = TokenType.UNDEFINED)
		{
			string value = token.Value;
			return new VariableIdNode (value, scope, token, arraySizeExpression: arraySizeExpression, arrayElementType: arrayElementType);
		}

		public AssignNode CreateAssignNode (VariableIdNode idNode, Scope scope, Token token, INameFactory nameFactory, ExpressionNode expression=null)
		{
			if (idNode.Token == null) {
				idNode.Token = token;
			}

			return new AssignNode (idNode, scope, token, nameFactory, expression);
		}

		public ArrayAssignStatement CreateAssignToArrayNode (VariableIdNode idNode, Scope scope, Token token, INameFactory nameFactory, ExpressionNode arrayIndexExpression, ExpressionNode assignValueExpression)
		{
			return new ArrayAssignStatement (idNode, scope, token, nameFactory, arrayIndexExpression, assignValueExpression);
		}

		public ReturnStatement CreateReturnStatement (Token token, ExpressionNode expression)
		{
			return new ReturnStatement (token, expression);
		}

		public ArgumentsNode CreateArgumentsNode (Scope scope, List<ExpressionNode> arguments, Token token)
		{
			return new ArgumentsNode (token, scope, arguments);
		}

		public FunctionCallNode CreateFunctionCallNode (VariableIdNode idNode, ArgumentsNode arguments, Token token, Scope scope, INameFactory nameFactory)
		{
			return new FunctionCallNode (token, idNode, nameFactory, arguments);
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

