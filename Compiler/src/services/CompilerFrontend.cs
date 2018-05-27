using System;
using System.Collections.Generic;

namespace Compiler
{
	/// <summary>
	/// This class controls the compilation frontend.
	/// </summary>
	public class CompilerFrontend
	{
		private Scanner scanner;							// lexical analyzer
		private Parser parser;								// syntactical analyzer
		private SemanticAnalyzer semanticAnalyzer;			// semantic analyzer
		private string[] sourceLines;						// source code as lines

		public CompilerFrontend ()
		{}

		/// <summary>
		/// Initializes the compiler frontend to analyze a given filepath
		/// </summary>
		/// <param name="filePath">File path.</param>
		private void Init(string filePath) {
			this.sourceLines = readSource (filePath);
			this.scanner = new Scanner (sourceLines);
			this.parser = new Parser (scanner);
		}

		/// <summary>
		/// Compiles the source code of the given filepath.
		/// </summary>
		/// <param name="filePath">File path.</param>
		public SyntaxTree Compile(string filePath)
		{
			Init (filePath);								// initialize the compiler
			SyntaxTree syntaxTree = this.parser.Parse ();	// parse the source code into an AST

			// if any errors were found during the parse, return null
			if (lexicalAndSyntacticalErrors ()) {
				return null;
			}

			// perform semantic analysis
			semanticAnalyzer = new SemanticAnalyzer (syntaxTree);

			semanticAnalyzer.Analyze ();

			// return the AST
			return syntaxTree;
		}

		public string[] SourceLines
		{
			get { return sourceLines; }
		}

		/// <summary>
		/// Reads the source file's lines into an array of strings.
		/// </summary>
		/// <returns>The source as an array of strings.</returns>
		/// <param name="filePath">File path.</param>
		private string[] readSource(string filePath)
		{
			SourceBuffer sourceBuffer = new SourceBuffer(filePath);

			return sourceBuffer.SourceLines;
		}

		/// <summary>
		/// Lexicals and syntactical errors.
		/// </summary>
		/// <returns><c>true</c>, if lexical or syntactical errors were found, <c>false</c> otherwise.</returns>
		private bool lexicalAndSyntacticalErrors ()
		{
			bool errors = false;

			if (scanner != null) {
				errors |= scanner.getErrors ().Count != 0;
			}

			if (parser != null) {
				errors |= parser.getErrors ().Count != 0;
			}

			return errors;
		}

		public List<Error> getErrors ()
		{
			List<Error> errors = new List<Error> ();

			if (scanner != null) {
				errors.AddRange (scanner.getErrors ());
			}

			if (parser != null) {
				errors.AddRange (parser.getErrors ());
			}

			if (semanticAnalyzer != null) {
				errors.AddRange (semanticAnalyzer.getErrors ());
			}

			return errors;
		}
	}
}
