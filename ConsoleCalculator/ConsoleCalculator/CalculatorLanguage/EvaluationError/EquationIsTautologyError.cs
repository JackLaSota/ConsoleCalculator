namespace ConsoleCalculator.CalculatorLanguage.EvaluationError {
	public class EquationIsTautologyError : NoUniqueValueError {
		public EquationIsTautologyError () : base("Equation is true for all values of X.") {}
	}
}
