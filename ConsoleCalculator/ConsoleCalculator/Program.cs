using System;

namespace ConsoleCalculator {
	static class Program {
		static void Main () {
			//Console.WriteLine("Preparing parser...");
			var calculator = new ConsoleCalculator();
			//Console.WriteLine("done.");
			Console.WriteLine(ConsoleCalculator.instructions);
			calculator.Repl();
		}
	}
}
