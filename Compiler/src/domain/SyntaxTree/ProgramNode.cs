using System;
using System.Collections.Generic;

namespace Compiler
{
	public class ProgramNode : SyntaxTreeNode
	{
		private Dictionary<string, FunctionNode> functions;
		private BlockNode mainBlock;

		public ProgramNode (Token token, Dictionary<string, FunctionNode> functionNodes, BlockNode mainBlock, Scope scope)
			: base (token, scope: scope)
		{
			this.mainBlock = mainBlock;
			this.functions = functionNodes;
		}

		public override void Accept(INodeVisitor visitor)
		{
			visitor.VisitProgramNode (this);
		}

		public Dictionary<string, FunctionNode> Functions
		{
			get { return functions; }
		}

		public BlockNode MainBlock
		{
			get { return mainBlock; }
		}
	}
}