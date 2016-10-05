using ConsoleCalculator.Parser.Language;
using NUnit.Framework;

namespace ConsoleCalculator.Parser {
	public partial struct Lr0Item {
		[TestFixture] public class Tests {
			static Nonterminal start = new Nonterminal {name = "S"};
			static Nonterminal expression = new Nonterminal {name = "E"};
			static CfgProduction startToExpression = new CfgProduction(start, expression);
			[Test] public void ContinuationTest () {
				Assert.AreEqual(new Lr0Item(startToExpression, 1), new Lr0Item(startToExpression, 0).Continuation);
				Assert.AreEqual(null, new Lr0Item(startToExpression, 1).Continuation);
			}
			[Test] public void NextSymbolTest () {
				Assert.AreEqual(expression, new Lr0Item(startToExpression, 0).NextSymbol);
				Assert.AreEqual(null, new Lr0Item(startToExpression, 1).NextSymbol);
			}
			[Test] public void CompleteTest () {
				Assert.AreEqual(false, new Lr0Item(startToExpression, 0).Complete);
				Assert.AreEqual(true, new Lr0Item(startToExpression, 1).Complete);
			}
		}
	}
}
