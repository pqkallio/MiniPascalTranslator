using System;
using System.Collections.Generic;

namespace Compiler
{
	public class ParametersNode : SyntaxTreeNode
	{
		private List<Parameter> parameters;

		public ParametersNode (Token token, List<Parameter> parameters, Scope scope)
			: base(token, scope)
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

		public override void Accept(INodeVisitor visitor)
		{
			visitor.VisitParametersNode (this);
		}
	}
}

