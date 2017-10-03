using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using Pohgee.Framework.LightIoC.Concrete;
using Pohgee.SIF.DesktopAgent.Abstractions;
using Pohgee.SIF.DesktopAgent.Core.Configuration;

namespace Pohgee.SIF.DesktopAgent.Core.Service {
	/// <summary>
	/// 
	/// </summary>
	public class ServiceOperation : IServiceOperation, IDisposable {
		/// <summary>
		/// Gets or sets a value indicating whether this instance is disposed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
		/// </value>
		private bool IsDisposed {
			get;
			set;
		}

		/// <summary>
		/// Gets the process new files timer.
		/// </summary>
		/// <value>
		/// The process new files timer.
		/// </value>
		protected System.Timers.Timer ProcessNewFilesTimer {
			get;
			private set;
		}

		/// <summary>
		/// Gets the target directory.
		/// </summary>
		/// <value>
		/// The target directory.
		/// </value>
		protected FileSystemWatcher TargetDirectory {
			get;
			private set;
		}

		/// <summary>
		/// Gets the Q valent integration directory.
		/// </summary>
		/// <value>
		/// The Q valent integration directory.
		/// </value>
		protected FileSystemWatcher QValentIntegrationDirectory {
			get;
			private set;
		}


		/// <summary>
		/// Gets the instance id.
		/// </summary>
		/// <value>
		/// The instance id.
		/// </value>
		public Guid InstanceId {
			get;
			private set;
		}

		/// <summary>
		/// Gets the created on.
		/// </summary>
		/// <value>
		/// The created on.
		/// </value>
		public DateTime CreatedOn {
			get;
			private set;
		}

		/// <summary>
		/// Gets the log file location.
		/// </summary>
		/// <value>
		/// The log file location.
		/// </value>
		public string LogFileLocation {
			get;
			private set;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceOperation"/> class.
		/// </summary>
		public ServiceOperation() {
			Initialize();
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		protected void Initialize() {
			IsDisposed = false;
			CreatedOn = DateTime.Now;
			InstanceId = Guid.NewGuid();
			LogFileLocation = ConfigReader.GetConfiguration().RuntimeConfig.LogFileLocation;
		}


		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool isDisposing) {
			if (!IsDisposed) {
				if (isDisposing) {

				}
				IsDisposed = true;
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}