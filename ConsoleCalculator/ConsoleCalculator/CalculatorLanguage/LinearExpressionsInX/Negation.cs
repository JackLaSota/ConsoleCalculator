namespace ConsoleCalculator.CalculatorLanguage.LinearExpressionsInX {
	public class Negation : LinearExpressionInX {
		public readonly LinearExpressionInX negated;
		public Negation (LinearExpressionInX negated) {this.negated = negated;}
		public override LinearInX ComputeValue () {return -negated.ComputeValue();}
		public override string ToString () => "(-" + negated + ")";
	}
}
