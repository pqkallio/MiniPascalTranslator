using System;
using System.Collections;

namespace Compiler
{
	public interface ISyntaxTreeNode
	{
		ISemanticCheckValue Accept(INodeVisitor visitor);
		Token Token { 
			get; 
			set; 
		}
	}
}

