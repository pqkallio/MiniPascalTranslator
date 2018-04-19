using System;

namespace Compiler
{
	public class MiniPascalLabelFactory : ILabelFactory
	{
		private string baseString;
		private int counter;

		public MiniPascalLabelFactory ()
		{
			this.baseString = "L";
			this.counter = -1;
		}

		public string GetLabel ()
		{
			counter++;
			return baseString + counter.ToString ();
		}
	}
}

