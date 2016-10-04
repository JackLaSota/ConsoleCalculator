using ConsoleCalculator.CalculatorLanguage.EvaluationError;
using ConsoleCalculator.CalculatorLanguage.LinearExpressionsInX;

namespace ConsoleCalculator.CalculatorLanguage {
	public partial class LinearEquationInX : SemanticTreeNode {
		public readonly LinearExpressionInX right;
		public readonly LinearExpressionInX left;
		public LinearEquationInX (LinearExpressionInX left, LinearExpressionInX right) {
			this.left = left;
			this.right = right;
		}
		public float ComputeUniquelyDeterminedX () {
			var rightValue = right.ComputeValue();
			var leftValue = left.ComputeValue();
			if (rightValue.a == leftValue.a) {
				if (rightValue.b == leftValue.b)
					throw new EquationIsTautologyError();
				throw new EquationIsContradictionError();
			}
			leftValue.a -= rightValue.a; rightValue.a -= rightValue.a;
			leftValue.b -= leftValue.b; rightValue.b -= leftValue.b;
			return rightValue.b / leftValue.a;
		}
		public override string ToString () => left + "=" + right;
	}
}
