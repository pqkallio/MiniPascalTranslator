using System;

namespace Compiler
{
	public interface IIdentifierContainer
	{
		VariableIdNode IDNode {
			get;
			set;
		}
	}
}

