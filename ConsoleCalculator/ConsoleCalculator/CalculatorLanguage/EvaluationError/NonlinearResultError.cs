namespace ConsoleCalculator.CalculatorLanguage.EvaluationError {
	public class NonlinearResultError : EvaluationError {
		public NonlinearResultError () : base("Cannot handle nonlinear expressions or equations in X.") {}
	}
}
