using System.Collections.Generic;
using System.Linq;
using ConsoleCalculator.Parser.Automaton;
using ConsoleCalculator.Parser.Language;
using ConsoleCalculator.Parser.ParseError;

namespace ConsoleCalculator.Parser {
	public partial class Slr1Parser <TSemanticTreeNode> {
		class Run {
			readonly Slr1Parser<TSemanticTreeNode> parser;
			readonly List<StackEntry> stack;
			IEnumerable<Lexeme> lexedInput;
			public Run (Slr1Parser<TSemanticTreeNode> parser, IEnumerable<Lexeme> lexedInput) {
				this.parser = parser;
				this.lexedInput = lexedInput;
				stack = new List<StackEntry> {new StackEntry(parser.dfaSpec.NewInstance(), null, null)};
			}
			void Shift (Lexeme lexeme) {
				stack.Add(new StackEntry(
					LastDfa.TransitionedOn(lexeme.token),
					parser.getLexemeSemantics(lexeme),
					lexeme.token
				));
			}
			bool CanShift (Lexeme lexeme) {return LastDfa.TransitionedOn(lexeme.token).Output;}
			StackEntry LastStackEntry => stack[stack.Count - 1];
			Dfa<List<Lr0Item>, ISymbol, bool> LastDfa => LastStackEntry.stackSoFarValidityAsPrefixClassifier;
			void ReduceBy (CfgProduction production) {
				var product = TakeProductFromStack(production);
				AddReagentToStack(production, product);
			}
			List<StackEntry> TakeProductFromStack (CfgProduction production) {
				var newCount = stack.Count - production.product.Count;
				var productStackEntries = stack.GetRange(newCount, production.product.Count);
				stack.RemoveRange(newCount, production.product.Count);
				return productStackEntries;
			}
			void AddReagentToStack (CfgProduction production, List<StackEntry> product) {
				stack.Add(new StackEntry(
					LastDfa.TransitionedOn(production.reagent),
					parser.getNonterminalSemantics(production, product.Select(entry => entry.meaning).ToList()),
					production.reagent
				));
			}
			public TSemanticTreeNode Execute () {
				var inputStream = lexedInput.GetEnumerator();
				try {
					while (inputStream.MoveNext()) {
						var next = inputStream.Current;
						var completableItem = LastDfa.currentState.SingleOrDefault(item => item.CanBeCompletedBy(next.token));
						if (completableItem != null) {
							stack.Add(new StackEntry(null /*dummy*/, parser.getLexemeSemantics(next), next.token));
							ReduceBy(completableItem.cfgProduction);
						}
						else if (CanShift(next))
							Shift(next);
						else
							throw new UnexpectedLexemeError("Unexpected lexeme: " + next.text);
					}
					if (stack.Count != 1)
						throw new UnexpectedEndOfInputError("Unexpected end of input.");
					return stack[0].meaning;
				}
				finally {inputStream.Dispose();}
			}
		}
	}
}
