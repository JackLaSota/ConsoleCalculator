using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleCalculator.Utilities;
using JetBrains.Annotations;

namespace ConsoleCalculator.CalculatorLanguage {
	/// <summary> Represents (ax + b) </summary>
	public partial struct LinearInX {
		/// <summary> Coefficient of x. </summary>
		public float a;
		/// <summary> Constant offset. </summary>
		public float b;
		[Pure] public static explicit operator string (LinearInX toCast) {
			return SimplifiedLinearCombinationExpression(new[] {toCast.a, toCast.b}, new [] {"x", ""});
		}
		[Pure] public override string ToString () {return (string) this;}
		[Pure] public static bool operator == (LinearInX left, LinearInX right) {
			return left.a == right.a && left.b == right.b;
		}
		[Pure] public static bool operator != (LinearInX left, LinearInX right) {return !(left == right);}
		[Pure] public bool Equals (LinearInX other) {return this == other;}
		[Pure] public override bool Equals (object o) {return o is LinearInX && this == (LinearInX) o;}
		[Pure] public override int GetHashCode () {return 19381 * a.GetHashCode() + 73459 * b.GetHashCode();}
		[Pure] public static LinearInX operator + (LinearInX left, LinearInX right) {
			return new LinearInX {a = left.a + right.a, b = left.b + right.b};
		}
		[Pure] public static LinearInX operator - (LinearInX left, LinearInX right) {return left + -right;}
		[Pure] public static LinearInX operator * (LinearInX left, float right) {
			return new LinearInX {a = left.a * right, b = left.b * right};
		}
		[Pure] public static LinearInX operator / (LinearInX left, float right) {
			return new LinearInX {a = left.a / right, b = left.b / right};
		}
		[Pure] public static LinearInX operator * (float left, LinearInX right) {return right * left;}
		[Pure] public static LinearInX operator - (LinearInX toNegate) {return -1 * toNegate;}
		[Pure] public static LinearInX operator + (LinearInX toReturn) {return toReturn;}
		///<summary>
		///	Preconditions:
		///		Coefficients has the same length as variableNames.
		///		All variable names unique.
		///</summary>
		public static string SimplifiedLinearCombinationExpression (IEnumerable<float> coefficients, IEnumerable<string> variableNames) {
			var coefficientsArray = coefficients.ToArray();
			return coefficientsArray.All(c => c == 0) ? "0" :
				string.Join(
					" + ",
					coefficients.ZipPairs(variableNames)
						.Where(pair => pair.first != 0)
						.Select(pair => Math.Abs(pair.first) == 1 && pair.second != "" ? (pair.first < 0 ? "-" : "") + pair.second : pair.first + pair.second)
						.ToArray()
				).Replace("+ -", "- ");
		}
	}
}
