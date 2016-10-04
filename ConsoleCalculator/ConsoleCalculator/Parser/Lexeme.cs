using ConsoleCalculator.Parser.Language;

namespace ConsoleCalculator.Parser {
	public class Lexeme {
		public readonly Token token;
		public readonly string text;
		public Lexeme (Token token, string text) {this.token = token; this.text = text;}
		public override string ToString () {return token + ": \"" + text + "\"";}
	}
}
