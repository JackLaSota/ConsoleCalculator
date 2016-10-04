using ConsoleCalculator.CalculatorLanguage.EvaluationError;

namespace ConsoleCalculator.CalculatorLanguage.LinearExpressionsInX {
	public class Division : LinearExpressionInX {
		public readonly LinearExpressionInX left;
		public readonly LinearExpressionInX right;
		public Division (LinearExpressionInX left, LinearExpressionInX right) {
			this.left = left;
			this.right = right;
		}
		public override LinearInX ComputeValue () {
			var leftValue = left.ComputeValue();
			var rightValue = right.ComputeValue();
			if (rightValue.a != 0) throw new NonlinearResultError();
			if (rightValue.b == 0) throw new DivisionByZeroError();
			return new LinearInX {a = leftValue.a / rightValue.b, b = leftValue.b / rightValue.b};
		}
		public override string ToString () => "(" + left + " - " + right + ")";
	}
}
