using System;
using System.Collections.Generic;

namespace Compiler
{
	public class BlockNode : StatementNode
	{
		List<StatementNode> statements;

		public BlockNode (Token token, Scope blockScope, INameFactory labelFactory, List<StatementNode> statements)
			: base(token, labelFactory, blockScope)
		{
			this.statements = statements;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return visitor.VisitBlockNode (this);
		}

		public List<StatementNode> Statements
		{
			get { return statements; }
		}
	}
}

