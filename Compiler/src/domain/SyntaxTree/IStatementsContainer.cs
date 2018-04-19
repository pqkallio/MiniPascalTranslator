using System;

namespace Compiler
{
	public interface IStatementsContainer
	{
		StatementsNode Sequitor {
			get;
			set;
		}
	}
}

