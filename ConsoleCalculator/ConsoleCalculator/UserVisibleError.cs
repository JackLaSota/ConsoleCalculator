using System;

namespace ConsoleCalculator {
	public abstract class UserVisibleError : Exception {
		protected UserVisibleError (string message) : base(message) {}
	}
}
