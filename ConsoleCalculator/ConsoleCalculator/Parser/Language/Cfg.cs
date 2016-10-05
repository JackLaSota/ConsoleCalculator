using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleCalculator.Utilities;

namespace ConsoleCalculator.Parser.Language {
	/// <summary> Context-Free Grammar </summary>
	public class Cfg {
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
		}
		bool IsInProductOfSomeProduction (Symbol symbol) => productions.Any(production => production.product.Contains(symbol));
		bool UsesOnlySymbolsInThisGrammar (CfgProduction production) => this.symbols.ContainsAll(production.product.Then(production.reagent));
		bool IsReagentOfAnyProduction (Nonterminal nonterminal) => productions.All(production => production.reagent != nonterminal);
		public IEnumerable<Token> Tokens => symbols.OfType<Token>();
		public IEnumerable<Nonterminal> Nonterminals => symbols.OfType<Nonterminal>();
	}
}
