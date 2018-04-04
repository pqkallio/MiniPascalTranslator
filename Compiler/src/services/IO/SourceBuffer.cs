using System;
using System.Text;
using System.Linq;
using System.IO;

namespace Compiler
{
	public class SourceBuffer
	{
		private string filePath;
		private string[] sourceLines;

		public SourceBuffer (string filePath)
		{
			this.filePath = filePath;
			this.sourceLines = null;
		}

		public string[] SourceLines
		{
			get { 
				if (sourceLines == null) {
					readLines ();
				}

				return sourceLines;
			}
		}

		public string FilePath
		{
			get { return filePath; }
			set { this.filePath = value; }
		}

		public void readLines () {
			sourceLines = File.ReadLines(FilePath, Encoding.UTF8).ToArray ();
		}
	}
}

