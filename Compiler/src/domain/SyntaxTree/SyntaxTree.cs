using System;
using System.Collections;

namespace Compiler
{
	public class SyntaxTree
	{
		private ProgramNode root;

		public SyntaxTree (ProgramNode root=null)
		{
			this.root = root;
		}

		public SyntaxTree ()
			: this (null)
		{}

		public ProgramNode Root {
			get { return root; }
			set { root = value; }
		}

		public override string ToString ()
		{
			return root.ToString ();
		}
	}
}
