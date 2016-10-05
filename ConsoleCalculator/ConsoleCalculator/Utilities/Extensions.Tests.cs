using System.Linq;
using NUnit.Framework;

namespace ConsoleCalculator.Utilities {
	public static partial class Extensions {
		[TestFixture] public class Tests {
			[Test] public void ClosureFromMultipleTest () {
				CollectionAssert.AreEquivalent(
					new [] {0, 1, 2, 3, 4, 5},
					new [] {4, 5}.ClosureFromMultiple(x => new [] {x - 1, x - 2}.Where(y => y >= 0))
				);
				CollectionAssert.AreEquivalent(
					new [] {0, 1, 2, 3, 4},
					new [] {4, 5}.ClosureFromMultiple(x => new [] {x - 1, x - 2}.Where(y => y >= 0), true)
				);
			}
		}
	}
}
