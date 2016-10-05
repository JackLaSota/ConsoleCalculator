using System.Linq;
using NUnit.Framework;

namespace ConsoleCalculator.Parser.Language {
	public partial class Cfg {
		[TestFixture] public class Tests {
			static Nonterminal start = new Nonterminal {name = "S"};
			static Token aToken = new Token {name = "a"};
			static Token bToken = new Token {name = "b"};
			static Nonterminal xNonterminal = new Nonterminal {name = "x"};
			static Nonterminal yNonterminal = new Nonterminal {name = "y"};
			static CfgProduction startToX = new CfgProduction(start, xNonterminal);
			static CfgProduction xToY = new CfgProduction(xNonterminal, yNonterminal);
			static CfgProduction yToYandA = new CfgProduction(yNonterminal, yNonterminal, aToken);
			static CfgProduction yToBYandA = new CfgProduction(yNonterminal, bToken, yNonterminal, aToken);
			static CfgProduction yToB = new CfgProduction(yNonterminal, bToken);
			static Symbol[] symbols = {start, aToken, bToken, xNonterminal, yNonterminal};
			static Cfg cfg = new Cfg(symbols, start, new[] {startToX, xToY, yToYandA, yToBYandA, yToB});
			[Test] public void SymbolsThatCanDirectlyFollowTest () {
				CollectionAssert.AreEquivalent(new Symbol [] {}, cfg.SymbolsThatCanDirectlyFollow(xNonterminal));
				CollectionAssert.AreEquivalent(new [] {aToken}, cfg.SymbolsThatCanDirectlyFollow(yNonterminal));
			}
			[Test] public void SymbolsThatCanComeFirstInOneExpansionOfTest () {
				CollectionAssert.AreEquivalent(new [] {yNonterminal}, cfg.SymbolsThatCanComeFirstInOneExpansionOf(xNonterminal));
				CollectionAssert.AreEquivalent(new Symbol[] {yNonterminal, bToken}, cfg.SymbolsThatCanComeFirstInOneExpansionOf(yNonterminal));
			}
			[Test] public void TokensThatCanComeFirstInExpansionOfTest () {
				CollectionAssert.AreEquivalent(
					cfg.TokensThatCanComeFirstInExpansionOf(yNonterminal),
					cfg.TokensThatCanComeFirstInExpansionOf(xNonterminal)
				);
				CollectionAssert.AreEquivalent(
					new [] {bToken},
					cfg.TokensThatCanComeFirstInExpansionOf(yNonterminal)
				);
			}
			static CfgProduction xToXAndY = new CfgProduction(xNonterminal, xNonterminal, yNonterminal);
			static Cfg withRecursingX = new Cfg(symbols, start, new[] {startToX, xToY, xToXAndY, yToYandA, yToBYandA, yToB});
			[Test] public void TokensThatCanFollowTest () {
				CollectionAssert.AreEquivalent(new Symbol [] {}, cfg.TokensThatCanFollow(xNonterminal));
				CollectionAssert.AreEquivalent(new [] {aToken}, cfg.TokensThatCanFollow(yNonterminal));
				CollectionAssert.AreEquivalent(new [] {bToken}, withRecursingX.TokensThatCanFollow(xNonterminal));
			}
		}
	}
}
