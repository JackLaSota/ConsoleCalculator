namespace ConsoleCalculator.Parser.ParseError {
	public abstract class ParseError : UserVisibleError {
		protected ParseError (string message) : base(message) {}
	}
}
