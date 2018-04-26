using System;
using System.Collections.Generic;

namespace Compiler
{
	public class ParametersNode : SyntaxTreeNode
	{
		private List<Parameter> parameters;

		public ParametersNode (Token token, params Parameter[] parameters)
			: base(token)
		{
			this.parameters = new List<Parameter> (parameters);
		}

		public ParametersNode (Token token, List<Parameter> parameters)
			: base(token)
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

		public override ISemanticCheckValue Accept(INodeVisitor visitor)
		{
			return visitor.VisitParametersNode (this);
		}
	}
}

