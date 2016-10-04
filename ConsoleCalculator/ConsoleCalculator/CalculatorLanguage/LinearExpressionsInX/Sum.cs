namespace ConsoleCalculator.CalculatorLanguage.LinearExpressionsInX {
	public class Sum : LinearExpressionInX {
		public readonly LinearExpressionInX left;
		public readonly LinearExpressionInX right;
		public Sum (LinearExpressionInX left, LinearExpressionInX right) {
			this.left = left;
			this.right = right;
		}
		public override LinearInX ComputeValue () {return left.ComputeValue() + right.ComputeValue();}
		public override string ToString () => "(" + left + " + " + right + ")";
	}
}
