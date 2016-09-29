namespace ConsoleCalculator.CalculatorLanguage.LinearExpressionsInX {
	public abstract class LinearExpressionInX : SemanticTreeNode {
		/// <summary> Can throw EvaluationException. </summary>
		public abstract LinearInX ComputeValue ();
	}
}
