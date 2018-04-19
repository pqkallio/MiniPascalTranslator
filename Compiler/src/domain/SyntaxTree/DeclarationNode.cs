using System;
using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
	/// <summary>
	/// Represents a Declaration statement in the AST
	/// </summary>
	public class DeclarationNode : SyntaxTreeNode
	{
		private List<string> ids;
		private TokenType type;

		public DeclarationNode (Token t, TokenType type, List<string> ids = null)
			: base(t)
		{
			this.type = type;
			this.ids = ids;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitDeclarationNode (this);
		}
	}
}