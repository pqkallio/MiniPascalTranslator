using System;
using System.Collections.Generic;

namespace Compiler
{
	public class Scope
	{
		private Scope parentScope;
		private Dictionary<string, Property> symbolTable;
		private List<Scope> childScopes;
		private string redeclarationId;

		public Scope (Scope parentScope = null)
		{
			this.parentScope = parentScope;
			this.symbolTable = new Dictionary<string, Property> ();
			this.childScopes = new List<Scope> ();
			this.redeclarationId = "";
		}

		public bool ContainsKey (string key, bool searchAncestors = true)
		{
			Property prop = GetProperty (key, searchAncestors);

			return prop.GetTokenType () != TokenType.ERROR;
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

		public Property GetProperty (string key, bool searchAncestors = true)
		{
			if (symbolTable.ContainsKey (key)) {
				return symbolTable[key];
			}

			if (searchAncestors) {
				Scope scope = parentScope;

				while (scope != null) {
					if (scope.symbolTable.ContainsKey (key)) {
						return scope.symbolTable[key];
					}

					scope = scope.ParentScope;
				}
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

		public Dictionary<string, Property> SymbolTable
		{
			get { return this.symbolTable; }
		}

		public string RedeclarationId
		{
			get { return redeclarationId; }
			set { this.redeclarationId = value; }
		}
	}
}

