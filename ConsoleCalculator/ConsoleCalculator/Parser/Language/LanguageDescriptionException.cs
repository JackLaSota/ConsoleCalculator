using System;

namespace ConsoleCalculator.Parser.Language {
	public class LanguageDescriptionException : Exception {
		public LanguageDescriptionException (string message) : base("Division by zero.") {}
	}
}
