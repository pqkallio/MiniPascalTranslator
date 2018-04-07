using System;

namespace Compiler
{
	public class Parameter
	{
		private VariableIdNode idNode;
		private TokenType parameterType;
		private bool reference;

		public Parameter (VariableIdNode idNode, TokenType parameterType, bool reference)
		{
			this.idNode = idNode;
			this.parameterType = parameterType;
			this.reference = reference;
		}

		public VariableIdNode IdNode {
			get { return idNode; }
		}

		public TokenType ParameterType {
			get { return this.parameterType; }
		}

		public bool Reference {
			get { return this.reference; }
		}
	}
}

