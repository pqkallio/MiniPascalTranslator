using System;

namespace Compiler
{
	public class ProcedureNode : FunctionNode
	{
		public ProcedureNode (Token token, INameFactory labelFactory, VariableIdNode idNode, ParametersNode parametersNode, BlockNode block, Scope scope)
			: base(token, idNode, parametersNode, block, scope)
		{}

		public override TokenType ReturnType
		{
			get { return TokenType.VOID; }
			set { base.ReturnType = TokenType.VOID; }
		}

		public override void Accept (INodeVisitor visitor)
		{
			visitor.VisitProcedureNode (this);
		}
	}
}