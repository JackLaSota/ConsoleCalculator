using ConsoleCalculator.CalculatorLanguage.EvaluationError;

namespace ConsoleCalculator.CalculatorLanguage.LinearExpressionsInX {
	public class Product : LinearExpressionInX {
		public readonly LinearExpressionInX left;
		public readonly LinearExpressionInX right;
		public Product (LinearExpressionInX left, LinearExpressionInX right) {
			this.left = left;
			this.right = right;
		}
		public override LinearInX ComputeValue () {
			var leftValue = left.ComputeValue();
			var rightValue = right.ComputeValue();
			if (leftValue.a != 0 && rightValue.a != 0)
				throw new NonlinearResultError();
			return new LinearInX {
				a = leftValue.a * rightValue.b + leftValue.b * rightValue.a,
				b = leftValue.b * rightValue.b
			};
		}
	}
}
