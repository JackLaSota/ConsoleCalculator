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
				Console.WriteLine(Evaluate(input));
			}
		}
		string Evaluate (string input) {
			try {
				var parsedInput = parser.Parse(input);
				var expression = parsedInput as LinearExpressionInX; if (expression != null)
					return expression.ComputeValue().ToString();
				var equation = parsedInput as LinearEquationInX; if (equation != null)
					return "x = " + equation.ComputeUniquelyDeterminedX();
				throw new SystemException("Should not be reachable.");//ncrunch: no coverage
			}
			catch (UserVisibleError error) {
				return error.GetType().Name + ": " + error.Message;
			}
		}
		readonly Slr1Parser<SemanticTreeNode> parser;
		public ConsoleCalculator () {
			parser = Language.CreateParser();
		}
	}
}
