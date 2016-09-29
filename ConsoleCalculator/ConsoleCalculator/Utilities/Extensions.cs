using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleCalculator.Utilities {
	public static class Extensions {
		public static List<List<T>> Combinations <T> (this IEnumerable<T> original) {
			throw new NotImplementedException();
		}
		public static bool HasAttribute<T> (this MemberInfo memberInfo) where T : Attribute {
			return memberInfo.GetCustomAttributes(typeof(T), true).Any();
		}
	}
}
