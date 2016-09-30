using ConsoleCalculator.Parser.Language;

namespace ConsoleCalculator.Parser {
	public class Lexeme {
		public readonly Token token;
		public string text;
		public Lexeme (Token token, string text) {this.token = token; this.text = text;}
	}
}
