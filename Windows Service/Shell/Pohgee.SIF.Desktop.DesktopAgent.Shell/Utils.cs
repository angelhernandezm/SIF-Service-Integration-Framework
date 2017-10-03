using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Pohgee.SIF.Desktop.DesktopAgent.Shell {
	public class Utils {
		/// <summary>
		/// Gets the registry key values.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="subkey">The subkey.</param>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		public static IDictionary<string, string> GetRegistryKeyValues(RegistryKey key, string subkey, IEnumerable<string> values) {
			var val = string.Empty;
			var retval = new Dictionary<string, string>();

			if (key != null && !string.IsNullOrEmpty(subkey) && values != null) {
				try {
					using (var k = key.OpenSubKey(subkey)) {
						if (k != null) {
							foreach (var x in values) {
								if (!string.IsNullOrEmpty(val = k.GetValue(x) as string))
									retval.Add(x, val);
							}
						}
					}
				} catch {
					retval = null;
				}
			} else
				throw new ArgumentException("Unable to retrieve registry information if arguments are missing.");

			return retval;
		}

	}
}
