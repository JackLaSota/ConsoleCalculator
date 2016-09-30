using NUnit.Framework;

namespace ConsoleCalculator.CalculatorLanguage {
	public partial class Language {
		[TestFixture] public class Tests {
			[Test] public void ChunkTest () {
				CollectionAssert.AreEqual(new [] {"1"}, Chunked("1"));
			}
		}
	}
}
