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
		private Dictionary<string, IProperty> symbolTable;
		private TokenType variableType;
		private Token token;

		public VariableIdNode(Dictionary<string, IProperty> symbolTable)
			: this(null, symbolTable, null)
		{}

		public VariableIdNode (string id, Dictionary<string, IProperty> symbolTable, Token token)
		{
			this.id = id;
			this.symbolTable = symbolTable;
			this.token = token;
		}

		/// <summary>
		/// The evaluation type is the type of the IProperty corresponding
		/// to the id string in the symbol table.
		/// </summary>
		/// <value>The type of the evaluation.</value>
		public TokenType EvaluationType
		{
			get { return symbolTable [id].GetTokenType (); }
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
	}
}