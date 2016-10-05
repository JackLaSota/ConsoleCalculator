using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace ConsoleCalculator.Utilities {
	public static partial class Extensions {
		public static bool HasAttribute<T> (this MemberInfo memberInfo) where T : Attribute {
			return memberInfo.GetCustomAttributes(typeof(T), true).Any();
		}
		[Pure] public static IEnumerable<T> AsSingleton <T> (this T t) {yield return t;}
		[Pure] public static IEnumerable<T> AsSingletonOrEmpty <T> (this T t) {if (t != null) yield return t;}
		[Pure] public static IEnumerable<T> AsNonNullableSingletonOrEmpty <T> (this T? t) where T : struct {if (t != null) yield return t.Value;}
		[Pure] public static IEnumerable<T> Closure <T> (//Adapted from: http://stackoverflow.com/a/10253591
			this T start,
			Func<T, IEnumerable<T>> reachableFrom,
			bool excludeStartUnlessReachedAgain = false
		) {
			return ClosureFromMultiple(start.AsSingleton(), reachableFrom, excludeStartUnlessReachedAgain);
		}
		[Pure] public static HashSet<T> FixedPointUnder <T> (this IEnumerable<T> startSet, Func<HashSet<T>, HashSet<T>> step) {
			var current = startSet.ToHashSet();
			HashSet<T> previousStep;
			do {
				previousStep = current;
				current = step(current);
			} while (!current.HasSameContentAs(previousStep));
			return current;
		}
		[Pure] public static bool HasSameContentAs <T> (this HashSet<T> left, HashSet<T> right) {
			return left.Count == right.Count && left.IsSubsetOf(right);
		}
		[Pure] public static IEnumerable<T> ClosureFromMultiple <T> (
			this IEnumerable<T> start,
			Func<T, IEnumerable<T>> reachableFrom,
			bool excludeStartUnlessReachedAgain = false
		) {
			if (excludeStartUnlessReachedAgain) {
				foreach (var reachable in start.SelectMany(reachableFrom).Distinct().ClosureFromMultiple(reachableFrom))
					yield return reachable;
				yield break;
			}
			var seen = new HashSet<T>();
			var stack = new Stack<T>(start);
			while (stack.Count != 0) {
				var next = stack.Pop();
				if (seen.Add(next)) {
					yield return next;
					foreach (var child in reachableFrom(next))
						stack.Push(child);
				}
			}
		}
		[Pure] public static HashSet<T> ToHashSet<T> (this IEnumerable<T> enumerable) {
			var hashset = new HashSet<T>();
			foreach (var t in enumerable)
				hashset.Add(t);
			return hashset;
		}
		[Pure] public static bool ContainsAll <T> (this IEnumerable<T> container, IEnumerable<T> possibleSubset) {
			var possibleSubsetAsHashSet = possibleSubset.ToHashSet();
			var seenSoFarInBoth = new HashSet<T>();
			// ReSharper disable once LoopCanBeConvertedToQuery
			foreach (var t in container) {
				if (possibleSubsetAsHashSet.Contains(t))
					if (seenSoFarInBoth.Add(t))
						if (seenSoFarInBoth.Count == possibleSubsetAsHashSet.Count)
							return true;
			}
			return possibleSubsetAsHashSet.Count == 0;
		}
		[Pure] public static IEnumerable<T> Then <T> (this IEnumerable<T> enumerable, T extra) {
			return enumerable.Concat(extra.AsSingleton());
		}
		[Pure] public static bool AllDistinct <T> (this IEnumerable<T> enumerable) {
			var hashSet = new HashSet<T>();
			return enumerable.All(t => hashSet.Add(t));
		}
		[Pure] public static IEnumerable<T> Except<T> (this IEnumerable<T> enumerable, T exception) {
			return enumerable.Where(t => !t.Equals(exception));
		}
		[Pure] public static string ToStringAllowingNull (this object o) {return o == null ? "null" : o.ToString();}
		[Pure] public static string BeforeLast (this string original, string substring) {
			var lastIndexOf = original.LastIndexOf(substring, StringComparison.Ordinal);
			return lastIndexOf == -1 ? original : original.Substring(0, lastIndexOf);
		}
		[Pure] public static string ToDetailedString<T> (this IEnumerable<T> enumerable) {
			var toReturn = "{";
			foreach (var t in enumerable)
				toReturn += t.ToStringAllowingNull() + ", ";
			if (enumerable.Count() != 0)
				toReturn = toReturn.BeforeLast(", ");
			toReturn += "}";
			return toReturn;
		}
		[Pure] public static IEnumerable<T> WhereNull <T> (this IEnumerable<T> enumerable) where T : class {return enumerable.Where(t => t == null);}
		[Pure] public static IEnumerable<Pair<T, S>> ZipPairs <T, S> (this IEnumerable<T> left, IEnumerable<S> right) {
			return ZipStructPairs(left, right).Select(pair => pair.AsClassPair);
		}
		[Pure] public static IEnumerable<StructPair<T, S>> ZipStructPairs <T, S> (this IEnumerable<T> left, IEnumerable<S> right) {
			var leftEnumerator = left.GetEnumerator(); var rightEnumerator = right.GetEnumerator();
			while (leftEnumerator.MoveNext() && rightEnumerator.MoveNext()) yield return new StructPair<T, S>(leftEnumerator.Current, rightEnumerator.Current);
		}
	}
}
