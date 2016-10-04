namespace ConsoleCalculator.CalculatorLanguage.LinearExpressionsInX {
	public class Difference : LinearExpressionInX {
		public readonly LinearExpressionInX left;
		public readonly LinearExpressionInX right;
		public Difference (LinearExpressionInX left, LinearExpressionInX right) {
			this.left = left;
			this.right = right;
		}
		public override LinearInX ComputeValue () {return left.ComputeValue() - right.ComputeValue();}
		public override string ToString () => "(" + left + " - " + right + ")";
	}
}
