using System;
using System.IO;
using System.Collections.Generic;

namespace Compiler
{
	public static class FileWriter
	{
		public static bool writeListToFile (List<string> lines, string target)
		{
			try {
				FileInfo fileInfo = new FileInfo (target);
			} catch {
				return false;
			}

			using (TextWriter tw = new StreamWriter (target)) {
				foreach (string line in lines) {
					tw.Write (line);
				}
			}

			return true;
		}
	}
}

