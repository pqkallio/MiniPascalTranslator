using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents a print statement in the AST
	/// </summary>
	public class IOPrintNode : StatementNode
	{
		private ArgumentsNode arguments;

		public IOPrintNode (Token token, ArgumentsNode arguments, INameFactory nameFactory)
			: base(token, nameFactory)
		{
			this.arguments = arguments;
		}

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitIOPrintNode (this);
		}

		public ArgumentsNode Arguments
		{
			get { return arguments; }
		}
	}
}

