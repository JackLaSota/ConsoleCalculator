using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCalculator.Parser.Automaton {
	public partial class Dfa <TState, TInput, TOutput> where TState : class {
		/// <summary> Represents the output and state transition diagram of a DFA. </summary>
		public class TimelessSpec {
			public readonly List<TState> states;
			public readonly Func<TState, TOutput> outputFunction;
			public readonly List<TInput> inputDomain;
			public readonly Func<TState, TInput, TState> transitionFunction;
			public readonly TState startState;
			/// <param name="states"> Must all be unique. </param>
			/// <param name="startState"> Must be in set of states. </param>
			/// <param name="transitionFunction"> Must be closed under set of states. Must accept everything in transitionFunctionDomain. </param>
			/// <param name="inputDomain"> Domain for transitionFunction's input parameter. </param>
			/// <param name="outputFunction"> Domain must contain all states. </param>
			public TimelessSpec (
				IEnumerable<TState> states,
				TState startState,
				Func<TState, TInput, TState> transitionFunction,
				List<TInput> inputDomain,
				Func<TState, TOutput> outputFunction
			) {
				this.states = states.ToList();
				this.startState = startState;
				this.transitionFunction = transitionFunction;
				this.inputDomain = inputDomain;
				this.outputFunction = outputFunction;
			}
			public Dfa<TState, TInput, TOutput> NewInstance () => new Dfa<TState, TInput, TOutput>(this);
		}
	}
}
