using System;

namespace ConsoleCalculator {
	static class Program {
		static void Main () {
			Console.WriteLine(ConsoleCalculator.instructions);
			var calculator = new ConsoleCalculator();
			calculator.Repl();
		}
	}
}
