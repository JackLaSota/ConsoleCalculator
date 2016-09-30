using JetBrains.Annotations;

namespace ConsoleCalculator.Parser.Automaton {
	/// <summary> Deterministic Finite Automaton. </summary>
	/// <typeparam name="TState"> State type. State identity is based on reference equality. </typeparam>
	/// <typeparam name="TInput"> Input type. </typeparam>
	/// <typeparam name="TOutput"> Output type. </typeparam>
	public partial class Dfa <TState, TInput, TOutput> where TState : class {
		readonly TimelessSpec timelessSpec;
		public readonly TState currentState;
		public Dfa (TimelessSpec timelessSpec) : this(timelessSpec, timelessSpec.startState) {}
		public Dfa (TimelessSpec timelessSpec, TState currentState) {
			this.timelessSpec = timelessSpec;
			this.currentState = currentState;
		}
		public TOutput Output => timelessSpec.outputFunction(currentState);
		[Pure] public TState NextState (TInput input) => timelessSpec.transitionFunction(currentState, input);
		[Pure] public Dfa <TState, TInput, TOutput> TransitionedOn (TInput input) =>
			new Dfa<TState, TInput, TOutput>(timelessSpec, NextState(input));
	}
}
