using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace ConsoleCalculator.Utilities {
	public static class Extensions {
		public static bool HasAttribute<T> (this MemberInfo memberInfo) where T : Attribute {
			return memberInfo.GetCustomAttributes(typeof(T), true).Any();
		}
		[Pure] public static IEnumerable<T> AsSingleton <T> (this T t) {yield return t;}
		[Pure] public static IEnumerable<T> AsSingletonOrEmpty <T> (this T t) {if (t != null) yield return t;}
		[Pure] public static IEnumerable<T> Closure <T> (//Adapted from: http://stackoverflow.com/a/10253591
			this T start,
			Func<T, IEnumerable<T>> reachableFrom,
			bool excludeStartUnlessReachedAgain = false
		) {
			return ClosureFromMultiple(start.AsSingleton(), reachableFrom, excludeStartUnlessReachedAgain);
		}
		[Pure] public static IEnumerable<T> ClosureFromMultiple <T> (
			this IEnumerable<T> start,
			Func<T, IEnumerable<T>> reachableFrom,
			bool excludeStartUnlessReachedAgain = false
		) {
			var seen = new HashSet<T>();
			var startStack = new Stack<T>(start);
			var reachedStack = new Stack<T>();
			while (startStack.Count != 0) {
				var next = startStack.Pop();
				if (seen.Add(next)) {
					if (!excludeStartUnlessReachedAgain)
						yield return next;
					foreach (var child in reachableFrom(next))
						reachedStack.Push(child);
				}
			}
			while (reachedStack.Count != 0) {
				var next = reachedStack.Pop();
				if (seen.Add(next)) {
					yield return next;
					foreach (var child in reachableFrom(next))
						reachedStack.Push(child);
				}
			}
		}
		[Pure] public static HashSet<T> ToHashSet<T> (this IEnumerable<T> enumerable) {
			var hashset = new HashSet<T>();
			foreach (var t in enumerable)
				hashset.Add(t);
			return hashset;
		}
	}
}