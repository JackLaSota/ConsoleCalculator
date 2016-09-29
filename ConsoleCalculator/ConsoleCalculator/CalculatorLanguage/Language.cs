using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConsoleCalculator.Parser;
using ConsoleCalculator.Parser.Language;
using ConsoleCalculator.Utilities;

namespace ConsoleCalculator.CalculatorLanguage {
	public static partial class Language {
		public static readonly Cfg grammar;
		static Language () {grammar = CreateGrammar();}//This is in a static constructor to ensure it's executed after the symbol and production fields are initialized.
		static Cfg CreateGrammar () {return new Cfg(IncludedFieldValuesOfType<ISymbol>(), IncludedFieldValuesOfType<CfgProduction>());}
		[AttributeUsage(AttributeTargets.Field)] public class IncludedInGrammarAttribute : Attribute {}
		static IEnumerable<FieldInfo> IncludedFields {
			get {
				return typeof(Language).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
					.Where(field => field.HasAttribute<IncludedInGrammarAttribute>());
			}
		}
		static IEnumerable<T> IncludedFieldValuesOfType <T> () {return IncludedFields.Select(fieldInfo => fieldInfo.GetValue(null)).OfType<T>();}
		public static Slr1Parser<SemanticTreeNode> CreateParser () {return new Slr1Parser<SemanticTreeNode>(grammar, Lex, LexemeSemantics, NonterminalSemantics);}
	}
}
