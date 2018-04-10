using System;
using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
	/// <summary>
	/// Represents a variable id in the AST.
	/// Can be used as an expression.
	/// </summary>
	public class VariableIdNode : IExpressionNode
	{
		private string id;
		private Scope scope;
		private TokenType variableType;
		private Token token;
		private IExpressionNode arrayIndexNode;
		private bool arrayRequestSize;

		public VariableIdNode(Scope scope)
			: this(null, scope, null, null)
		{}

		public VariableIdNode(string id, Scope scope, Token token)
			: this(id, scope, token, null)
		{}

		public VariableIdNode (string id, Scope scope, Token token, IExpressionNode arrayIndexNode)
		{
			this.id = id;
			this.scope = scope;
			this.token = token;
			this.arrayIndexNode = arrayIndexNode;
			this.arrayRequestSize = false;
		}

		/// <summary>
		/// The evaluation type is the type of the IProperty corresponding
		/// to the id string in the symbol table.
		/// </summary>
		/// <value>The type of the evaluation.</value>
		public TokenType EvaluationType
		{
			get { return scope.GetProperty (id).GetTokenType (); }
			set { }
		}

		public string ID {
			get { return id; }
			set { id = value; }
		}

		public TokenType VariableType
		{
			get { return variableType; }
			set { variableType = value; }
		}

		public override string ToString ()
		{
			return ID;
		}

		public IExpressionNode[] GetExpressions()
		{
			return null;
		}

		public TokenType Operation
		{
			get { return TokenType.BINARY_OP_NO_OP; }
			set { }
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitVariableIdNode (this);
		}

		public Token Token
		{
			get { return this.token; }
			set { this.token = value; }
		}

		public IExpressionNode ArrayIndex
		{
			get { return arrayIndexNode; }
			set { arrayIndexNode = value; }
		}

		public bool ArrayRequestSize
		{
			get { return arrayRequestSize; }
			set { arrayRequestSize = value; }
		}
	}
}