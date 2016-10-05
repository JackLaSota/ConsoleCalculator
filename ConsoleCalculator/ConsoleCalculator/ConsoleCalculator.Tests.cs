using NUnit.Framework;

namespace ConsoleCalculator {
	public partial class ConsoleCalculator {
		[TestFixture] public class Tests {
			[Test] public void ConstructTest () {
				new ConsoleCalculator();
			}
		}
	}
}
