using System;
using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
	/// <summary>
	/// This class performs a static semantic analysis to an AST. 
	/// </summary>
	public class SemanticAnalyzer : IErrorAggregator
	{
		private List<Error> errors;					// a list of errors found during analysis
		Dictionary<string, Property> symbolTable;	// the global symbol table common to all compiling phases
		private SyntaxTree syntaxTree;				// the earlier built AST

		/// <summary>
		/// Initializes a new instance of the <see cref="Compiler.SemanticAnalyzer"/> class.
		/// </summary>
		/// <param name="syntaxTree">Syntax tree.</param>
		/// <param name="symbolTable">Symbol table.</param>
		public SemanticAnalyzer (SyntaxTree syntaxTree, Dictionary<string, Property> symbolTable)
		{
			// at this point of the analysis the symbol table and the AST is assumed to be
			// built in order to be able to check the static semantic constraints of the source program
			this.syntaxTree = syntaxTree;
			this.symbolTable = symbolTable;
			this.errors = new List<Error> ();
		}

		/// <summary>
		/// Call to analyze the AST.
		/// </summary>
		public void Analyze ()
		{
			/* All the AST's nodes need to implement the ISyntaxTreeNode interface. This interface
			 * defines a method to accept an INodeVisitor in accordance to the visitor design pattern.
			 * The actual semantic checking is performed by an instance of a class called
			 * StatementCheckVisitor. */
			StatementCheckVisitor statementChecker = new StatementCheckVisitor (this);

			ProgramNode rootNode = syntaxTree.Root;

			// We only call on the root node to accept the statementChecker, since the checking methods
			// of the statementChecker implement the functionality of traversing and checking the
			// whole AST depth first.
			rootNode.Accept (statementChecker);
		}

		public void notifyError (Error error)
		{
			this.errors.Add (error);
		}

		public List<Error> getErrors ()
		{
			return this.errors;
		}

		public Dictionary<string, Property> SymbolicTable
		{
			get { return this.symbolTable; }
		}
	}
}

