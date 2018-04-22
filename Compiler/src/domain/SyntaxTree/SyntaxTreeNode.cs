using System;
using System.Collections;

namespace Compiler
{
	public abstract class SyntaxTreeNode
	{
		protected Token token;
		protected string label;
		protected INameFactory labelFactory;
		protected Scope scope;

		public SyntaxTreeNode (Token token, INameFactory labelFactory = null, Scope scope = null)
		{
			this.token = token;
			this.labelFactory = labelFactory;
			this.scope = scope;
			this.label = null;
		}

		public abstract ISemanticCheckValue Accept(INodeVisitor visitor);

		public Token Token { 
			get { return token; }
			set { this.token = value; }
		}

		public string Label
		{
			get {
				if (label == null && labelFactory != null) {
					label = labelFactory.GetLabel ();
				}

				return label;
			}
		}

		public Scope Scope
		{
			get { return scope; }
		}
	}
}

