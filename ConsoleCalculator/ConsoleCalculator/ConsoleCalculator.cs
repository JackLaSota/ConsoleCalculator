using System;
using System.Linq;
using ConsoleCalculator.Parser;
using ConsoleCalculator.CalculatorLanguage;
using ConsoleCalculator.CalculatorLanguage.LinearExpressionsInX;

namespace ConsoleCalculator {
	public partial class ConsoleCalculator {
		public const string instructions = "Enter expressions to evaluate them. Enter linear equations in \"x\" to solve them.";
		/// <summary> Read evaluate print loop. </summary>
		public void Repl () {
			while (true) {
				var input = Console.ReadLine();
				if (input == null) continue;
				if (input == "") continue;
				if (input.All(c => c == ' ')) continue;
				if (input.Contains("exit")) return;
				try {
					var parsedInput = parser.Parse(input);
					var expression = parsedInput as LinearExpressionInX; if (expression != null)
						Console.WriteLine(expression.ComputeValue());
					var equation = parsedInput as LinearEquationInX; if (equation != null)
						Console.WriteLine("x = " + equation.ComputeUniquelyDeterminedX());
				}
				catch (UserVisibleError error) {
					Console.WriteLine(error.GetType().Name + ": " + error.Message);
				}
			}
		}
		readonly Slr1Parser<SemanticTreeNode> parser;
		public ConsoleCalculator () {
			parser = Language.CreateParser();
		}
	}
}
