using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler
{
	public class CTempVarPool
	{
		private string id;
		private int counter;
		private Dictionary<TokenType, List<string>> freeTempIds;
		private Dictionary<string, Dictionary<SyntaxTreeNode, string>> mappedTempIds;

		public CTempVarPool (string id = null)
		{
			this.id = id;
			this.counter = 0;
			this.freeTempIds = new Dictionary<TokenType, List<string>> ();
			this.mappedTempIds = new Dictionary<string, Dictionary<SyntaxTreeNode, string>> ();
		}

		public string GetTempId (TokenType type, SyntaxTreeNode node)
		{
			createFreeTempIdEntryIfNeeded (type);
			string tempId;

			if (freeTempIds [type].Count == 0) {
				tempId = getNewId ();
			} else {
				tempId = freeTempIds [type] [0];
				freeTempIds [type].RemoveAt (0);
			}

			createMappedTempIdEntryIfNeeded (tempId);
			mappedTempIds [tempId] [node] = null;

			return tempId;
		}

		public string ID
		{
			get { return id; }
		}

		public void ReturnTempId (TokenType type, string id, SyntaxTreeNode node) {
			createFreeTempIdEntryIfNeeded (type);
			Dictionary<SyntaxTreeNode, string> mappedIdEntry = mappedTempIds [id];
			mappedIdEntry.Remove (node);

			if (mappedIdEntry.Count == 0) {
				freeTempIds [type].Add (id);
			}
		}

		private void createFreeTempIdEntryIfNeeded (TokenType type)
		{
			if (!freeTempIds.ContainsKey (type)) {
				freeTempIds [type] = new List<string> ();
			}
		}

		private void createMappedTempIdEntryIfNeeded (string tempId)
		{
			if (!mappedTempIds.ContainsKey (tempId)) {
				mappedTempIds [tempId] = new Dictionary<SyntaxTreeNode, string> ();
			}
		}

		private string getNewId ()
		{
			StringBuilder sb = new StringBuilder ();

			sb.Append ("r" + counter.ToString ());

			counter++;

			return sb.ToString ();
		}

		public void UpdateLocationUsage (string location, TokenType type, SyntaxTreeNode node)
		{
			createFreeTempIdEntryIfNeeded (type);
			createMappedTempIdEntryIfNeeded (location);

			if (!mappedTempIds [location].ContainsKey (node)) {
				mappedTempIds [location] [node] = null;
			}
		}
	}
}