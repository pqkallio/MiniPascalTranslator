using System;
using System.Collections.Generic;

namespace Compiler
{
	public class SimplifiedCSynthesizer : ITargetCodeSynthesizer
	{
		private CNameFactory nameFactory;
		private SyntaxTree syntaxTree;
		private CSynthesisVisitor translationVisitor;

		public SimplifiedCSynthesizer (SyntaxTree syntaxTree = null)
		{
			this.nameFactory = new CNameFactory ();
			this.syntaxTree = syntaxTree;
			this.translationVisitor = new CSynthesisVisitor (syntaxTree.ProgramName, nameFactory);
		}

		public List<string> Translate (SyntaxTree syntaxTree = null)
		{
			if (this.syntaxTree == null) {
				this.syntaxTree = syntaxTree;
			}

			if (this.syntaxTree == null) {
				throw new ArgumentNullException ("SyntaxTree wasn't set before translation");
			}

			this.syntaxTree.Root.Accept (translationVisitor);

			return translationVisitor.Translation;
		}
	}
}

