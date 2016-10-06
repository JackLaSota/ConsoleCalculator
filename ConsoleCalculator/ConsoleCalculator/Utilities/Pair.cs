namespace ConsoleCalculator.Utilities {
	public class Pair <T, U> {
		// ReSharper disable once FieldCanBeMadeReadOnly.Global
		public T first;
		// ReSharper disable once FieldCanBeMadeReadOnly.Global
		public U second;
		public Pair (T first, U second) {
			this.first = first;
			this.second = second;
		}
		public override string ToString () {return "{" + first.ToStringAllowingNull() + ", " + second.ToStringAllowingNull() + "}";}
		// ReSharper disable once UnusedMember.Global
		public StructPair<T, U> AsStructPair => new StructPair<T, U>(first, second);
	}
	public struct StructPair <T, U> {
		// ReSharper disable once FieldCanBeMadeReadOnly.Global
		public T first;
		// ReSharper disable once FieldCanBeMadeReadOnly.Global
		public U second;
		public StructPair (T first, U second) {
			this.first = first;
			this.second = second;
		}
		public static bool operator == (StructPair<T, U> left, StructPair<T, U> right) {
			return left.first.Equals(right.first) && left.second.Equals(right.second);
		}
		public static bool operator != (StructPair<T, U> left, StructPair<T, U> right) {return !(left == right);}
		public override bool Equals (object o) {return o is StructPair<T, U> && this == (StructPair<T, U>) o;}
		public override int GetHashCode () {return 19381 * first.GetHashCode() + 73459 * second.GetHashCode();}
		public Pair<T, U> AsClassPair => new Pair<T, U>(first, second);
	}
}
