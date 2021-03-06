﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
	/// <summary>
	/// Represents a read statement in the AST
	/// </summary>
	public class IOReadNode : StatementNode
	{
		private List<Evaluee> idNodes;

		public IOReadNode (List<Evaluee> idNodes, Scope scope, Token token)
			: base(token, scope)
		{
			this.idNodes = idNodes;
		}

		public List<Evaluee> IDNodes
		{
			get { return this.idNodes; }
			set { this.idNodes = value; }
		}

		public override void Accept(INodeVisitor visitor) {
			visitor.VisitIOReadNode (this);
		}
	}
}

