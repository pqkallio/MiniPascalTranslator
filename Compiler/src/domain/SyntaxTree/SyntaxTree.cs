using System;
using System.Collections;

namespace Compiler
{
	public class SyntaxTree
	{
		private ProgramNode root;
		private string programName;

		public SyntaxTree (ProgramNode root=null, string programName=null)
		{
			this.root = root;
			this.programName = programName;
		}

		public SyntaxTree ()
			: this (null)
		{}

		public ProgramNode Root {
			get { return root; }
			set { root = value; }
		}

		public string ProgramName
		{
			get { return programName; }
			set { programName = value; }
		}

		public override string ToString ()
		{
			return root.ToString ();
		}
	}
}
