using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Pohgee.SIF.DesktopAgent.Abstractions;

namespace Pohgee.SIF.DesktopAgent.Core.Concrete {
	public class Logger : ILogger {
		/// <summary>
		/// The mutex
		/// </summary>
		private static readonly Mutex mutex = new Mutex();
		 
		/// <summary>
		/// Writes the specified entry.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <param name="location">The location.</param>
		public void Write(string entry, string location) {
			var hasError = false;

			if (!string.IsNullOrEmpty(entry) && !string.IsNullOrEmpty(location)) {
				var fileName = string.Format(Consts.LogFileNameAndLocation, new object[] { location, DateTime.Now.ToShortDateString().Replace("/", string.Empty) });
				try {
					mutex.WaitOne();

					if (!Directory.Exists(location))
						Directory.CreateDirectory(location);

					using (var writer = !File.Exists(fileName) ? File.CreateText(fileName) : File.AppendText(fileName)) {
						writer.WriteLine(string.Format(Consts.LogEntryFormat, new object[] {DateTime.Now, entry }));
						writer.Close();
					}
				} catch (Exception ex) {
					hasError = true;
				} finally {
					mutex.ReleaseMutex();
				}

				if (hasError)
					throw new IOException(Consts.UnableToFindLogFolder);

			} else
				throw new ArgumentException(Consts.MissingArgumentException);

		}
	}
}
