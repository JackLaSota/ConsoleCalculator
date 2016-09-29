using System;
using System.Collections.Generic;
using ConsoleCalculator.Parser.Automaton;
using ConsoleCalculator.Parser.Language;

namespace ConsoleCalculator.Parser {
	/// <summary> Immutable. </summary>
	public class Slr1Parser <TSemanticTreeNode> {
		readonly Cfg cfg;
		readonly Dfa<Lr0Item, ISymbol, bool>.TimelessSpec dfaSpec;
		readonly GetLexemeSemanticsDelegate getLexemeSemantics;
		readonly GetNonterminalSemanticsDelegate getNonterminalSemantics;
		readonly LexDelegate lex;
		public static Dfa<Lr0Item, ISymbol, bool>.TimelessSpec DfaSpecFor (Cfg cfg) {
			throw new NotImplementedException();
		}
		public delegate IEnumerable<Lexeme> LexDelegate (string input);
		public delegate TSemanticTreeNode GetLexemeSemanticsDelegate (Lexeme lexeme);
		public delegate TSemanticTreeNode GetNonterminalSemanticsDelegate (CfgProduction production, List<TSemanticTreeNode> productSemantics);
		public Slr1Parser (Cfg cfg, LexDelegate lex, GetLexemeSemanticsDelegate getLexemeSemantics, GetNonterminalSemanticsDelegate getNonterminalSemantics) {
			this.cfg = cfg;
			this.getLexemeSemantics = getLexemeSemantics;
			this.getNonterminalSemantics = getNonterminalSemantics;
			this.lex = lex;
			dfaSpec = DfaSpecFor(cfg);
		}
		public class StackEntry {
			public readonly Dfa<Lr0Item, ISymbol, bool> stackSoFarValidityAsPrefixClassifier;
			public readonly TSemanticTreeNode meaning;
			public readonly ISymbol symbol;
			public StackEntry (
				Dfa<Lr0Item, ISymbol, bool> stackSoFarValidityAsPrefixClassifier,
				TSemanticTreeNode meaning,
				ISymbol symbol
			) {
				this.stackSoFarValidityAsPrefixClassifier = stackSoFarValidityAsPrefixClassifier;
				this.meaning = meaning;
				this.symbol = symbol;
			}
		}
		class Run {
			public readonly Slr1Parser<TSemanticTreeNode> parser;
			readonly List<StackEntry> stack;
			IEnumerator<Lexeme> inputStream;
			public Run (Slr1Parser<TSemanticTreeNode> parser, IEnumerable<Lexeme> lexedInput) {
				this.parser = parser;
				inputStream = lexedInput.GetEnumerator();
				stack = new List<StackEntry> ();
			}
			public void Shift (Lexeme lexeme) {throw new NotImplementedException();}
			public void ReduceBy (CfgProduction production) {throw new NotImplementedException();}
			public TSemanticTreeNode Execute () {
				while (inputStream.MoveNext()) {
					throw new NotImplementedException();
				}
				throw new NotImplementedException();
			}
		}
		public TSemanticTreeNode Parse (string input, Func<string, IEnumerable<Lexeme>> lex) {
			return Parse(lex(input));
		}
		TSemanticTreeNode Parse (IEnumerable<Lexeme> lexedInput) {
			return new Run(this, lexedInput).Execute();
		}
	}
}
