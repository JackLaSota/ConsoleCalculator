namespace ConsoleCalculator.CalculatorLanguage.EvaluationError {
	public abstract class NoUniqueValueError : UserVisibleError {
		protected NoUniqueValueError (string message) : base(message) {}
	}
}
