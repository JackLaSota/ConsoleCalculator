namespace ConsoleCalculator.CalculatorLanguage.EvaluationError {
	public class LogOfNonpositiveError : EvaluationError {
		public LogOfNonpositiveError () : base("Tried to take the logarithm of a nonpositive number.") {}
	}
}
