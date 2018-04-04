using System;
using System.Collections;

namespace Compiler
{
	public class SyntaxTree
	{
		private IStatementsContainer root;

		public SyntaxTree (IStatementsContainer root)
		{
			this.root = root;
		}

		public SyntaxTree ()
			: this (null)
		{}

		public IStatementsContainer Root {
			get { return root; }
			set { root = value; }
		}

		public override string ToString ()
		{
			return root.ToString ();
		}
	}
}
