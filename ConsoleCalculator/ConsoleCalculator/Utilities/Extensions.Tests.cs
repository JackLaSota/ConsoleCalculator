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
			[Test] public void ContainsAllTest () {
				Assert.That(new int[] {}.ContainsAll(new int[] {}));
				Assert.That(new [] {1, 2, 3, 4}.ContainsAll(new int[] {}));
				Assert.That(new [] {1, 2, 3, 4}.ContainsAll(new [] {4, 3}));
				Assert.That(!new [] {1, 2, 3, 4}.ContainsAll(new [] {0, 4, 3}));
			}
			[Test] public void AllDistinctTest () {
				Assert.That(new int[] {}.AllDistinct());
				Assert.That(new [] {1, 2, 3}.AllDistinct());
				Assert.That(!new [] {1, 1}.AllDistinct());
			}
		}
	}
}
