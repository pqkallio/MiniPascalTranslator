using System;
using System.Collections;

namespace Compiler
{
	public abstract class SyntaxTreeNode
	{
		private Token token;
		private string label;
		private ILabelFactory labelFactory;

		public SyntaxTreeNode (Token token, ILabelFactory labelFactory = null)
		{
			this.token = token;
			this.labelFactory = labelFactory;
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
	}
}

