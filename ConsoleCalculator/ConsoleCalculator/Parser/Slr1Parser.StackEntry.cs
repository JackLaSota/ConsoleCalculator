using ConsoleCalculator.Parser.Automaton;
using ConsoleCalculator.Parser.Language;

namespace ConsoleCalculator.Parser {
	public partial class Slr1Parser <TSemanticTreeNode> {
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
	}
}
