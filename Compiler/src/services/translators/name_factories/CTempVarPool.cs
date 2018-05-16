using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler
{
	public class CTempVarPool
	{
		private string id;
		private int counter;
		private Dictionary<TokenType, List<string>> tempIds;

		public CTempVarPool (string id = null)
		{
			this.id = id;
			this.counter = 0;
			this.tempIds = new Dictionary<TokenType, List<string>> ();
		}

		public string GetTempId (TokenType type, ref bool declared)
		{
			createEntryIfNeeded (type);
			string tempId;

			if (tempIds [type].Count == 0) {
				declared = false;
				tempId = getNewId ();
			} else {
				declared = true;
				tempId = tempIds [type] [0];
				tempIds [type].RemoveAt (0);
			}

			return tempId;
		}

		public string ID
		{
			get { return id; }
		}

		public void ReturnTempId (TokenType type, string id) {
			createEntryIfNeeded (type);

			tempIds [type].Add (id);
		}

		private void createEntryIfNeeded (TokenType type)
		{
			if (!tempIds.ContainsKey (type)) {
				tempIds [type] = new List<string> ();
			}
		}

		private string getNewId ()
		{
			StringBuilder sb = new StringBuilder ();

			/*
			if (id != null) {
				sb.Append (id + "_");
			}
			*/

			sb.Append ("r" + counter.ToString ());

			counter++;

			return sb.ToString ();
		}
	}
}