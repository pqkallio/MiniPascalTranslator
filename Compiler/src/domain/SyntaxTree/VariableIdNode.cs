using System;
using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
	/// <summary>
	/// Represents a variable id in the AST.
	/// Can be used as an expression.
	/// </summary>
	public class VariableIdNode : Evaluee
	{
		private string id;
		private TokenType variableType;
		private ExpressionNode arrayIndexNode;
		private ExpressionNode arraySizeExpression;
		private TokenType arrayElementType;
		private bool arrayRequestSize;

		public VariableIdNode(Scope scope)
			: this(null, scope, null, null)
		{}

		public VariableIdNode(string id, Scope scope, Token token)
			: this(id, scope, token, null)
		{}

		public VariableIdNode (string id, Scope scope, Token token, ExpressionNode arrayIndexNode = null, ExpressionNode arraySizeExpression = null, TokenType arrayElementType = TokenType.UNDEFINED)
			: base(token, scope: scope, isVariable: true)
		{
			this.id = id;
			this.arrayIndexNode = arrayIndexNode;
			this.arraySizeExpression = arraySizeExpression;
			this.arrayElementType = arrayElementType;
			this.arrayRequestSize = false;
		}

		/// <summary>
		/// The evaluation type is the type of the IProperty corresponding
		/// to the id string in the symbol table.
		/// </summary>
		/// <value>The type of the evaluation.</value>
		public override TokenType EvaluationType
		{
			get {
				if (evaluationType != TokenType.UNDEFINED) {
					return evaluationType;
				}

				Property prop = scope.GetProperty (id);

				if (prop.GetTokenType () == TokenType.TYPE_ARRAY && arrayIndexNode != null) {
					return ((ArrayProperty)prop).ArrayElementType;
				} else if (ArrayRequestSize) {
					return TokenType.INTEGER_VAL;
				}

				return prop.GetTokenType ();
			}
		}

		public TokenType ArrayElementType
		{
			get { return arrayElementType; }
			set { arrayElementType = value; }
		}

		public ExpressionNode ArraySizeExpression
		{
			get { return arraySizeExpression; }
		}

		public void SetEvaluationType(TokenType type)
		{
			evaluationType = type;
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

		public override ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitVariableIdNode (this);
		}

		public ExpressionNode ArrayIndex
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