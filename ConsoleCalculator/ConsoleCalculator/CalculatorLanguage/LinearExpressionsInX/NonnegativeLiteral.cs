namespace ConsoleCalculator.CalculatorLanguage.LinearExpressionsInX {
	/// <summary> Nonnegative as in there is no minus sign. "x" would be one of these even if included in an equation that solved to x = -1. </summary>
	public class NonnegativeLiteral : LinearExpressionInX {
		public readonly LinearInX value;
		public NonnegativeLiteral (LinearInX value) {this.value = value;}
		public override LinearInX ComputeValue () {return value;}
	}
}
