using System;
using System.Collections.Generic;

namespace Compiler
{
	public class Scope
	{
		private Scope parentScope;
		private Dictionary<string, Property> symbolTable;
		private List<Scope> childScopes;

		public Scope (Scope parentScope=null)
		{
			this.parentScope = parentScope;
			this.symbolTable = new Dictionary<string, Property> ();
			this.childScopes = new List<Scope> ();
		}

		public bool ContainsKey (string key)
		{
			Scope scope = this;

			while (scope != null) {
				if (symbolTable.ContainsKey (key)) {
					return true;
				}
				scope = scope.ParentScope;
			}

			return false;
		}

		public bool TypeMatch (string key, TokenType type)
		{
			Property property = GetProperty (key);

			if (property.GetTokenType () == type) {
				return true;
			}

			return false;
		}

		public void AddProperty (string key, Property property)
		{
			symbolTable.Add (key, property);
		}

		public Property GetProperty (string key)
		{
			Scope scope = this;

			while (scope != null) {
				if (symbolTable.ContainsKey (key)) {
					return symbolTable[key];
				}
				scope = scope.ParentScope;
			}

			return new ErrorProperty ();
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

		public List<Scope> Children
		{
			get { return childScopes; }
		}
	}
}

