using System;

namespace Compiler
{
	public class CNameFactory : INameFactory
	{
		private string labelBaseString;
		private string genericTempBaseString;
		private int counter;

		public CNameFactory ()
		{
			this.labelBaseString = "L";
			this.genericTempBaseString = "temp";

			this.counter = -1;
		}

		public string GetLabel ()
		{
			counter++;
			return labelBaseString + counter.ToString ();
		}

		public string GetCName (string function)
		{
			return "_" + function;
		}

		public string GetTempVarId (Object objet = null)
		{
			counter++;

			if (objet == null) {
				return GetName (genericTempBaseString);
			}

			return GetName (objet.GetType ().Name);
		}

		public string GetSizeVariableForArray (string arrayId)
		{
			return "_" + GetCName(arrayId) + "_SIZE";
		}

		private string GetName (string baseStr)
		{
			counter++;
			return "_" + baseStr + "_" + counter.ToString ();
		}
	}
}

