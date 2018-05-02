using System;
using Compiler;
using System.Linq;

namespace CompilerTests
{
	public class ParserTestInputs
	{
		public static readonly string[] progHeader = {
			"program prog;\n"
		};

		public static readonly string[] blockStart = {
			"begin\n"
		};

		public static readonly string[] blockEnd = {
			"end;\n"
		};

		public static readonly string[] mainBlockEnd = {
			"end .\n"
		};

		public static readonly string[] functionHeaderStart = {
			"function func("
		};

		public static readonly string[] functionHeaderEnd = {
			") : string;\n"
		};

		public static readonly string[] procedureHeaderStart = {
			"procedure proc("
		};

		public static readonly string[] procedureHeaderEnd = {
			");\n"
		};

		public static string[] assembleFunction(string name = "func", string returnType = "string", string parameters = null, string[] block = null)
		{
			string[] header = {"function " + name + "("};

			if (parameters != null) {
				header[0] += parameters;
			}

			header[0] += ") : " + returnType + ";\n";

			string[] functionBlock;

			if (block != null) {
				functionBlock = blockStart.Concat (block).Concat (blockEnd).ToArray ();
			} else {
				functionBlock = blockStart.Concat (blockEnd).ToArray ();
			}

			return header.Concat (functionBlock).ToArray ();
		}

		public static string[] assembleProcedure(string name = "proc", string parameters = null, string[] block = null)
		{
			string[] header = {"procedure " + name + "("};

			if (parameters != null) {
				header[0] += parameters;
			}

			header[0] += ");\n";

			string[] procedureBlock;

			if (block != null) {
				procedureBlock = blockStart.Concat (block).Concat (blockEnd).ToArray ();
			} else {
				procedureBlock = blockStart.Concat (blockEnd).ToArray ();
			}

			return header.Concat (procedureBlock).ToArray ();
		}

		public static string[] assembleBlock(string[] statements)
		{
			string[] block = statements != null ? blockStart.Concat (statements).Concat (blockEnd).ToArray () : blockStart.Concat (blockEnd).ToArray ();

			return block;
		}

		public static string[] assembleProgramCode(string[] functions = null, string[] mainBlock = null)
		{
			string [] ret = functions == null ? progHeader : progHeader.Concat (functions).ToArray ();
			ret = ret.Concat (blockStart).ToArray ();
			ret = mainBlock == null ? ret : ret.Concat (mainBlock).ToArray ();

			return ret.Concat (mainBlockEnd).ToArray ();
		}

		public static string[] emptyProgram {
			get { return assembleProgramCode(); }
		}

		public static string[] oneFunction {
			get { return assembleProgramCode (assembleFunction ()); }
		}

		public static string[] oneProcedure {
			get { return assembleProgramCode (assembleProcedure ()); }
		}

		public static string[] functionAndProcedure {
			get {
				string[] functions = assembleFunction(returnType: "real").Concat(assembleProcedure()).ToArray();
				return assembleProgramCode (functions);
			}
		}

		public static string[] functionWithParams {
			get {
				string parameters = "var i : integer, ii : real, str : string, ary : array[] of Boolean";
				return assembleProgramCode (assembleFunction (parameters: parameters, block: new[]{"var x : Boolean;\n"}), new[]{"var y : Boolean;\n"});
			}
		}

		public static string[] procedureWithParams {
			get {
				string parameters = "var i : integer, ii : real, str : string, ary : array[5] of Boolean";
				return assembleProgramCode(assembleProcedure(parameters: parameters));
			}
		}

		public static string[] declareInteger {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : integer;\n" });
			}
		}

