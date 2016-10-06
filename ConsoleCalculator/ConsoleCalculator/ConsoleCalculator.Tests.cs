using NUnit.Framework;

namespace ConsoleCalculator {
	public partial class ConsoleCalculator {
		[TestFixture] public class Tests {
			[Test] public void ConstructTest () {
				new ConsoleCalculator();
			}
			ConsoleCalculator calculator = new ConsoleCalculator();
			[TestCase("(3+(4-1))*5", ExpectedResult = "30")]
			[TestCase("2 * x + 0.5 = 1", ExpectedResult = "x = 0.25")]
			[TestCase("2x + 1 = 2(1-x)", ExpectedResult = "x = 0.25")]
			[TestCase("x", ExpectedResult = "x")]
			[TestCase("2(1+x)", ExpectedResult = "2x + 2")]
			[TestCase("1", ExpectedResult = "1")]
			[TestCase("1+1", ExpectedResult = "2")]
			[TestCase("1-1", ExpectedResult = "0")]
			[TestCase("3 * 4", ExpectedResult = "12")]
			[TestCase("1 / -1", ExpectedResult = "-1")]
			[TestCase("1 * -1", ExpectedResult = "-1")]
			[TestCase("1 - -1", ExpectedResult = "2")]
			[TestCase("1 - -(1)", ExpectedResult = "2")]
			[TestCase("1 - (-1)", ExpectedResult = "2")]
			[TestCase("1 + -1", ExpectedResult = "0")]
			[TestCase("3 / 4", ExpectedResult = "0.75")]
			[TestCase("3 / 4 / 4", ExpectedResult = "0.1875")]
			[TestCase("3 / 4 * 4", ExpectedResult = "3")]
			[TestCase("3 - 4 - 4", ExpectedResult = "-5")]
			[TestCase("1 + 2 3", ExpectedResult = "7")]
			[TestCase("1 +1", ExpectedResult = "2")]
			[TestCase(" 1", ExpectedResult = "1")]
			[TestCase("(1)", ExpectedResult = "1")]
			[TestCase("(((((1)))))", ExpectedResult = "1")]
			[TestCase("(2)1", ExpectedResult = "2")]
			[TestCase("1(2)", ExpectedResult = "2")]
			[TestCase("-1", ExpectedResult = "-1")]
			[TestCase("---1", ExpectedResult = "-1")]
			[TestCase("log(1)", ExpectedResult = "0")]
			[TestCase("1=x", ExpectedResult = "x = 1")]
			[TestCase("1=2x", ExpectedResult = "x = 0.5")]
			[TestCase("3x - 1=2x +2", ExpectedResult = "x = 3")]
			[TestCase("log(-1)", ExpectedResult = "LogOfNonpositiveError: Tried to take the logarithm of a nonpositive number.")]
			[TestCase("log(0)", ExpectedResult = "LogOfNonpositiveError: Tried to take the logarithm of a nonpositive number.")]
			[TestCase("1 / 0", ExpectedResult = "DivisionByZeroError: Division by zero.")]
			[TestCase("x*x", ExpectedResult = "NonlinearResultError: Cannot handle nonlinear expressions or equations in X.")]
			[TestCase("log(x)", ExpectedResult = "NonlinearResultError: Cannot handle nonlinear expressions or equations in X.")]
			[TestCase("1/x", ExpectedResult = "NonlinearResultError: Cannot handle nonlinear expressions or equations in X.")]
			[TestCase("1=1", ExpectedResult = "EquationIsTautologyError: Equation is true for all values of X.")]
			[TestCase("1=0", ExpectedResult = "EquationIsContradictionError: Equation is true for no values of X.")]
			[TestCase("xx", ExpectedResult = "IllegalWordError: Illegal word: xx")]
			[TestCase("a", ExpectedResult = "IllegalCharacterError: Illegal character: a")]
			[TestCase("++x", ExpectedResult = "UnexpectedLexemeError: Unexpected lexeme: +")]
			[TestCase("1 + ", ExpectedResult = "UnexpectedEndOfInputError: Unexpected end of input.")]
			[Test] public string Evaluate (string input) {
				return calculator.Evaluate(input);
			}
		}
	}
}
