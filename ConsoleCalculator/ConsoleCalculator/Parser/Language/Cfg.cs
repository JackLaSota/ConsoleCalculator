using System.Collections.Generic;
using System.Linq;

namespace ConsoleCalculator.Parser.Language {
	/// <summary> Context-Free Grammar </summary>
	public class Cfg {
		public readonly List<ISymbol> symbols;
		public readonly Nonterminal startSymbol;
		public readonly List<CfgProduction> productions;
		/// <summary> productions must be in terms of symbols. </summary>
		public Cfg (IEnumerable<ISymbol> symbols, Nonterminal startSymbol, IEnumerable<CfgProduction> productions) {
			this.symbols = symbols.ToList();
			this.startSymbol = startSymbol;
			this.productions = productions.ToList();
		}
		public IEnumerable<Token> Tokens => symbols.OfType<Token>();
		public IEnumerable<Nonterminal> Nonterminals => symbols.OfType<Nonterminal>();
	}
}
