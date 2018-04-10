using System;
using System.Collections.Generic;

namespace Compiler
{
	public class Scope
	{
		private Scope parentScope;
		private Dictionary<string, IProperty> symbolTable;
		private List<Scope> childScopes;

		public Scope (Scope parentScope=null)
		{
			this.parentScope = parentScope;
			this.symbolTable = new Dictionary<string, IProperty> ();
			this.childScopes = new List<Scope> ();
		}

		public IProperty GetProperty (string key)
		{
			Scope scope = this;

			while (scope != null) {
				if (symbolTable.ContainsKey (key)) {
					return symbolTable [key];
				}
				scope = scope.ParentScope;
			}

			return null;
		}

		public void AddProperty (string key, IProperty property)
		{
			if (symbolTable.ContainsKey (key)) {
				throw new DeclarationException ();
			}

			symbolTable [key] = property;
		}

		public Scope ParentScope
		{
			get { return parentScope; }
		}

		public Scope AddChildScope()
		{
			Scope newScope = new Scope (this);
			this.childScopes.Add (newScope);

			return newScope;
		}
	}
}

