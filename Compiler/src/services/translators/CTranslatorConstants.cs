using System;
using System.Text;
using System.Collections.Generic;

namespace Compiler
{
	public static class CTranslatorConstants
	{
		public static readonly string PREPROCESSOR_DIRECTIVE = "#";
		public static readonly string INCLUSION = PREPROCESSOR_DIRECTIVE + "include";
		public static readonly Tuple<string, string> LIBRARY_INCLUSION_DELIMITERS = Tuple.Create("<", ">");
		public static readonly Tuple<string, string> CALL_TAIL_DELIMITERS = Tuple.Create("(", ")");
		public static readonly Tuple<string, string> BLOCK_DELIMITERS = Tuple.Create("{", "}");
		public static readonly string[] LIBRARIES = new string[] {"stdio.h", "stdlib.h"};

		public static readonly Dictionary<TokenType, string> SIMPLE_TYPE_NAMES = new Dictionary<TokenType, string>()
		{
			{TokenType.INTEGER_VAL, "int"},
			{TokenType.REAL_VAL, "float"},
			{TokenType.STRING_VAL, "char*"},
			{TokenType.BOOLEAN_VAL, "int"},
			{TokenType.TYPE_INTEGER, "int"},
			{TokenType.TYPE_REAL, "float"},
			{TokenType.TYPE_STRING, "char*"},
			{TokenType.TYPE_BOOLEAN, "int"},
			{TokenType.VOID, "void"}
		};

		public static readonly Dictionary<TokenType, string> PARAM_SIMPLE_TYPE_NAMES = new Dictionary<TokenType, string>()
		{
			{TokenType.INTEGER_VAL, "int"},
			{TokenType.REAL_VAL, "float"},
			{TokenType.STRING_VAL, "char"},
			{TokenType.BOOLEAN_VAL, "int"},
			{TokenType.TYPE_INTEGER, "int"},
			{TokenType.TYPE_REAL, "float"},
			{TokenType.TYPE_STRING, "char"},
			{TokenType.TYPE_BOOLEAN, "int"},
		};

		public static string LibraryInclusion (string library)
		{
			return spaced (INCLUSION, LibraryDelimit(library));
		}

		private static string LibraryDelimit (string library)
		{
			return LIBRARY_INCLUSION_DELIMITERS.Item1 + library + LIBRARY_INCLUSION_DELIMITERS.Item2;
		}

		private static string spaced (params string[] strings)
		{
			StringBuilder sb = new StringBuilder ();

			for (int i = 0; i < strings.Length - 1; i++) {
				sb.Append (strings [i] + ' ');
			}

			sb.Append (strings [strings.Length - 1]);

			return sb.ToString ();
		}

		public static string GetDeclarationType (TypeNode typeNode)
		{
			return SIMPLE_TYPE_NAMES [typeNode.PropertyType];
		}

		public static string createFunctionDeclaration (string functionName, VariableIdNode idNode, CNameFactory nameFactory, List<Parameter> parameters = null)
		{
			string returnType = null;

			if (SIMPLE_TYPE_NAMES.ContainsKey(idNode.VariableType)) {
				returnType = SIMPLE_TYPE_NAMES [idNode.VariableType];
			} else {
				returnType = SIMPLE_TYPE_NAMES [idNode.ArrayElementType] + '*';
			}

			string parameterString = createParameters (nameFactory, parameters);

			return statement(spaced(returnType, functionName, parameterString));
		}

		public static string createFunctionStart (string functionName, VariableIdNode idNode, CNameFactory nameFactory, List<Parameter> parameters = null)
		{
			string returnType = null;

			if (SIMPLE_TYPE_NAMES.ContainsKey(idNode.VariableType)) {
				returnType = SIMPLE_TYPE_NAMES [idNode.VariableType];
			} else {
				returnType = SIMPLE_TYPE_NAMES [idNode.ArrayElementType] + '*';
			}

			string parameterString = createParameters (nameFactory, parameters);

			return spaced(returnType, functionName, parameterString);
		}

		private static string createParameters (CNameFactory nameFactory, List<Parameter> parameters = null)
		{
			StringBuilder sb = new StringBuilder (CALL_TAIL_DELIMITERS.Item1);

			if (parameters != null && parameters.Count > 0) {
				for (int i = 0; i < parameters.Count - 1; i++) {
					sb.Append (parameterToString (nameFactory, parameters [i]) + ", ");
				}

				sb.Append (parameterToString (nameFactory, parameters [parameters.Count - 1]));
			}

			sb.Append (CALL_TAIL_DELIMITERS.Item2);

			return sb.ToString ();
		}

		private static string parameterToString (CNameFactory nameFactory, Parameter param)
		{
			StringBuilder sb = new StringBuilder ("");

			if (PARAM_SIMPLE_TYPE_NAMES.ContainsKey (param.ParameterType)) {
				sb.Append (PARAM_SIMPLE_TYPE_NAMES [param.ParameterType]);
				if (param.Reference) {
					sb.Append ('*');
				}

				sb.Append (' ');
				sb.Append (nameFactory.GetCName (param.IdNode.ID));
			} else {
				sb.Append (SIMPLE_TYPE_NAMES [param.IdNode.ArrayElementType]);
				sb.Append ('*');

				sb.Append (' ');
				sb.Append (nameFactory.GetCName (param.IdNode.ID));
				sb.Append (", ");

				sb.Append ("int " + nameFactory.GetSizeVariableForArray (param.IdNode.ID));
			}

			return sb.ToString ();
		}

		public static string statement (string str)
		{
			return str + ';';
		}
	}
}

