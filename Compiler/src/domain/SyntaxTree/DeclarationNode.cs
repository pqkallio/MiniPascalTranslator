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
		private TypeNode type;

		public DeclarationNode (Token token, INameFactory nameFactory, Scope scope, TypeNode type, List<VariableIdNode> idsToDeclare)
			: base (token, nameFactory, scope)
		{
			this.type = type;
			this.idsToDeclare = idsToDeclare;
		}

		public List<VariableIdNode> IDsToDeclare
		{
			get { return this.idsToDeclare; }
		}

		public TypeNode DeclarationType
		{
			get { return this.type; }
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitDeclarationNode (this);
		}
	}
}