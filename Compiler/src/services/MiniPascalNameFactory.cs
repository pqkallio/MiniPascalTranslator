using System;

namespace Compiler
{
	public class MiniPascalNameFactory : INameFactory
	{
		private string labelBaseString;
		private string genericTempBaseString;
		private int counter;

		public MiniPascalNameFactory ()
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

		public string GetTempVarId (Object objet = null)
		{
			counter++;

			if (objet == null) {
				return GetName (genericTempBaseString);
			}

			return GetName (objet.GetType ().Name);
		}

		private string GetName (string baseStr)
		{
			counter++;
			return "_" + baseStr + "_" + counter.ToString ();
		}
	}
}

