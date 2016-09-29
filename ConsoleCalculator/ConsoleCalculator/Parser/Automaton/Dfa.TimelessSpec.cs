using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleCalculator.Utilities;

namespace ConsoleCalculator.Parser.Automaton {
	public partial class Dfa <TState, TInput, TOutput> where TState : class {
		/// <summary> Represents the output and state transition diagram of a DFA. </summary>
		public class TimelessSpec {
			public readonly List<TState> states;
			public readonly Func<TState, TOutput> outputFunction;
			public readonly Func<TState, TInput, TState> transitionFunction;
			public readonly TState startState;
			/// <param name="states"> Must all be unique. </param>
			/// <param name="startState"> Must be in set of states. </param>
			/// <param name="transitionFunction"> Must be closed under set of states. </param>
			/// <param name="outputFunction"> Domain must contain all states. </param>
			public TimelessSpec (
				IEnumerable<TState> states,
				TState startState,
				Func<TState, TInput, TState> transitionFunction,
				Func<TState, TOutput> outputFunction
			) {
				this.states = states.ToList();
				this.startState = startState;
				this.transitionFunction = transitionFunction;
				this.outputFunction = outputFunction;
			}
			public Dfa<TState, TInput, TOutput> Instance => new Dfa<TState, TInput, TOutput>(this);
			/// <summary> Generates the DFA spec equivalent to the NFA created by allowing any start state. </summary>
			public Dfa<List<TState>, TInput, List<TOutput>>.TimelessSpec MadeNondeterministic {
				get {
					return new Dfa<List<TState>, TInput, List<TOutput>>.TimelessSpec(
						states.Combinations(),
						states,
						(possibleStates, input) => possibleStates.Select(state => transitionFunction(state, input)).Distinct().ToList(),
						possibleStates => possibleStates.Select(outputFunction).ToList()
					);
				}
			}
		}
	}
}
