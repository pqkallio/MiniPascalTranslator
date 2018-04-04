using System;
using System.Collections;

namespace Compiler
{
	public class RootNode : IStatementsContainer
	{
		private StatementsNode sequitor;
		private Token token;

		public RootNode ()
		{
			this.sequitor = null;
			this.token = new Token (1, 0, null, TokenType.PROGRAM);
		}

		public StatementsNode Sequitor {
			get { return sequitor; }
			set { sequitor = value; }
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitRootNode (this);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}

		public override string ToString ()
		{
			return "ROOT";
		}
	}
}

