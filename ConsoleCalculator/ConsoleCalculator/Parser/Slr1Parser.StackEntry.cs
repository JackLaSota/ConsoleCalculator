using System.Collections.Generic;
using ConsoleCalculator.Parser.Automaton;
using ConsoleCalculator.Parser.Language;

namespace ConsoleCalculator.Parser {
	public partial class Slr1Parser <TSemanticTreeNode> {
		public class StackEntry {
			/// <summary> Has read in up to and including the things placed alongside it on the stack. </summary>
			public readonly Dfa<List<Lr0Item>, ISymbol, bool> stackSoFarValidityAsPrefixClassifier;
			public readonly TSemanticTreeNode meaning;
			public readonly ISymbol symbol;
			public StackEntry (
				Dfa<List<Lr0Item>, ISymbol, bool> stackSoFarValidityAsPrefixClassifier,
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
