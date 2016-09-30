namespace ConsoleCalculator.Parser.ParseError {
	public class UnexpectedLexemeError : ParseError {
		public UnexpectedLexemeError (string message) : base(message) {}
	}
}
