using System.Collections.Generic;
using System.Linq;

namespace ConsoleCalculator.Parser.Language {
	/// <summary> A production in a context-free grammar. </summary>
	public class CfgProduction {
		public readonly Nonterminal reagent;
		public readonly List<Symbol> product;
		/// <summary> reagant -> product[0]product[1]... </summary>
		/// <param name="reagent">The nonterminal symbol which is replaced by the product when the production is applied.</param>
		/// <param name="product">The result of applying the production.</param>
		public CfgProduction (Nonterminal reagent, params Symbol[] product) {
			this.reagent = reagent;
			this.product = product.ToList();
		}
		public override string ToString () => reagent + " -> " + string.Join("", product) + "";
	}
}
