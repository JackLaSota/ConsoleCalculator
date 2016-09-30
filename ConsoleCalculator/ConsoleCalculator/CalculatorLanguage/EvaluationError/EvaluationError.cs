namespace ConsoleCalculator.CalculatorLanguage.EvaluationError {
	public abstract class EvaluationError : UserVisibleError {
		protected EvaluationError (string message) : base(message) {}
	}
}
