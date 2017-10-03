using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pohgee.SIF.DesktopAgent.Core {
	public static class Extensions {
		/// <summary>
		/// Determines whether the specified value is between.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <param name="min">The min.</param>
		/// <param name="max">The max.</param>
		/// <returns>
		///   <c>true</c> if the specified value is between; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsBetween<T>(this T value, T min, T max) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
			var retval = false;

			switch (typeof(T).Name) {
				case "Int32":
					retval = (int)((object)value) >= (int)((object)min) && (int)((object)value) <= (int)((object)max);
					break;
				case "Decimal":
					retval = (decimal)((object)value) >= (decimal)((object)min) && (decimal)((object)value) <= (decimal)((object)max);
					break;

				case "Double":
					retval = (double)((object)value) >= (double)((object)min) && (double)((object)value) <= (double)((object)max);
					break;

				case "Single":
					retval = (float)((object)value) >= (float)((object)min) && (float)((object)value) <= (float)((object)max);
					break;

				case "Int64":
					retval = (long)((object)value) >= (long)((object)min) && (long)((object)value) <= (long)((object)max);
					break;
			}

			return retval;
		}
	}
}
