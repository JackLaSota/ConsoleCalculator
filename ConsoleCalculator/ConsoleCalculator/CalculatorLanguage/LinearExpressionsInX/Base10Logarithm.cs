using System;
using ConsoleCalculator.CalculatorLanguage.EvaluationError;

namespace ConsoleCalculator.CalculatorLanguage.LinearExpressionsInX {
	public class Base10Logarithm : LinearExpressionInX {
		public readonly LinearExpressionInX argument;
		public Base10Logarithm (LinearExpressionInX argument) {this.argument = argument;}
		public override LinearInX ComputeValue () {
			var argumentValue = argument.ComputeValue();
			if (argumentValue.a != 0) throw new NonlinearResultError();
			return new LinearInX {a = 0, b = (float) Math.Log10(argumentValue.b)};
		}
		public override string ToString () => "log(" + argument + ")";
	}
}
