using System;

namespace ConsoleCalculator.Parser.ParserSetupException {
	public abstract class ParserSetupException : Exception {
		protected ParserSetupException (string message) : base(message) {}
	}
}
