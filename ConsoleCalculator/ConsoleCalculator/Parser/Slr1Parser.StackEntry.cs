using System.Collections.Generic;
using ConsoleCalculator.Parser.Automaton;
using ConsoleCalculator.Parser.Language;

namespace ConsoleCalculator.Parser {
	public partial class Slr1Parser <TSemanticTreeNode> {
		public class StackEntry {
			/// <summary> Has read in up to and including the things placed alongside it on the stack. </summary>
			public readonly Dfa<List<Lr0Item>, Symbol, bool> stackSoFarValidityAsPrefixClassifier;
			public readonly TSemanticTreeNode meaning;
			public readonly Symbol symbol;
			public StackEntry (
				Dfa<List<Lr0Item>, Symbol, bool> stackSoFarValidityAsPrefixClassifier,
				TSemanticTreeNode meaning,
				Symbol symbol
			) {
				this.stackSoFarValidityAsPrefixClassifier = stackSoFarValidityAsPrefixClassifier;
				this.meaning = meaning;
				this.symbol = symbol;
			}
		}
	}
}
