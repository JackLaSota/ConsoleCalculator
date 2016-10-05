using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleCalculator.Utilities;

namespace ConsoleCalculator.Parser.Automaton {
	/// <summary> Represents the output and state transition diagram of a nondeterministic finite automaton. </summary>
	/// <typeparam name="TState"> State type. State identity is based on reference equality. </typeparam>
	/// <typeparam name="TInput"> Input type. </typeparam>
	/// <typeparam name="TOutput"> Output type. </typeparam>
	public class NfaTimelessSpec <TState, TInput, TOutput> {
		public readonly List<TState> states;
		public readonly Func<TState, TOutput> outputFunction;
		public readonly List<TInput> inputDomain;
		public readonly Func<TState, TInput, IEnumerable<TState>> transitionsFunction;
		public readonly TState startState;
		/// <param name="states"> Must all be unique. </param>
		/// <param name="startState"> Must be in set of states. </param>
		/// <param name="transitionsFunction"> Elements of return value must always be in set of states. Must accept everything in transitionFunctionDomain. </param>
		/// <param name="inputDomain"> Domain for transitionFunction's input parameter. </param>
		/// <param name="outputFunction"> Domain must contain all states. </param>
		public NfaTimelessSpec (
			IEnumerable<TState> states,
			TState startState,
			Func<TState, TInput, IEnumerable<TState>> transitionsFunction,
			List<TInput> inputDomain,
			Func<TState, TOutput> outputFunction
		) {
			this.states = states.ToList();
			this.startState = startState;
			this.transitionsFunction = transitionsFunction;
			this.inputDomain = inputDomain;
			this.outputFunction = outputFunction;
		}
		/// <summary> Precondition: no duplicate elements in a single list. </summary>
		static bool StateSetsEquivalent (List<TState> left, List<TState> right) {
			var leftAsHashSet = left.ToHashSet();
			return left.Count == right.Count && right.All(leftAsHashSet.Contains);
		}
		/// <summary> There is a finite number of sets of possible states in a NFA. Therefore you can find an equivalent DFA whose states are sets of states. </summary>
		public Dfa<List<TState>, TInput, TNewOutput>.TimelessSpec DeterministicEquivalent <TNewOutput> (
			List<TState> possibleStartStates,
			Func<List<TState>, TNewOutput> newOutputFunction
		) {
			var setsOfStates = new List<List<TState>>();
			var getOrMakeStateSet = (Func<List<TState>, List<TState>>) (possibleDuplicateStateSet => {
				var equivalent = setsOfStates.Where(set => StateSetsEquivalent(set, possibleDuplicateStateSet));
				if (equivalent.Any())
					return equivalent.Single();
				setsOfStates.Add(possibleDuplicateStateSet);
				return possibleDuplicateStateSet;
			});
			var newTransitionFunction = (Func<List<TState>, TInput, List<TState>>) ((possibleSetOfOldStates, input) =>
				getOrMakeStateSet(possibleSetOfOldStates.SelectMany(oldState => transitionsFunction(oldState, input)).Distinct().ToList())
			);
			var reachableStateSets = possibleStartStates.Closure(
				//This is to return a list of sets of states.
				//	each set of states is the set you can have after a different possible input from the domain.
				newState => inputDomain.Select(input => newTransitionFunction(newState, input))
			).ToList();
			var cacheForNewTransitionFunction = new Dictionary<Tuple<List<TState>, TInput>, List<TState>>();
			foreach (var stateSet in reachableStateSets) {
				foreach (var input in inputDomain) {
					cacheForNewTransitionFunction[new Tuple<List<TState>, TInput>(stateSet, input)] = newTransitionFunction(stateSet, input);
				}
			}
			var cachedNewTransitionFunction = (Func<List<TState>, TInput, List<TState>>) ((possibleStates, input) =>
				cacheForNewTransitionFunction[new Tuple<List<TState>, TInput>(possibleStates, input)]
			);
			return new Dfa<List<TState>, TInput, TNewOutput>.TimelessSpec(
				reachableStateSets,
				possibleStartStates,
				cachedNewTransitionFunction,
				inputDomain,
				newOutputFunction
			);
		}
	}
}
