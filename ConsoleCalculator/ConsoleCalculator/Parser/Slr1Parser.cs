using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleCalculator.Parser.Automaton;
using ConsoleCalculator.Parser.Language;
using ConsoleCalculator.Parser.ParserSetupException;
using ConsoleCalculator.Utilities;
using JetBrains.Annotations;

namespace ConsoleCalculator.Parser {
	/// <summary> Immutable. </summary>
	public partial class Slr1Parser <TSemanticTreeNode> where TSemanticTreeNode : class {
		public readonly Cfg cfg;
		readonly Dfa<List<Lr0Item>, Symbol, bool>.TimelessSpec dfaSpec;
		readonly GetLexemeSemanticsDelegate getLexemeSemantics;
		readonly GetNonterminalSemanticsDelegate getNonterminalSemantics;
		readonly LexDelegate lex;
		[Pure] public static IEnumerable<Lr0Item> ItemsFor (Cfg cfg) {
			return cfg.productions.SelectMany(ItemsFor);
		}
		[Pure] public static IEnumerable<Lr0Item> StartItemsFor (Cfg cfg) {
			return ItemsFromFreshExpansionOfSymbol(cfg, cfg.startSymbol);
		}
		[Pure] public static IEnumerable<Lr0Item> ItemsFromFreshExpansionOfSymbol (Cfg cfg, Nonterminal symbol) {
			return cfg.productions.Where(production => production.reagent == symbol).Select(FirstItemFor);
		}
		[Pure] public static Lr0Item FirstItemFor (CfgProduction production) {return new Lr0Item(production, 0);}
		[Pure] public static IEnumerable<Lr0Item> ItemsFor (CfgProduction production) {
			return Enumerable.Range(0, production.product.Count).Select(index => new Lr0Item(production, index));
		}
		[Pure] public static IEnumerable<Lr0Item> TransitionsFunction (Cfg cfg, Lr0Item startItem, Symbol input) {
			return startItem.AsSingleton().SelectMany(item => item.ContinuationOn(input).AsNonNullableSingletonOrEmpty())
				.Concat(ReachableWithEmptyTokenStringFrom(cfg, startItem).SelectMany(item => item.ContinuationOn(input).AsNonNullableSingletonOrEmpty()));
		}
		[Pure] public static IEnumerable<Lr0Item> ReachableWithEmptyTokenStringFrom (Cfg cfg, Lr0Item startItem) {
			return startItem.Closure(item => ReachableWithOneEmptyTokenStringStepFrom(cfg, item), true);
		}
		[Pure] public static IEnumerable<Lr0Item> ReachableWithOneEmptyTokenStringStepFrom (Cfg cfg, Lr0Item startItem) {
			return (startItem.NextSymbol as Nonterminal).AsSingletonOrEmpty()
				.SelectMany(nonterminal => ItemsFromFreshExpansionOfSymbol(cfg, nonterminal));
		}
		[Pure] public static NfaTimelessSpec<Lr0Item, Symbol, bool> NfaSpecFor (Cfg cfg) {
			var items = ItemsFor(cfg).ToList();
			var inputDomain = cfg.symbols;
			return new NfaTimelessSpec<Lr0Item, Symbol, bool>(
				items,
				items.First()/*dummy.*/,
				(startItem, input) => TransitionsFunction(cfg, startItem, input),
				inputDomain,
				state => true
			);
		}
		[Pure] public static Dfa<List<Lr0Item>, Symbol, bool>.TimelessSpec DfaSpecFor (Cfg cfg) {
			//null Lr0Item represents state of invalid prefix.
			return NfaSpecFor(cfg).DeterministicEquivalent(
				StartItemsFor(cfg).ToList(),
				possibleStates => possibleStates.Any()
			);
		}
		Dictionary<Tuple<List<Lr0Item>, Symbol>, List<CfgProduction>> handlesWithLastOnStack
			= new Dictionary<Tuple<List<Lr0Item>, Symbol>, List<CfgProduction>>();
		[Pure] public List<CfgProduction> HandlesWithLastOnStack (List<Lr0Item> state, Symbol lastOnStack) {
			var key = new Tuple<List<Lr0Item>, Symbol>(state, lastOnStack);
			if (!handlesWithLastOnStack.ContainsKey(key))
				return new List<CfgProduction>();
			return handlesWithLastOnStack[key];
		}
		[Pure] public bool CanShift (List<Lr0Item> dfaState, Token symbol) {
			return dfaSpec.outputFunction(dfaSpec.transitionFunction(dfaState, symbol));
		}
		[Pure] public static IEnumerable<Tuple<Symbol, CfgProduction>> HandlesWithLastOnStackIn (List<Lr0Item> dfaState) {
			return dfaState.Where(item => item.Continuation == null).Select(item => new Tuple<Symbol, CfgProduction>(item.NextSymbol, item.cfgProduction));
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
			foreach (var state in dfaSpec.states) {
				var handlesByLastOnStack = HandlesWithLastOnStackIn(state).ToList();
				foreach (var lastOnStack in this.cfg.symbols) {
					var token = lastOnStack as Token;
					var canShift = token != null && CanShift(state, token);
					var handlesWithLastOnStack = handlesByLastOnStack.Where(pair => pair.Item1 == lastOnStack).ToList();
					if (canShift && handlesWithLastOnStack.Any())
						throw new NonSlr1GrammarException("Shift-reduce conflict on last on stack: " + lastOnStack + ", following state: " + state.ToDetailedString() + ".");
					var nonterminalsThisCouldReduceTo = handlesWithLastOnStack.Select(handle => handle.Item2.reagent);
					foreach (var possibleLookahead in this.cfg.symbols.OfType<Token>())
						if (nonterminalsThisCouldReduceTo.Count(nonterminal => cfg.TokensThatCanFollow(nonterminal).Contains(possibleLookahead)) > 1)
							throw new NonSlr1GrammarException("Reduce-reduce conflict on last on stack: " + lastOnStack + ", following state: " + state.ToDetailedString() + ", with input: " + possibleLookahead + ".");
					this.handlesWithLastOnStack[new Tuple<List<Lr0Item>, Symbol>(state, lastOnStack)] =
						handlesByLastOnStack.Where(handleWithRequiredLastOnStack => handleWithRequiredLastOnStack.Item1 == lastOnStack)
						.Select(handleWithRequiredLastOnStack => handleWithRequiredLastOnStack.Item2).ToList();
				}
			}
		}
		[Pure] public TSemanticTreeNode Parse (string input) {return new Run(this, lex(input)).Execute();}
	}
}
