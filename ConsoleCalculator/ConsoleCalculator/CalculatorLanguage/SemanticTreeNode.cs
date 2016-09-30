using ConsoleCalculator.Parser.Language;

namespace ConsoleCalculator.CalculatorLanguage {
	/// <summary> Happens to correspond to a symbol in the grammar, but does not know about the grammar. </summary>
	public abstract class SemanticTreeNode {
		public class MeaninglessAloneToken : SemanticTreeNode {
			public readonly Token token;
			public MeaninglessAloneToken (Token token) {this.token = token;}
		}
	}
}
