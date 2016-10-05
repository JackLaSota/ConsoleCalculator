using ConsoleCalculator.Parser.Language;
using NUnit.Framework;

namespace ConsoleCalculator.Parser {
	public partial struct Lr0Item {
		[TestFixture] public class Tests {
			static Nonterminal start = new Nonterminal {name = "S"};
			static Nonterminal expression = new Nonterminal {name = "E"};
			static CfgProduction startToExpression = new CfgProduction(start, expression);
			[Test] public void ContinuationTest () {
				Assert.AreEqual(null, new Lr0Item(startToExpression, 0).Continuation);
			}
			[Test] public void NextSymbolTest () {
				Assert.AreEqual(expression, new Lr0Item(startToExpression, 0).NextSymbol);
			}
		}
	}
}
