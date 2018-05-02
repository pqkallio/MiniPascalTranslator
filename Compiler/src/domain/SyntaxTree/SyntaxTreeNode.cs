using System;
using System.Collections;

namespace Compiler
{
	public abstract class SyntaxTreeNode
	{
		protected Token token;
		protected string label;
		protected string location;
		protected INameFactory labelFactory;
		protected Scope scope;

		public SyntaxTreeNode (Token token, INameFactory labelFactory = null, Scope scope = null)
		{
			this.token = token;
			this.labelFactory = labelFactory;
			this.scope = scope;
			this.label = null;
			this.location = null;
		}

		public abstract ISemanticCheckValue Accept(INodeVisitor visitor);

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