		public static string[] declareReal {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : real;\n" });
			}
		}

		public static string[] declareBoolean {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : Boolean;\n" });
			}
		}

		public static string[] declareString {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : string;\n" });
			}
		}

		public static string[] declareArrayOfInteger {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : array[1] of integer;\n" });
			}
		}

		public static string[] declareArrayOfReal {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : array[2] of real;\n" });
			}
		}

		public static string[] declareArrayOfBoolean {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : array[3] of Boolean;\n" });
			}
		}

		public static string[] declareArrayOfString {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : array[4] of string;\n" });
			}
		}

		public static string[] declareMultipleInteger {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x, y : integer;\n" });
			}
		}

		public static string[] declareMultipleReal {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x,y,z : real;\n" });
			}
		}

		public static string[] declareMultipleBoolean {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x,y,z,k : Boolean;\n" });
			}
		}

		public static string[] declareMultipleString {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x,y,z,k,l : string;\n" });
			}
		}

		public static string[] declareMultipleArrayOfInteger {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x,y,z,k,l,m : array[10] of integer;\n" });
			}
		}

		public static string[] declareMultipleArrayOfReal {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x,y,z,k,l,m,n : array[11] of real;\n" });
			}
		}

		public static string[] declareMultipleArrayOfBoolean {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x,y,z,k,l,m,n,o : array[12] of Boolean;\n" });
			}
		}

		public static string[] declareMultipleArrayOfString {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x,y,z,k,l,m,n,o,p : array[13] of string;\n" });
			}
		}

		public static string[] declareAndAssignInteger {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : integer;\n", "x := 5;\n" });
			}
		}

		public static string[] declareAndAssignReal {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : real;\n", "x := 5.68e-4;\n" });
			}
		}

		public static string[] declareAndAssignBoolean {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : Boolean;\n", "x := true;\n" });
			}
		}

		public static string[] declareAndAssignString {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : string;\n", "x := \"supadupa\";\n" });
			}
		}

		public static string[] declareAndAssignArrayOfInteger {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : array[1] of integer;\n", "x[0] := 5;\n" });
			}
		}

		public static string[] declareAndAssignArrayOfReal {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : array[2] of real;\n", "x[1] := 0.68;\n" });
			}
		}

		public static string[] declareAndAssignArrayOfBoolean {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : array[3] of Boolean;\n", "x[0] := true;\n", "x[1] := false;\n" });
			}
		}

		public static string[] declareAndAssignArrayOfString {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : array[4] of string;\n", "x[3] := \"supadupa\";\n" });
			}
		}

		public static string[] declareAndAssignFunctionCall1 {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : string;\n", "x := func();\n" });
			}
		}

		public static string[] declareAndAssignFunctionCall2 {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : string;\n", "x := func(3, 5 + 8, true);\n" });
			}
		}

		public static string[] returnStatement1 {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : string;\n", "return x;\n" });
			}
		}

		public static string[] returnStatement2 {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : string;\n", "return;\n" });
			}
		}

		public static string[] readStatement {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : string;\n", "read(x, y, zeta);\n" });
			}
		}

		public static string[] writeStatement {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : string;\n", "writeln(x, 6 + z[0], -9, true);\n" });
			}
		}

		public static string[] assertStatement {
			get {
				return assembleProgramCode (mainBlock: new [] { "assert(x[9] % 9 < 6 / 5);\n" });
			}
		}

		public static string[] blockInABlockStatement1 {
			get {
				return assembleProgramCode (mainBlock: new [] { "begin\n",
					"  writeln(\"is funny\");",
					"  writeln(x)\n",
					"end;\n" });
			}
		}

		public static string[] blockInABlockStatement2 {
			get {
				return assembleProgramCode (mainBlock: new [] { "begin\n",
					"  writeln(\"is funny\");",
					"  writeln(x);\n",
					"end;\n" });
			}
		}

		public static string[] ifThenStatement {
			get {
				return assembleProgramCode (mainBlock: new [] { "if x > 5 then\n", "x := 4;\n" });
			}
		}

		public static string[] ifThenElseStatement {
			get {
				return assembleProgramCode (mainBlock: new [] { "if x > 5 then\n", 
					"x := 4\n", 
					"else x := x - 1;\n",
					"writeln(x)\n" });
			}
		}

		public static string[] whileStatement {
			get {
				return assembleProgramCode (mainBlock: new [] { "  var x : string;\n",
					"  x := 0;\n",
					"  while x <= 10 do\n",
					"    begin\n",
					"      x := x + 1;\n",
					"      writeln(\"x is now \", x)\n",
					"    end\n" });
			}
		}

		public static string[] accessArraySize {
			get {
				return assembleProgramCode (mainBlock: new [] { "var x : array[5] of integer;\n",
					"var y : integer;\n",
					"y := x.size\n" });
			}
		}

		public static string[] procedureCall1 {
			get {
				return assembleProgramCode (mainBlock: new [] { "proc(\"supadupa\")\n" });
			}
		}

		public static string[] procedureCall2 {
			get {
				return assembleProgramCode (mainBlock: new [] { "proc()\n" });
			}
		}

		public static string[] validIdDeclarations {
			get {
				string[] declarations = new string[ScannerConstants.RESERVED_SEQUENCES.Count - ScannerConstants.KEYWORDS.Count];
				int i = 0;

				foreach (string s in ScannerConstants.RESERVED_SEQUENCES.Keys) {
					if (!ScannerConstants.KEYWORDS.ContainsKey (s)) {
						declarations [i] = String.Format ("var {0} : integer;\n", s);
						i++;
					}
				}

				return assembleProgramCode (mainBlock: declarations);
			}
		}

		public static string[] invalidIdDeclarations {
			get {
				string[] declarations = new string[ScannerConstants.KEYWORDS.Count];
				int i = 0;

				foreach (string s in ScannerConstants.KEYWORDS.Keys) {
					declarations [i] = String.Format ("var {0} : integer;\n", s);
					i++;
				}

				return assembleProgramCode (mainBlock: declarations);
			}
		}

		public static string[] doubleDeclarationWithinScope {
			get {
				return assembleProgramCode (null, new [] {"var x : integer;\n", "var x : string;\n"});
			}
		}

		public static string[] doubleDeclarationNestedScopes {
			get {
				return assembleProgramCode (null, new [] {"var x : integer;\n"}.Concat(assembleBlock (new [] {"var x : string;\n"})).ToArray ());
			}
		}

		public static readonly string[] programMissingId = new[]
		{
			"program;\n",
			"end ."
		};

		public static readonly string[] programMissingProgramKeyword = new[]
		{
			"banana;\n",
			"end .\n"
		};

		public static readonly string[] programMissingEndKeyword = new[]
		{
			"program banana;\n",
			".\n"
		};

		public static readonly string[] programMissingColonInHeader = new[]
		{
			"program banana\n",
			"end .\n"
		};

		public static readonly string[] programMissingLastDot = new[]
		{
			"program banana;\n",
			"end \n"
		};

		public static string[] functionMissingFunctionKeyword
		{
			get {
				return assembleProgramCode (new[] {"fofo() : string;\n", "begin\n", "end;\n"});
			}
		}

		public static string[] functionMissingId
		{
			get {
				return assembleProgramCode (new[] {"function () : string;\n", "begin\n", "end;\n"});
			}
		}

		public static string[] functionMissingParams
		{
			get {
				return assembleProgramCode (new[] {"function fofo : string;\n", "begin\n", "end;\n"});
			}
		}

		public static string[] functionParamsMissingLeftParenthesis
		{
			get {
				return assembleProgramCode (new[] {"function fofo var coco : integer) : string;\n", "begin\n", "end;\n"});
			}
		}

		public static string[] functionParamsMissingRightParenthesis
		{
			get {
				return assembleProgramCode (new[] {"function fofo (var coco : integer : string;\n", "begin\n", "end;\n"});
			}
		}

		public static string[] functionParamsMissingSetTypeColon
		{
			get {
				return assembleProgramCode (new[] {"function fofo (var coco integer) : string;\n", "begin\n", "end;\n"});
			}
		}

		public static string[] functionParamsMissingType
		{
			get {
				return assembleProgramCode (new[] {"function fofo (var coco :) : string;\n", "begin\n", "end;\n"});
			}
		}

		public static string[] functionParamsMissingSetTypeColonAndType
		{
			get {
				return assembleProgramCode (new[] {"function fofo (var coco) : string;\n", "begin\n", "end;\n"});
			}
		}

		public static string[] functionParamsMissingComma
		{
			get {
				return assembleProgramCode (new[] {"function fofo (var coco : integer var it : real) : string;\n", "begin\n", "end;\n"});
			}
		}

		public static string[] functionParamsMissingReturnTypeColon
		{
			get {
				return assembleProgramCode (new[] {"function fofo (var coco : integer)  string;\n", "begin\n", "end;\n"});
			}
		}

		public static string[] functionParamsMissingReturnType
		{
			get {
				return assembleProgramCode (new[] {"function fofo (var coco : integer) :;\n", "begin\n", "end;\n"});
			}
		}

		public static string[] functionParamsMissingReturnTypeColonAndReturnType
		{
			get {
				return assembleProgramCode (new[] {"function fofo (var coco : integer);\n", "begin\n", "end;\n"});
			}
		}

		public static string[] functionHeaderMissingEndStatement
		{
			get {
				return assembleProgramCode (new[] {"function fofo (var coco : integer) : integer\n", "begin\n", "end;\n"});
			}
		}

		public static string[] functionMissingBlock
		{
			get {
				return assembleProgramCode (new[] {"function fofo (var coco : integer) : integer;\n"});
			}
		}

		// PROCEDURES
		public static string[] procedureMissingProcedureKeyword
		{
			get {
				return assembleProgramCode (new[] {"fofo();\n", "begin\n", "end;\n"});
			}
		}

		public static string[] procedureMissingId
		{
			get {
				return assembleProgramCode (new[] {"procedure ();\n", "begin\n", "end;\n"});
			}
		}

		public static string[] procedureMissingParams
		{
			get {
				return assembleProgramCode (new[] {"procedure fofo;\n", "begin\n", "end;\n"});
			}
		}

		public static string[] procedureParamsMissingLeftParenthesis
		{
			get {
				return assembleProgramCode (new[] {"procedure fofo var coco : integer);\n", "begin\n", "end;\n"});
			}
		}

		public static string[] procedureParamsMissingRightParenthesis
		{
			get {
				return assembleProgramCode (new[] {"procedure fofo (var coco : integer;\n", "begin\n", "end;\n"});
			}
		}

		public static string[] procedureParamsMissingSetTypeColon
		{
			get {
				return assembleProgramCode (new[] {"procedure fofo (var coco integer);\n", "begin\n", "end;\n"});
			}
		}

		public static string[] procedureParamsMissingType
		{
			get {
				return assembleProgramCode (new[] {"procedure fofo (var coco :);\n", "begin\n", "end;\n"});
			}
		}

		public static string[] procedureParamsMissingSetTypeColonAndType
		{
			get {
				return assembleProgramCode (new[] {"procedure fofo (var coco);\n", "begin\n", "end;\n"});
			}
		}

		public static string[] procedureParamsMissingComma
		{
			get {
				return assembleProgramCode (new[] {"procedure fofo (var coco : integer var it : real);\n", "begin\n", "end;\n"});
			}
		}

		public static string[] procedureHasReturnType
		{
			get {
				return assembleProgramCode (new[] {"procedure fofo (var coco : integer) : integer;\n", "begin\n", "end;\n"});
			}
		}

		public static string[] procedureHeaderMissingEndStatement
		{
			get {
				return assembleProgramCode (new[] {"procedure fofo (var coco : integer)\n", "begin\n", "end;\n"});
			}
		}

		public static string[] procedureMissingBlock
		{
			get {
				return assembleProgramCode (new[] {"procedure fofo (var coco : integer);\n"});
			}
		}

		public static string[] varDeclarationMissingVar
		{
			get {
				return assembleProgramCode (null, new[] {"fofo : integer;\n"});
			}
		}

		public static string[] varDeclarationMissingId
		{
			get {
				return assembleProgramCode (null, new[] {"var : integer;\n"});
			}
		}

		public static string[] varDeclarationMissingSetTypeColon
		{
			get {
				return assembleProgramCode (null, new[] {"var fofo  integer;\n"});
			}
		}

		public static string[] varDeclarationMissingType
		{
			get {
				return assembleProgramCode (null, new[] {"var babs :;\n"});
			}
		}

		public static string[] varDeclarationMissingSetTypeColonAndType
		{
			get {
				return assembleProgramCode (null, new[] {"var fofo;\n"});
			}
		}

		public static string[] varDeclarationMissingEndStatement
		{
			get {
				return assembleProgramCode (null, new[] {"var fofo : real\n"});
			}
		}

		public static string[] varDeclarationMissingComma
		{
			get {
				return assembleProgramCode (null, new[] {"var fofo baba : real;\n"});
			}
		}

		public static string[] varDeclarationIllegalType
		{
			get {
				return assembleProgramCode (null, new[] {"var fofo : regal;\n"});
			}
		}

		public static string[] varDeclarationArrayTypeMissingBrackets
		{
			get {
				return assembleProgramCode (null, new[] {"var fofo : array of string;\n"});
			}
		}

		public static string[] varDeclarationArrayTypeMissingOfKeyword
		{
			get {
				return assembleProgramCode (null, new[] {"var fofo : array[5] string;\n"});
			}
		}

		public static string[] varDeclarationArrayTypeIllegalElementType
		{
			get {
				return assembleProgramCode (null, new[] {"var fofo : array[5] of strnig;\n"});
			}
		}

		public static string[] varDeclarationNestedArrayType
		{
			get {
				return assembleProgramCode (null, new[] {"var fofo : array[5] of array[3] of string;\n"});
			}
		}

		public static string[] blockNotEnded
		{
			get {
				return assembleProgramCode (null, new[] {"begin\n"});
			}
		}

		public static string[] blockNotBegun
		{
			get {
				return assembleProgramCode (null, new[] {"end;\n"});
			}
		}

		public static string[] blockEndWithoutEndStatement
		{
			get {
				return assembleProgramCode (null, new[] {"begin\n", "end\n"});
			}
		}

		public static string[] idIsNotAStatement {
			get {
				return assembleProgramCode (null, new[] {"idid;\n"});
			}
		}

		public static string[] arrayAccessorIsNotAStatement {
			get {
				return assembleProgramCode (null, new[] {"idid[3];\n"});
			}
		}

		public static string[] assignmentMissingAssignmentSymbol1 {
			get {
				return assembleProgramCode (null, new[] {"idid[3] 4;\n"});
			}
		}

		public static string[] assignmentMissingAssignmentSymbol2 {
			get {
				return assembleProgramCode (null, new[] {"idid 4;\n"});
			}
		}

		public static string[] assignmentMissingAssignee1 {
			get {
				return assembleProgramCode (null, new[] {"idid[3] := ;\n"});
			}
		}

		public static string[] assignmentMissingAssignee2 {
			get {
				return assembleProgramCode (null, new[] {"idid := ;\n"});
			}
		}

		public static string[] assignmentMissingArrayAccessorExpression {
			get {
				return assembleProgramCode (null, new[] {"idid[] := 4;\n"});
			}
		}

		public static string[] assigneeMissingArrayAccessorExpression {
			get {
				return assembleProgramCode (null, new[] {"idid := baba[];\n"});
			}
		}
	}
}

