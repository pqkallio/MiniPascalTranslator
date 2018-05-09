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

		public IOPrintNode (Token token, ArgumentsNode arguments, Scope scope)
			: base(token, scope)
		{
			this.arguments = arguments;
		}

		public override void Accept(INodeVisitor visitor) {
			visitor.VisitIOPrintNode (this);
		}

		public ArgumentsNode Arguments
		{
			get { return arguments; }
		}
	}
}

