namespace ConsoleCalculator.CalculatorLanguage.EvaluationError {
	public class EquationIsContradictionError : NoUniqueValueError {
		public EquationIsContradictionError () : base("Equation is true for no values of X.") {}
	}
}
