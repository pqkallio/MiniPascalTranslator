using System;

namespace Compiler
{
	public class ProcedureNode : FunctionNode
	{
		public ProcedureNode (Token token, INameFactory labelFactory, VariableIdNode idNode, ParametersNode parametersNode, BlockNode block, Scope scope)
			: base(token, labelFactory, idNode, parametersNode, block, scope)
		{}

		public override TokenType ReturnType
		{
			get { return TokenType.VOID; }
			set { base.ReturnType = TokenType.VOID; }
		}
	}
}