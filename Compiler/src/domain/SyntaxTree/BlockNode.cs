using System;
using System.Collections.Generic;

namespace Compiler
{
	public class BlockNode : StatementNode
	{
		List<StatementNode> statements;

		public BlockNode (Token token, Scope blockScope, List<StatementNode> statements)
			: base(token, blockScope)
		{
			this.statements = statements;
		}

		public override void Accept(INodeVisitor visitor)
		{
			visitor.VisitBlockNode (this);
		}

		public List<StatementNode> Statements
		{
			get { return statements; }
		}
	}
}

