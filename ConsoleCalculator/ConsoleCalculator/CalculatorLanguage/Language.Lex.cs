using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleCalculator.CalculatorLanguage.LexError;
using ConsoleCalculator.Parser;
using ConsoleCalculator.Parser.Language;

namespace ConsoleCalculator.CalculatorLanguage {
	public static partial class Language {
		static HashSet<char> legalCharacters = new HashSet<char> {
			' ',
			'=',
			'(', ')',
			'+', '-', '*', '/',
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'.',
			'x',
			'l', 'o', 'g'
		};
		static bool IsLegalCharacter (char character) {return legalCharacters.Contains(character);}
		static bool IsLegalWord (string word) {return word == "log" || word == "x";}
		static bool IsLegalNumericLiteral (string literal) {return literal != "" && literal.Count(c => c == '.') <= 1 && literal.All(IsNumberPart);}
		/// <summary> Important note: unary "-" is an operator and not part of a numeric literal in this language. </summary>
		static bool IsNumberPart (char character) {return char.IsDigit(character) || character == '.';}
		static IEnumerable<string> ScreenedForIllegalWords (IEnumerable<string> chunkedInput) {
			foreach (var wordOrNonAlphabetic in chunkedInput) {
				if (char.IsLetter(wordOrNonAlphabetic[0]) && !IsLegalWord(wordOrNonAlphabetic))
					throw new IllegalWordError("Illegal word: " + wordOrNonAlphabetic);
				yield return wordOrNonAlphabetic;
			}
		}
		static IEnumerable<char> ScreenedForIllegalCharacters (IEnumerable<char> input) {
			foreach (var character in input) {
				if (!IsLegalCharacter(character))
					throw new IllegalCharacterError("Illegal character: " + character);
				yield return character;
			}
		}
		static IEnumerable<string> ScreenedForMalformedNumericLiterals (IEnumerable<string> chunkedInput) {
			foreach (var possibleNumericLiteral in chunkedInput) {
				if (IsNumberPart(possibleNumericLiteral[0]) && !IsLegalNumericLiteral(possibleNumericLiteral))
					throw new MalformedNumericLiteralError("Malformed numeric literal: " + possibleNumericLiteral);
				yield return possibleNumericLiteral;
			}
		}
		/// <summary> This will group together all contiguous characters that need to be in the same lexeme. </summary>
		static IEnumerable<string> Chunked (IEnumerable<char> input) {
			var accumulated = new List<char>();
			var takeAccumulatedChunk = (Func<string>) (() => {
				var chunk = string.Join("", accumulated);
				accumulated.Clear();
				return chunk == "" ? null : chunk;
			});
			foreach (var character in input) {
				var isLetter = char.IsLetter(character);
				var isNumberPart = IsNumberPart(character);
				var isGroupable = isLetter || isNumberPart;
				var canExtend = isGroupable && accumulated.Count != 0
					&& (isLetter && char.IsLetter(accumulated[0]) || isNumberPart && IsNumberPart(accumulated[0]));
				if (!canExtend) {
					var justFinishedChunk = takeAccumulatedChunk();
					if (justFinishedChunk != null)
						yield return justFinishedChunk;
					if (!isGroupable)
						yield return "" + character;
				}
				if (isGroupable)
					accumulated.Add(character);
			}
			var lastChunk = takeAccumulatedChunk();
			if (lastChunk != null)
				yield return lastChunk;
		}
		public static IEnumerable<Lexeme> Lex (IEnumerable<char> input) {
			var screenedAndChunked = ScreenedForMalformedNumericLiterals(ScreenedForIllegalWords(Chunked(ScreenedForIllegalCharacters(input))));
			return screenedAndChunked.Where(chunk => chunk != " ").Select(chunk => {
				var makeLexeme = (Func<Token, Lexeme>) (token => new Lexeme(token, chunk));
				if (chunk == "=") return makeLexeme(equalsToken);
				if (chunk == "+") return makeLexeme(plusToken);
				if (chunk == "-") return makeLexeme(minusToken);
				if (chunk == "*") return makeLexeme(multiplyToken);
				if (chunk == "/") return makeLexeme(divideToken);
				if (chunk == "(") return makeLexeme(leftParenthesisToken);
				if (chunk == ")") return makeLexeme(rightParenthesisToken);
				if (chunk == "log") return makeLexeme(logToken);
				return makeLexeme(literalToken);
			});
		}
	}
}
