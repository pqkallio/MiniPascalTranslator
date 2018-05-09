using System;
using System.Collections;

namespace Compiler
{
	public abstract class SyntaxTreeNode
	{
		protected Token token;
		protected string label;
		protected string location;
		protected Scope scope;

		public SyntaxTreeNode (Token token, Scope scope = null)
		{
			this.token = token;
			this.scope = scope;
			this.label = null;
			this.location = null;
		}

		public abstract void Accept(INodeVisitor visitor);

		public Token Token { 
			get { return token; }
			set { this.token = value; }
		}

		public string Label
		{
			get { return this.label; }
			set { this.label = value; }
		}

		public string Location
		{
			get { return this.location; }
			set { this.location = value; }
		}

		public Scope Scope
		{
			get { return scope; }
		}
	}
}

