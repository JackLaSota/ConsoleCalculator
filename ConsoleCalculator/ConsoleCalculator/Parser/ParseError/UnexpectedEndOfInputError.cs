namespace ConsoleCalculator.Parser.ParseError {
	public class UnexpectedEndOfInputError : ParseError {
		public UnexpectedEndOfInputError (string message) : base(message) {}
	}
}
