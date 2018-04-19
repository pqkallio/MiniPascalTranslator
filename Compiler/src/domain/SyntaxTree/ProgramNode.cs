using System;
using System.Collections.Generic;

namespace Compiler
{
	public class ProgramNode : SyntaxTreeNode
	{
		private Dictionary<string, FunctionNode> functions;
		private BlockNode mainBlock;
		private Scope scope;

		public ProgramNode (Token token, Dictionary<string, FunctionNode> functionNodes, BlockNode mainBlock, Scope scope)
			: base (token)
		{
			this.mainBlock = mainBlock;
			this.scope = scope;
			this.functions = functionNodes;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return new VoidProperty ();
		}
	}
}