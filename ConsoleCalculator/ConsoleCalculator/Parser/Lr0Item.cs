using System;
using ConsoleCalculator.Parser.Language;
using JetBrains.Annotations;

namespace ConsoleCalculator.Parser {
	public partial class Lr0Item {
		public readonly CfgProduction cfgProduction;
		public readonly int nextProductIndex;
		public Lr0Item (CfgProduction cfgProduction, int nextProductIndex) {
			if (nextProductIndex < 0 || nextProductIndex >= cfgProduction.product.Count)
				throw new ArgumentOutOfRangeException();
			this.cfgProduction = cfgProduction;
			this.nextProductIndex = nextProductIndex;
		}
		public Symbol NextSymbol => cfgProduction.product[nextProductIndex];
		public Lr0Item Continuation => nextProductIndex == cfgProduction.product.Count - 1 ? null : new Lr0Item(cfgProduction, nextProductIndex + 1);
		[Pure] public static bool operator == (Lr0Item left, Lr0Item right) {
			if ((object) left == null || (object) right == null) return (object) left == null && (object) right == null;
			return left.cfgProduction == right.cfgProduction && left.nextProductIndex == right.nextProductIndex;
		}
		[Pure] public static bool operator != (Lr0Item left, Lr0Item right) {return !(left == right);}
		// ReSharper disable once UnusedMember.Global
		[Pure] public bool Equals (Lr0Item other) {return this == other;}
		[Pure] public override bool Equals (object o) {
			var lr0Item = o as Lr0Item;
			return lr0Item != null && this == lr0Item;
		}
		[Pure] public override int GetHashCode () {return 19381 * cfgProduction.GetHashCode() + 73459 * nextProductIndex.GetHashCode();}
		public bool CanBeCompletedBy (Symbol symbol) {return nextProductIndex == cfgProduction.product.Count && NextSymbol == symbol;}
		public override string ToString () {
			var before = cfgProduction.product.GetRange(0, nextProductIndex);
			var after = cfgProduction.product.GetRange(nextProductIndex, cfgProduction.product.Count);
			return cfgProduction.reagent + " = " + string.Join(",", before) + " . " + string.Join(",", after) + ";";
		}
	}
}
