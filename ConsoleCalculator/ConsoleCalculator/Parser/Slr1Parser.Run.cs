using System.Collections.Generic;
using System.Linq;
using ConsoleCalculator.Parser.Automaton;
using ConsoleCalculator.Parser.Language;
using ConsoleCalculator.Parser.ParseError;
using JetBrains.Annotations;

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
			StackEntry LastStackEntry => stack[stack.Count - 1];
			Dfa<List<Lr0Item>, Symbol, bool> LastDfa => LastStackEntry.stackSoFarValidityAsPrefixClassifier;
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
			bool CanShift => LastDfa.Output;
			/// <param name="lookahead">null lookahead indicates end of stream.</param>
			CfgProduction ReductionToTake ([CanBeNull] Token lookahead) {
				return parser.Handles(LastDfa.currentState)
					.SingleOrDefault(reduction => FollowContainsLookahead(reduction, lookahead));
			}
			/// <param name="reduction">production whose reagant's follow shall be checked.</param>
			/// <param name="lookahead">null lookahead indicates end of stream.</param>
			bool FollowContainsLookahead (CfgProduction reduction, [CanBeNull] Token lookahead) {
				return parser.cfg.TokensThatCanFollow(reduction.reagent).Contains(lookahead);
			}
			/// <param name="lookahead">null lookahead indicates end of stream.</param>
			void MakeAllReductionsPossibleWith ([CanBeNull] Token lookahead) {
				while (ReductionToTake(lookahead) != null)
					ReduceBy(ReductionToTake(lookahead));
			}
			public TSemanticTreeNode Execute () {
				var inputStream = lexedInput.GetEnumerator();
				try {
					while (inputStream.MoveNext()) {
						var next = inputStream.Current;
						MakeAllReductionsPossibleWith(next.token);
						if (CanShift)
							Shift(next);
						else
							throw new UnexpectedLexemeError("Unexpected lexeme: " + next.text);
					}
					MakeAllReductionsPossibleWith(null);
					if (stack.Count != 2)//First is the special-case stack entry which holds the start-state of the DFA and no symbols. If succcessful there is a start symbol on the next and only other entry.
						throw new UnexpectedEndOfInputError("Unexpected end of input.");
					if (stack[1].symbol != parser.cfg.startSymbol)
						throw new UnexpectedEndOfInputError("Unexpected end of input.");
					return stack[1].meaning;
				}
				finally {inputStream.Dispose();}
			}
		}
	}
}
