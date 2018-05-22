using System;
using System.Collections.Generic;

namespace Compiler
{
	public class CNameFactory : INameFactory
	{
		private string labelBaseString;
		private string scopeBaseString;
		private int labelCounter;
		private Dictionary<Scope, CTempVarPool> tempies;
		private int scopeIdCounter;
		private int redeclarationIdCounter;

		public CNameFactory ()
		{
			this.labelBaseString = "L";
			this.scopeBaseString = "S";
			this.tempies = new Dictionary<Scope, CTempVarPool> ();

			this.labelCounter = -1;
			this.scopeIdCounter = -1;
			this.redeclarationIdCounter = -1;
		}

		public string GetLabel ()
		{
			labelCounter++;
			return labelBaseString + labelCounter.ToString ();
		}

		public string GetCName (Scope scope, string id)
		{
			createCTempVarPoolIfNeeded (scope);
			if (scope.GetProperty (id).Redeclaration) {
				SetRedeclarationIdForScope (scope);
			}

			return "_" + scope.RedeclarationId + id;
		}

		private void SetRedeclarationIdForScope (Scope scope)
		{
			if (scope.RedeclarationId.Equals ("")) {
				redeclarationIdCounter++;
				scope.RedeclarationId = redeclarationIdCounter.ToString ();
			}
		}

		public string GetTempVarId (Scope scope, TokenType type)
		{
			createCTempVarPoolIfNeeded (scope);

			string tempVarId = tempies [scope].GetTempId (type);

			return tempVarId;
		}

		public void ReturnTempVarId (Scope scope, string tempVarId, TokenType varType)
		{
			tempies [scope].ReturnTempId (varType, tempVarId);
		}

		private void createCTempVarPoolIfNeeded (Scope scope)
		{
			if (!tempies.ContainsKey (scope)) {
				scopeIdCounter++;
				tempies [scope] = new CTempVarPool (scopeBaseString + scopeIdCounter.ToString ());
			}
		}
	}
}

