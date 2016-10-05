using System.Linq;
using ConsoleCalculator.CalculatorLanguage.LexError;
using NUnit.Framework;

namespace ConsoleCalculator.CalculatorLanguage {
	public partial class Language {
		[TestFixture] public class Tests {
			[Test] public void ChunkTest () {
				CollectionAssert.AreEqual(new [] {"2"}, Chunked("2").ToArray());
				CollectionAssert.AreEqual(new string[] {}, Chunked(""));
				CollectionAssert.AreEqual(new [] {"2", " ", "asdf"}, Chunked("2 asdf"));
				CollectionAssert.AreEqual(new [] {"2", "asdf"}, Chunked("2asdf"));
				CollectionAssert.AreEqual(new [] {"asdf", "2"}, Chunked("asdf2"));
				CollectionAssert.AreEqual(new [] {"asdf", " ", "2"}, Chunked("asdf 2"));
				CollectionAssert.AreEqual(new [] {"asdf", " ", "2.2",  "a"}, Chunked("asdf 2.2a"));
				CollectionAssert.AreEqual(new [] {"2.", " ", "2",  "a"}, Chunked("2. 2a"));
			}
			[Test] public void IsLegalWordTest () {
				Assert.AreEqual(false, IsLegalWord(""));
				Assert.AreEqual(false, IsLegalWord("a"));
				Assert.AreEqual(false, IsLegalWord("go"));
				Assert.AreEqual(false, IsLegalWord("2"));
				Assert.AreEqual(true, IsLegalWord("x"));
				Assert.AreEqual(true, IsLegalWord("log"));
			}
			[Test] public void ScreenedForIllegalWordsTest () {
				// ReSharper disable ReturnValueOfPureMethodIsNotUsed
				Assert.Throws<IllegalWordError>(() => ScreenedForIllegalWords(new[] {"ll"}).ToList());
				ScreenedForIllegalWords(new[] {"log", "x", "123", " ", "-"}).ToList();
				// ReSharper restore ReturnValueOfPureMethodIsNotUsed
			}
			[Test] public void IsLegalNumericLiteralTest () {
				Assert.AreEqual(false, IsLegalNumericLiteral(""));
				Assert.AreEqual(true, IsLegalNumericLiteral("1"));
				Assert.AreEqual(true, IsLegalNumericLiteral("1."));
				Assert.AreEqual(true, IsLegalNumericLiteral(".1"));
				Assert.AreEqual(true, IsLegalNumericLiteral("1.1"));
				Assert.AreEqual(false, IsLegalNumericLiteral("1.1."));
				Assert.AreEqual(false, IsLegalNumericLiteral(".1.1"));
			}
			[Test] public void ScreenedForMalformedNumericLiteralsTest () {
				// ReSharper disable ReturnValueOfPureMethodIsNotUsed
				ScreenedForMalformedNumericLiterals(new [] {"1", "1.", "1.1", ".1", "a", " ", "-"}).ToList();
				Assert.Throws<MalformedNumericLiteralError>(() => ScreenedForMalformedNumericLiterals(new[] {".1.1"}).ToList());
				// ReSharper restore ReturnValueOfPureMethodIsNotUsed
			}
			[Test] public void ScreenedForIllegalCharactersTest () {
				// ReSharper disable ReturnValueOfPureMethodIsNotUsed
				ScreenedForIllegalCharacters("log(x) + 1 - 2 / 4 * (3.)").ToList();
				Assert.Throws<IllegalCharacterError>(() => ScreenedForIllegalCharacters(",").ToList());
				// ReSharper restore ReturnValueOfPureMethodIsNotUsed
			}
			[Test] public void LexCorrectTokensTest () {
				CollectionAssert.AreEqual(
					new [] {
						minusToken, plusToken, minusToken, plusToken, literalToken, literalToken, logToken, literalToken,
						leftParenthesisToken, leftParenthesisToken, multiplyToken, divideToken, divideToken, multiplyToken
					},
					Lex("-+ - + .2 1 log x ( ( */ /*").Select(lexeme => lexeme.token)
				);
			}
			[Test] public void LexCorrectTextTest () {
				CollectionAssert.AreEqual(
					new [] {"log", "(", "1.1", "x", ")"},
					Lex("log(1.1x)").Select(lexeme => lexeme.text)
				);
			}
		}
	}
}
