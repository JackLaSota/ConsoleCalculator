using System.Collections.Generic;
using System.Linq;
using ConsoleCalculator.Parser.Automaton;
using ConsoleCalculator.Parser.Language;
using ConsoleCalculator.Utilities;

namespace ConsoleCalculator.Parser {
	/// <summary> Immutable. </summary>
	public partial class Slr1Parser <TSemanticTreeNode> where TSemanticTreeNode : class {
		readonly Cfg cfg;
		readonly Dfa<List<Lr0Item>, ISymbol, bool>.TimelessSpec dfaSpec;
		readonly GetLexemeSemanticsDelegate getLexemeSemantics;
		readonly GetNonterminalSemanticsDelegate getNonterminalSemantics;
		readonly LexDelegate lex;
		public static IEnumerable<Lr0Item> ItemsFor (Cfg cfg) {
			return cfg.productions.SelectMany(ItemsFor);
		}
		static IEnumerable<Lr0Item> StartItemsFor (Cfg cfg) {
			return ItemsFromFreshExpansionOfSymbol(cfg, cfg.startSymbol);
		}
		static IEnumerable<Lr0Item> ItemsFromFreshExpansionOfSymbol (Cfg cfg, Nonterminal symbol) {
			return cfg.productions.Where(production => production.reagent == symbol).Select(FirstItemFor);
		}
		static Lr0Item FirstItemFor (CfgProduction production) {return new Lr0Item(production, 0);}
		static IEnumerable<Lr0Item> ItemsFor (CfgProduction production) {
			return Enumerable.Range(0, production.product.Count).Select(index => new Lr0Item(production, index));
		}
		static IEnumerable<Lr0Item> TransitionsFunction (Cfg cfg, Lr0Item startItem, ISymbol input) {
			return startItem.Continuation.AsSingletonOrEmpty()
				.Concat(ReachableWithEmptyTokenStringFrom(cfg, startItem).Where(item => item.NextSymbol == input));
		}
		static IEnumerable<Lr0Item> ReachableWithEmptyTokenStringFrom (Cfg cfg, Lr0Item startItem) {
			return startItem.Closure(item => ReachableWithOneEmptyTokenStringStepFrom(cfg, item), true);
		}
		static IEnumerable<Lr0Item> ReachableWithOneEmptyTokenStringStepFrom (Cfg cfg, Lr0Item startItem) {
			return (startItem.NextSymbol as Nonterminal).AsSingletonOrEmpty()
				.SelectMany(nonterminal => ItemsFromFreshExpansionOfSymbol(cfg, nonterminal));
		}
		public static Dfa<List<Lr0Item>, ISymbol, bool>.TimelessSpec DfaSpecFor (Cfg cfg) {
			var items = ItemsFor(cfg).ToList();
			var inputDomain = cfg.symbols;
			//null Lr0Item represents state of invalid prefix.
			return new NfaTimelessSpec<Lr0Item, ISymbol, bool>(items, items.First()/*dummy.*/, (startItem, input) => TransitionsFunction(cfg, startItem, input), inputDomain, state => true)
				.DeterministicEquivalent(StartItemsFor(cfg).ToList(), possibleStates => possibleStates.Any());
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
		public TSemanticTreeNode Parse (string input) {return new Run(this, lex(input)).Execute();}
	}
}
