using System;
using ConsoleCalculator.Parser.Language;
using JetBrains.Annotations;

namespace ConsoleCalculator.Parser {
	public partial struct Lr0Item {
		public readonly CfgProduction cfgProduction;
		public readonly int nextProductIndex;
		public Lr0Item (CfgProduction cfgProduction, int nextProductIndex) {
			if (nextProductIndex < 0 || nextProductIndex > cfgProduction.product.Count)
				throw new ArgumentOutOfRangeException();
			this.cfgProduction = cfgProduction;
			this.nextProductIndex = nextProductIndex;
		}
		public Symbol NextSymbol => nextProductIndex >= cfgProduction.product.Count ? null : cfgProduction.product[nextProductIndex];
		public Lr0Item? Continuation => NextSymbol == null ? (Lr0Item?) null : new Lr0Item(cfgProduction, nextProductIndex + 1);
		public Lr0Item? ContinuationOn (Symbol input) {
			if (NextSymbol != input) return null;
			return Continuation;
		}
		[Pure] public static bool operator == (Lr0Item left, Lr0Item right) {
			return left.cfgProduction == right.cfgProduction && left.nextProductIndex == right.nextProductIndex;
		}
		[Pure] public static bool operator != (Lr0Item left, Lr0Item right) {return !(left == right);}
		// ReSharper disable once UnusedMember.Global
		[Pure] public bool Equals (Lr0Item other) {return this == other;}
		[Pure] public override bool Equals (object o) {
			if (!(o is Lr0Item))
				return false;
			return this == (Lr0Item) o;
		}
		[Pure] public override int GetHashCode () {return 19381 * cfgProduction.GetHashCode() + 73459 * nextProductIndex.GetHashCode();}
		public bool CanBeCompletedBy (Symbol symbol) {return nextProductIndex == cfgProduction.product.Count && NextSymbol == symbol;}
		public bool Complete => nextProductIndex == cfgProduction.product.Count;
		public override string ToString () {
			var before = cfgProduction.product.GetRange(0, nextProductIndex);
			var after = cfgProduction.product.GetRange(nextProductIndex, cfgProduction.product.Count - nextProductIndex);
			return cfgProduction.reagent + " -> " + string.Join("", before) + "." + string.Join("", after) + "";
		}
	}
}
