using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleCalculator.Utilities;
using JetBrains.Annotations;

namespace ConsoleCalculator.Parser.Language {
	/// <summary> Context-Free Grammar </summary>
	public partial class Cfg {
		public readonly List<Symbol> symbols;
		public readonly Nonterminal startSymbol;
		public readonly List<CfgProduction> productions;
		/// <summary> productions must be in terms of symbols. </summary>
		public Cfg (IEnumerable<Symbol> symbols, Nonterminal startSymbol, IEnumerable<CfgProduction> productions) {
			this.symbols = symbols.ToList();
			this.startSymbol = startSymbol;
			this.productions = productions.ToList();
			if (!this.symbols.Contains(startSymbol))
				throw new ArgumentException("Symbols set did not contain start symbol.");
			if (!this.productions.All(UsesOnlySymbolsInThisGrammar))
				throw new ArgumentException("Production referenced symbols not contained in symbols set.");
			if (Nonterminals.Any(IsReagentOfAnyProduction))
				throw new ArgumentException("Nonterminal was not a reagent of any production.");
			if (!symbols.AllDistinct())
				throw new ArgumentException("Symbols must be unique.");
			if (!productions.AllDistinct())//Only checks reference equality.
				throw new ArgumentException("Productions must be unique.");
			if (!symbols.Except(startSymbol).All(IsInProductOfSomeProduction))
				throw new ArgumentException("Non-start symbols must be reachable by some production.");
			var nonTerminalsThatCanExpandDirectlyToTokens = Nonterminals.Where(n => CanExpandDirectlyIntoNothingBut(n, Tokens)).ToList();
			if (!nonTerminalsThatCanExpandDirectlyToTokens.Any())
				throw new ArgumentException("There are no possible leaf-productions.");
			var resolvableNonterminals = nonTerminalsThatCanExpandDirectlyToTokens.FixedPointUnder(
				knownResolvable => Nonterminals.Where(nonterminal => CanExpandDirectlyIntoNothingBut(nonterminal, knownResolvable.Concat<Symbol>(Tokens))).ToHashSet());
			var nonTerminalsThatCannotFullyResolve = Nonterminals.Except(resolvableNonterminals).ToList();
			if (nonTerminalsThatCannotFullyResolve.Any())
				throw new ArgumentException("These nonterminals can never resolve: " + nonTerminalsThatCannotFullyResolve.ToDetailedString());
		}
		bool CanExpandDirectlyIntoNothingBut (Nonterminal expander, IEnumerable<Symbol> expandees) {
			return productions.Any(production => production.reagent == expander && expandees.ContainsAll(production.product));
		}
		bool IsInProductOfSomeProduction (Symbol symbol) => productions.Any(production => production.product.Contains(symbol));
		bool UsesOnlySymbolsInThisGrammar (CfgProduction production) => symbols.ContainsAll(production.product.Then(production.reagent));
		bool IsReagentOfAnyProduction (Nonterminal nonterminal) => productions.All(production => production.reagent != nonterminal);
		public IEnumerable<Token> Tokens => symbols.OfType<Token>();
		public IEnumerable<Nonterminal> Nonterminals => symbols.OfType<Nonterminal>();
		/// <summary> Includes null if nonterminal can be followed by end of input. </summary>
		[Pure] public IEnumerable<Token> TokensThatCanFollow (Nonterminal nonterminal) {
			var nonterminalsItCouldBe = nonterminal.Closure(NonterminalsThatCanExpandToEndWith);
			var followingSymbols = nonterminalsItCouldBe.SelectMany(SymbolsThatCanDirectlyFollow).ToList();
			return followingSymbols.OfType<Token>()
				.Concat(followingSymbols.OfType<Nonterminal>().SelectMany(TokensThatCanComeFirstInExpansionOf))
				.Concat(followingSymbols.WhereNull().Cast<Token>())
				.Distinct();
		}
		[Pure] public IEnumerable<CfgProduction> ProductionsFrom (Nonterminal nonterminal) {
			return productions.Where(production => production.reagent == nonterminal);
		}
		[Pure] public IEnumerable<Nonterminal> NonterminalsThatCanExpandToEndWith ([NotNull] Symbol symbol) {
			return Nonterminals.Where(nonterminal => ProductionsFrom(nonterminal).Any(production => production.product.LastOrDefault() == symbol));
		}
		[Pure] public IEnumerable<Token> TokensThatCanComeFirstInExpansionOf (Nonterminal nonterminal) {
			var withIntermediateNonterminals = nonterminal.Closure(
				intermediate => SymbolsThatCanComeFirstInOneExpansionOf(intermediate).OfType<Nonterminal>()
			);
			return withIntermediateNonterminals.SelectMany(SymbolsThatCanComeFirstInOneExpansionOf).OfType<Token>().Distinct();
		}
		[Pure] public IEnumerable<Symbol> SymbolsThatCanComeFirstInOneExpansionOf (Nonterminal nonterminal) {
			return productions.Where(production => production.reagent == nonterminal && production.product.Count > 0)
				.Select(production => production.product[0]).Distinct();
		}
		/// <summary> Includes null if nonterminal can be followed by end of input. </summary>
		[Pure] public IEnumerable<Symbol> SymbolsThatCanDirectlyFollow (Symbol symbol) {
			var answerIfNotStartSymbol = productions.SelectMany(production => {
				var productWithNullAtEndIfAllowed = production.reagent == startSymbol ? production.product.Then(null).ToList() : production.product;
				return Enumerable.Range(1, productWithNullAtEndIfAllowed.Count - 1)
					.Where(followIndex => productWithNullAtEndIfAllowed[followIndex - 1] == symbol)
					.Select(followIndex => productWithNullAtEndIfAllowed[followIndex]);
			}).Distinct();
			return symbol == startSymbol ? answerIfNotStartSymbol.Then(null) : answerIfNotStartSymbol;
		}
	}
}
