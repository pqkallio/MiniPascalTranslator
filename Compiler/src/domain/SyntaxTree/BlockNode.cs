using System;
using System.Collections.Generic;

namespace Compiler
{
	public class BlockNode : SyntaxTreeNode
	{
		private Scope blockScope;
		List<StatementNode> statements;

		public BlockNode (Token token, Scope blockScope, ILabelFactory labelFactory, List<StatementNode> statements)
			: base(token, labelFactory)
		{
			this.blockScope = blockScope;
			this.statements = statements;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return new VoidProperty ();
		}
	}
}

