using System.Collections.Generic;
using ConsoleCalculator.CalculatorLanguage.LinearExpressionsInX;
using ConsoleCalculator.Parser;
using ConsoleCalculator.Parser.Language;

namespace ConsoleCalculator.CalculatorLanguage {
	public partial class Language {
		public static SemanticTreeNode LexemeSemantics (Lexeme lexeme) {//todo test that this covers all included tokens.
			var token = lexeme.token;
			if (token == equalsToken) return new SemanticTreeNode.MeaninglessAloneToken(token);
			if (token == plusToken) return new SemanticTreeNode.MeaninglessAloneToken(token);
			if (token == minusToken) return new SemanticTreeNode.MeaninglessAloneToken(token);
			if (token == multiplyToken) return new SemanticTreeNode.MeaninglessAloneToken(token);
			if (token == divideToken) return new SemanticTreeNode.MeaninglessAloneToken(token);
			if (token == leftParenthesisToken) return new SemanticTreeNode.MeaninglessAloneToken(token);
			if (token == rightParenthesisToken) return new SemanticTreeNode.MeaninglessAloneToken(token);
			if (token == logToken) return new SemanticTreeNode.MeaninglessAloneToken(token);
			if (token == literalToken) return new NonnegativeLiteral(ParseLiteral(lexeme.text));
			throw new LanguageDescriptionException("ParseLexeme method is incomplete. It does not handle lexeme: " + lexeme + ".");
		}
		static LinearInX ParseLiteral (string text) {
			if (text == "x") return new LinearInX {a = 1, b = 0};
			return new LinearInX {a = 0, b = float.Parse(text)};
		}
		/// <summary> Precondition: expansion is valid to generate those productSemantics. This does not check that. </summary>
		public static SemanticTreeNode NonterminalSemantics (CfgProduction production, List<SemanticTreeNode> productSemantics) {//todo test that this covers all included tokens.
			if (production == startToEquation)
				return productSemantics[0];
			if (production == startToExpression)
				return productSemantics[0];
			if (production == expandEquation)
				return new LinearEquationInX((LinearExpressionInX) productSemantics[0], (LinearExpressionInX) productSemantics[2]);
			if (production == expressionToSum)
				return new Sum((LinearExpressionInX) productSemantics[0], (LinearExpressionInX) productSemantics[2]);
			if (production == expressionToDifference)
				return new Difference((LinearExpressionInX) productSemantics[0], (LinearExpressionInX) productSemantics[2]);
			if (production == expressionToNegatableTerm)
				return productSemantics[0];
			if (production == termToParenthesizedExpression)
				return productSemantics[1];
			if (production == termToLiteral)
				return productSemantics[0];
			if (production == termToExplicitProductWithLiteral)
				return new Product((LinearExpressionInX) productSemantics[0], (LinearExpressionInX) productSemantics[2]);
			if (production == termToDivisionWithLiteral)
				return new Division((LinearExpressionInX) productSemantics[0], (LinearExpressionInX) productSemantics[2]);
			if (production == termToExplicitProductWithNegatableParenthesizedExpression)
				return new Product((LinearExpressionInX) productSemantics[0], (LinearExpressionInX) productSemantics[2]);
			if (production == termToDivisionWithNegatableParenthesizedExpression)
				return new Division((LinearExpressionInX) productSemantics[0], (LinearExpressionInX) productSemantics[2]);
			if (production == termToImplicitProductWithLiteral)
				return new Product((LinearExpressionInX) productSemantics[0], (LinearExpressionInX) productSemantics[1]);
			if (production == termToImplicitProductWithParenthesizedExpression)
				return new Product((LinearExpressionInX) productSemantics[0], (LinearExpressionInX) productSemantics[2]);
			if (production == termToLog)
				return new Base10Logarithm((LinearExpressionInX) productSemantics[2]);
			if (production == foregoParenthesizedExpressionNegation)
				return productSemantics[1];
			if (production == negateParenthesizedExpression)
				return new Negation((LinearExpressionInX) productSemantics[1]);
			if (production == foregoTermNegation)
				return productSemantics[0];
			if (production == negateTerm)
				return new Negation((LinearExpressionInX) productSemantics[1]);
			if (production == foregoLiteralNegation)
				return productSemantics[0];
			if (production == negateLiteral)
				return new Negation((LinearExpressionInX) productSemantics[1]);
			throw new LanguageDescriptionException("ParseLexeme method is incomplete. It does not handle production: " + production + ".");
		}
	}
}
