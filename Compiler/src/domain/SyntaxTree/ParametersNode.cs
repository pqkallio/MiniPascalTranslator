using System;
using System.Collections.Generic;

namespace Compiler
{
	public class ParametersNode : ISyntaxTreeNode
	{
		private Token token;
		private List<Parameter> parameters;

		public ParametersNode (Token token)
		{
			this.token = token;
		}

		public ParametersNode (Token token, params Parameter[] parameters)
			: this(token)
		{
			this.parameters = new List<Parameter> (parameters);
		}

		public ParametersNode (Token token, List<Parameter> parameters)
			: this(token)
		{
			this.parameters = parameters;
		}

		public void AddParameter (Parameter parameter)
		{
			this.parameters.Add (parameter);
		}

		public void AddParameter (VariableIdNode idNode, TokenType parameterType, bool reference)
		{
			this.parameters.Add (new Parameter(idNode, parameterType, reference));
		}

		public List<Parameter> Parameters {
			get { return parameters; }
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return null;
		}

		public Token Token {
			get { return token; }
			set { this.token = value; }
		}
	}
}

