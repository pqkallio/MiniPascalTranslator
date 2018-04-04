using System;

namespace Compiler
{
	public interface IStatementsContainer : ISyntaxTreeNode
	{
		StatementsNode Sequitor {
			get;
			set;
		}
	}
}

