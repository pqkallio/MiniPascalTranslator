using System;
using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
	/// <summary>
	/// Represents a variable id in the AST.
	/// Can be used as an expression.
	/// </summary>
	public class VariableIdNode : VariableEvaluee
	{
		private string id;
		private TokenType variableType;
		private TokenType arrayElementType;

		public VariableIdNode(Scope scope)
			: this(null, scope, null)
		{}

		public VariableIdNode (string id, Scope scope, Token token)
			: base(token, scope: scope)
		{
			this.id = id;
		}

		public string ID {
			get { return id; }
			set { id = value; }
		}

		public override string ToString ()
		{
			return ID;
		}

		public override void Accept(INodeVisitor visitor) {
			visitor.VisitVariableIdNode (this);
		}

		public TokenType VariableType 
		{
			get { return this.variableType; }
			set { this.variableType = value; }
		}

		public TokenType ArrayElementType
		{
			get { return this.arrayElementType; }
			set { this.arrayElementType = value; }
		}

		public override VariableIdNode IDNode {
			get { return this; }
		}
	}
}