using System.Collections.Generic;
using System.Linq;

namespace ConsoleCalculator.Parser.Language {
	/// <summary> Context-Free Grammar </summary>
	public class Cfg {
		public readonly List<ISymbol> symbols;
		public readonly List<CfgProduction> productions;
		/// <summary> productions must be in terms of symbols. </summary>
		public Cfg (IEnumerable<ISymbol> symbols, IEnumerable<CfgProduction> productions) {
			this.symbols = symbols.ToList();
			this.productions = productions.ToList();
		}
	}
}
