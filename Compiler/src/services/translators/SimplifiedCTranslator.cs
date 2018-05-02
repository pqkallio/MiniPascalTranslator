using System;
using System.Collections.Generic;

namespace Compiler
{
	public class SimplifiedCTranslator : ITargetCodeTranslator
	{
		private CNameFactory nameFactory;
		private SyntaxTree syntaxTree;
		private CTranslationVisitor translationVisitor;

		public SimplifiedCTranslator (SyntaxTree syntaxTree = null)
		{
			this.nameFactory = new CNameFactory ();
			this.syntaxTree = syntaxTree;
			this.translationVisitor = new CTranslationVisitor (syntaxTree.ProgramName, nameFactory);
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

