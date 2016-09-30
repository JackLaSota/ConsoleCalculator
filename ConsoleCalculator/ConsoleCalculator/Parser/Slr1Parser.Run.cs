using System;
using System.Collections.Generic;
using ConsoleCalculator.Parser.Language;

namespace ConsoleCalculator.Parser {
	public partial class Slr1Parser <TSemanticTreeNode> {
		class Run {
			readonly Slr1Parser<TSemanticTreeNode> parser;
			readonly List<StackEntry> stack;
			IEnumerable<Lexeme> lexedInput;
			public Run (Slr1Parser<TSemanticTreeNode> parser, IEnumerable<Lexeme> lexedInput) {
				this.parser = parser;
				this.lexedInput = lexedInput;
				throw new NotImplementedException();
				//stack = new List<StackEntry> {new StackEntry(parser.dfaSpec.NewInstance(), null, )};
			}
			public void Shift (Lexeme lexeme) {throw new NotImplementedException();}
			public void ReduceBy (CfgProduction production) {throw new NotImplementedException();}
			public TSemanticTreeNode Execute () {
				var inputStream = lexedInput.GetEnumerator();
				try {
					while (inputStream.MoveNext()) {
						throw new NotImplementedException();
					}
					throw new NotImplementedException();
				}
				finally {inputStream.Dispose();}
			}
		}
	}
}
