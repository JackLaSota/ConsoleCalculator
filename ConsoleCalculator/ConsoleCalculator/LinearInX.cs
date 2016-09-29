using JetBrains.Annotations;

namespace ConsoleCalculator {
	/// <summary> Represents (ax + b) </summary>
	public partial struct LinearInX {
		public float a;
		public float b;
		[Pure] public static explicit operator string (LinearInX toCast) {
			return "(" + toCast.a + "x + " + toCast.b + ")";
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
	}
}
