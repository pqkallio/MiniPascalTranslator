using System;
using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
	/// <summary>
	/// Represents a Declaration statement in the AST
	/// </summary>
	public class DeclarationNode : StatementNode
	{
		private List<VariableIdNode> idsToDeclare;
		private TokenType type;

		public DeclarationNode (Token token, INameFactory nameFactory, TokenType type, List<VariableIdNode> idsToDeclare)
			: base(token, nameFactory)
		{
			this.type = type;
			this.idsToDeclare = idsToDeclare;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitDeclarationNode (this);
		}
	}
}