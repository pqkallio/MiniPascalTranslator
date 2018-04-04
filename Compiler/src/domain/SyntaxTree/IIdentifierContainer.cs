using System;

namespace Compiler
{
	public interface IIdentifierContainer : ISyntaxTreeNode
	{
		VariableIdNode IDNode {
			get;
			set;
		}
	}
}

