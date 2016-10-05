using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleCalculator.Parser.Language;
using ConsoleCalculator.Utilities;
using NUnit.Framework;

namespace ConsoleCalculator.Parser {
	[TestFixture] public class Slr1ParserTests {
		static Nonterminal startSymbol = new Nonterminal {name = "S"};
		static Token aToken = new Token {name = "a"};
		static CfgProduction startSymbolToAToken = new CfgProduction(startSymbol, aToken);
		static Cfg mostTrivialGrammar = new Cfg(new Symbol[] {startSymbol, aToken}, startSymbol, new [] {startSymbolToAToken});
		class ExampleSemanticNode {}
		[Test] public void FirstItemTest () {
			Assert.AreEqual(
				new Lr0Item(startSymbolToAToken, 0),
				Slr1Parser<ExampleSemanticNode>.FirstItemFor(startSymbolToAToken)
			);
		}
		[Test] public void StartItemsForTest () {
			CollectionAssert.AreEquivalent(
				new [] {new Lr0Item(startSymbolToAToken, 0)},
				Slr1Parser<ExampleSemanticNode>.StartItemsFor(mostTrivialGrammar)
			);
			CollectionAssert.AreEquivalent(
				new[] {
					new Lr0Item(startToNeedlessDetour, 0),
					new Lr0Item(startSymbolToAToken, 0)
				},
				Slr1Parser<ExampleSemanticNode>.StartItemsFor(slightlyMoreComplicatedGrammar)
			);
		}
		static Nonterminal needlessDetour = new Nonterminal {name = "needless detour"};
		static CfgProduction startToNeedlessDetour = new CfgProduction(startSymbol, needlessDetour);
		static Token bToken = new Token {name = "b"};
		static CfgProduction needlessDetourToB = new CfgProduction(needlessDetour, bToken);
		static Cfg slightlyMoreComplicatedGrammar = new Cfg(new Symbol [] {startSymbol, aToken, bToken, needlessDetour}, startSymbol, new [] {
			startToNeedlessDetour, needlessDetourToB, startSymbolToAToken
		});
		[Test] public void ItemsForTest () {
			CollectionAssert.AreEquivalent(
				new[] {
					new Lr0Item(startToNeedlessDetour, 0),
					new Lr0Item(startSymbolToAToken, 0),
					new Lr0Item(needlessDetourToB, 0)
				},
				Slr1Parser<ExampleSemanticNode>.ItemsFor(slightlyMoreComplicatedGrammar)
			);
		}
		[Test] public void ReachableWithEmptyTokenStringFromTest () {
			CollectionAssert.AreEquivalent(
				new[] {new Lr0Item(needlessDetourToB, 0)},
				Slr1Parser<ExampleSemanticNode>.ReachableWithEmptyTokenStringFrom(slightlyMoreComplicatedGrammar, new Lr0Item(startToNeedlessDetour, 0))
			);
		}
		static Token leftBracketToken = new Token {name = "["};
		static Token rightBracketToken = new Token {name = "]"};
		static Nonterminal expression = new Nonterminal {name = "E"};
		static CfgProduction startToExpression = new CfgProduction(startSymbol, expression);
		static CfgProduction expressionToBrackets = new CfgProduction(expression, leftBracketToken, rightBracketToken);
		static CfgProduction expressionToBracketedExpression = new CfgProduction(expression, leftBracketToken, expression, rightBracketToken);
		/// <summary> Contains "[]" and "[[]]" but not "[][]" </summary>
		static Cfg nestedBracketsGrammar = new Cfg(new Symbol[] {startSymbol, leftBracketToken, rightBracketToken, expression}, startSymbol, new [] {
			startToExpression, expressionToBrackets, expressionToBracketedExpression
		});
		static IEnumerable<Lexeme> LexBrackets (string input) {
			foreach (var character in input) {
				if (character == '[') yield return new Lexeme(leftBracketToken, "" + character); 
				if (character == ']') yield return new Lexeme(rightBracketToken, "" + character);
			}
		}
		[Test] public void ParseBracketsTransitionFunctionTest () {
			var nfaSpec = Slr1Parser<ExampleSemanticNode>.NfaSpecFor(nestedBracketsGrammar);
			CollectionAssert.IsEmpty(nfaSpec.transitionsFunction(new Lr0Item(expressionToBrackets, 0), startSymbol));
			Assert.AreEqual(expression, Slr1Parser<ExampleSemanticNode>.CompletionsFrom(new List<Lr0Item> {new Lr0Item(startToExpression, 0)}).Single().Item1);
			CollectionAssert.AreEquivalent(
				new [] {
					new Lr0Item(expressionToBrackets, 1),
					new Lr0Item(expressionToBracketedExpression, 1)
				}, nfaSpec.transitionsFunction(new Lr0Item(startToExpression, 0), leftBracketToken));
		}
		[Test] public void ParseBracketsCanShiftTest () {
			var nfaSpec = Slr1Parser<ExampleSemanticNode>.NfaSpecFor(nestedBracketsGrammar);
			foreach (var state in nfaSpec.states) {
				foreach (var input in nfaSpec.inputDomain)
				Console.WriteLine(state + " transitioned on " + input + " goes to " + nfaSpec.transitionsFunction(state, input).ToDetailedString());
			}
			var dfaSpec = Slr1Parser<ExampleSemanticNode>.DfaSpecFor(nestedBracketsGrammar);
			foreach (var state in dfaSpec.states) {
				Console.WriteLine(state.ToDetailedString());
			}
		}
		class Bracketed {
			// ReSharper disable once NotAccessedField.Local
			public readonly Bracketed contents;
			public Bracketed (Bracketed contents) {this.contents = contents;}
		}
		[Test] public void ParseBracketsTest () {
			var parser = new Slr1Parser<Bracketed>(
				nestedBracketsGrammar,
				LexBrackets,
				lexeme => null,
				(production, product) => {
					if (production == startToExpression) return product[0];
					if (production == expressionToBracketedExpression) return new Bracketed(product[0]);
					if (production == expressionToBrackets) return new Bracketed(null);
					throw new Exception();
				}
			);
			// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
			parser.Parse("[]");
		}
		//todo test with a producible start symbol.
		//todo test with a non-slr1 grammar.
	}
}
