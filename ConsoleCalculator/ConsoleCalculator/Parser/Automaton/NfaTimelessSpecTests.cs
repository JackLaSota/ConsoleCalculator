using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ConsoleCalculator.Parser.Automaton {
	[TestFixture] public class NfaTimelessSpecTests {
		[Test] public void DeterministicEquivalentTest () {
			var maxState = 4;
			var nfaTimelessSpec = new NfaTimelessSpec<int, int, int>(
				Enumerable.Range(0, maxState + 1),
				0,
				(state, maximumToAdd) => Enumerable.Range(1, maximumToAdd).Select(toAdd => Math.Min(maxState, state + toAdd)).Distinct(),
				Enumerable.Range(1, 2).ToList(),
				x => x
			);
			var dfaSpec = nfaTimelessSpec.DeterministicEquivalent(new List<int> {0}, x => x);
			CollectionAssert.AreEquivalent(nfaTimelessSpec.inputDomain, dfaSpec.inputDomain);
			CollectionAssert.AreEquivalent(new [] {0}, dfaSpec.startState);
			CollectionAssert.AreEquivalent(new [] {1, 2, 3}, dfaSpec.outputFunction(new List<int> {1, 2, 3}));
			var instance = dfaSpec.NewInstance();
			CollectionAssert.AreEquivalent(new [] {1, 2}, instance.TransitionedOn(2).Output);
			CollectionAssert.AreEquivalent(new [] {2, 3, 4}, instance.TransitionedOn(2).TransitionedOn(2).Output);
			CollectionAssert.AreEquivalent(new [] {3, 4}, instance.TransitionedOn(2).TransitionedOn(2).TransitionedOn(2).Output);
			Assert.AreEqual(4 + 3 + 1 + 1, dfaSpec.states.Count);
		}
	}
}
