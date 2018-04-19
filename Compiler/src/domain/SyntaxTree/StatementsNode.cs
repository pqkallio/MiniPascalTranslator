using System;
using System.Collections;

namespace Compiler
{
	/// <summary>
	/// Represents a statement in the AST
	/// </summary>
	public class StatementsNode : IStatementsContainer
	{
		public StatementNode statement;
		public StatementsNode sequitor;
		public Token token;

		public StatementsNode (Token token)
		{
			this.statement = null;
			this.sequitor = null;
			this.token = token;
		}

		public StatementNode Statement {
			get { return statement; }
			set { statement = value; }
		}

		public StatementsNode Sequitor {
			get { return sequitor; }
			set { sequitor = value; }
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitStatementsNode (this);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}
	}
}

