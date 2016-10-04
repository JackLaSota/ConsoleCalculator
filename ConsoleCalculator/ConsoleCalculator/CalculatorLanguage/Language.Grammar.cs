using ConsoleCalculator.Parser.Language;

namespace ConsoleCalculator.CalculatorLanguage {
	public partial class Language {
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		//Order of declaration of fields in this part of the class is important because of dependencies in static initialization.
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		[IncludedInGrammar] public static readonly Nonterminal startSymbol = new Nonterminal {name = "start"};
		[IncludedInGrammar] public static readonly Nonterminal equationSymbol = new Nonterminal {name = "equation"};
		[IncludedInGrammar] public static readonly Nonterminal expressionSymbol = new Nonterminal {name = "expression"};
		[IncludedInGrammar] public static readonly Nonterminal termSymbol = new Nonterminal {name = "term"};
		[IncludedInGrammar] public static readonly Nonterminal negatableParenthesizedExpressionSymbol = new Nonterminal {name = "negatable parenthesized expression"};
		[IncludedInGrammar] public static readonly Nonterminal negatableTermSymbol = new Nonterminal {name = "negatable term"};
		[IncludedInGrammar] public static readonly Nonterminal negatableLiteralSymbol = new Nonterminal {name = "negatable literal"};
		[IncludedInGrammar] public static readonly Token equalsToken = new Token {name = "equals"};
		[IncludedInGrammar] public static readonly Token plusToken = new Token {name = "plus"};
		[IncludedInGrammar] public static readonly Token minusToken = new Token {name = "minus"};
		[IncludedInGrammar] public static readonly Token multiplyToken = new Token {name = "multiply"};
		[IncludedInGrammar] public static readonly Token divideToken = new Token {name = "divide"};
		[IncludedInGrammar] public static readonly Token leftParenthesisToken = new Token {name = "left parenthesis"};
		[IncludedInGrammar] public static readonly Token rightParenthesisToken = new Token {name = "right parenthesis"};
		[IncludedInGrammar] public static readonly Token logToken = new Token {name = "log"};
		[IncludedInGrammar] public static readonly Token literalToken = new Token {name = "literal"};
		[IncludedInGrammar] public static readonly CfgProduction startToEquation = new CfgProduction(startSymbol, equationSymbol);
		[IncludedInGrammar] public static readonly CfgProduction startToExpression = new CfgProduction(startSymbol, expressionSymbol);
		[IncludedInGrammar] public static readonly CfgProduction expandEquation = new CfgProduction(equationSymbol, expressionSymbol, equalsToken, expressionSymbol);
		[IncludedInGrammar] public static readonly CfgProduction expressionToSum = new CfgProduction(expressionSymbol, negatableTermSymbol, plusToken, expressionSymbol);
		[IncludedInGrammar] public static readonly CfgProduction expressionToDifference = new CfgProduction(expressionSymbol, negatableTermSymbol, minusToken, expressionSymbol);
		[IncludedInGrammar] public static readonly CfgProduction expressionToNegatableTerm = new CfgProduction(expressionSymbol, negatableTermSymbol);
		[IncludedInGrammar] public static readonly CfgProduction termToParenthesizedExpression = new CfgProduction(termSymbol, leftParenthesisToken, expressionSymbol, rightParenthesisToken);
		[IncludedInGrammar] public static readonly CfgProduction termToLiteral = new CfgProduction(termSymbol, literalToken);
		[IncludedInGrammar] public static readonly CfgProduction termToExplicitProductWithLiteral = new CfgProduction(termSymbol, termSymbol, multiplyToken, negatableLiteralSymbol);
		[IncludedInGrammar] public static readonly CfgProduction termToDivisionWithLiteral = new CfgProduction(termSymbol, termSymbol, divideToken, negatableLiteralSymbol);
		[IncludedInGrammar] public static readonly CfgProduction termToExplicitProductWithNegatableParenthesizedExpression = new CfgProduction(termSymbol, termSymbol, multiplyToken, negatableParenthesizedExpressionSymbol);
		[IncludedInGrammar] public static readonly CfgProduction termToDivisionWithNegatableParenthesizedExpression = new CfgProduction(termSymbol, termSymbol, divideToken, negatableParenthesizedExpressionSymbol);
		[IncludedInGrammar] public static readonly CfgProduction termToImplicitProductWithLiteral = new CfgProduction(termSymbol, termSymbol, literalToken);
		[IncludedInGrammar] public static readonly CfgProduction termToImplicitProductWithParenthesizedExpression = new CfgProduction(termSymbol, termSymbol, leftParenthesisToken, expressionSymbol, rightParenthesisToken);
		[IncludedInGrammar] public static readonly CfgProduction termToLog = new CfgProduction(termSymbol, logToken, leftParenthesisToken, expressionSymbol, rightParenthesisToken);
		[IncludedInGrammar] public static readonly CfgProduction foregoParenthesizedExpressionNegation = new CfgProduction(negatableParenthesizedExpressionSymbol, leftParenthesisToken, expressionSymbol, rightParenthesisToken);
		[IncludedInGrammar] public static readonly CfgProduction negateParenthesizedExpression = new CfgProduction(negatableParenthesizedExpressionSymbol, minusToken, negatableParenthesizedExpressionSymbol);
		[IncludedInGrammar] public static readonly CfgProduction foregoTermNegation = new CfgProduction(negatableTermSymbol, termSymbol);
		[IncludedInGrammar] public static readonly CfgProduction negateTerm = new CfgProduction(negatableTermSymbol, minusToken, negatableTermSymbol);
		[IncludedInGrammar] public static readonly CfgProduction foregoLiteralNegation = new CfgProduction(negatableLiteralSymbol, literalToken);
		[IncludedInGrammar] public static readonly CfgProduction negateLiteral = new CfgProduction(negatableLiteralSymbol, minusToken, negatableLiteralSymbol);
	}
}
