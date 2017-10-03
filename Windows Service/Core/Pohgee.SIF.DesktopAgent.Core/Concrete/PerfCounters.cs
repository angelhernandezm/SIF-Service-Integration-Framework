using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pohgee.SIF.DesktopAgent.Abstractions;

namespace Pohgee.SIF.DesktopAgent.Core.Concrete {
	public class PerfCounters : IPerfCounters, IDisposable {
		/// <summary>
		/// Gets the created configuration.
		/// </summary>
		/// <value>
		/// The created configuration.
		/// </value>
		public DateTime CreatedOn {
			get;
			private set;
		}

		/// <summary>
		/// Gets the instance unique identifier.
		/// </summary>
		/// <value>
		/// The instance unique identifier.
		/// </value>
		public Guid InstanceId {
			get;
			private set;
		}

		/// <summary>
		/// Gets the <see cref="PerformanceCounter" /> with the specified counter name.
		/// </summary>
		/// <value>
		/// The <see cref="PerformanceCounter" />.
		/// </value>
		/// <param name="counterName">Name of the counter.</param>
		/// <returns></returns>
		public PerformanceCounter this[PerfCounter counterName] {
			get {
				var retval = CountersAvailable.ContainsKey(counterName) ? CountersAvailable[counterName] : null;
				return retval;
			}
		}

		/// <summary>
		/// Gets or sets the counters available.
		/// </summary>
		/// <value>
		/// The counters available.
		/// </value>
		protected Dictionary<PerfCounter, PerformanceCounter> CountersAvailable {
			get;
			set;
		}

		/// <summary>
		/// Gets the counter data collection.
		/// </summary>
		/// <value>
		/// The counter data collection.
		/// </value>
		protected CounterCreationDataCollection CounterDataCollection {
			get;
			private set;
		}

		/// <summary>
		/// The disposed
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
		/// </value>
		protected bool IsDisposed {
			get;
			private set;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="PerfCounters"/> class.
		/// </summary>
		public PerfCounters() {
			CreatedOn = DateTime.Now;
			InstanceId = Guid.NewGuid();
			CounterDataCollection = new CounterCreationDataCollection();
			CountersAvailable = new Dictionary<PerfCounter, PerformanceCounter>();
			CreateOrInitializePerfCounters();
		}

		/// <summary>
		/// Finalizes an instance of the <see cref="PerfCounters"/> class.
		/// </summary>
		~PerfCounters() {
			Dispose(false);
		}

		/// <summary>
		/// Creates the original initialize perf counters.
		/// </summary>
		protected void CreateOrInitializePerfCounters() {
			// Let's create Performance counter category
			if (!PerformanceCounterCategory.Exists(Consts.PerformanceCounterCategory)) {
				CounterDataCollection.AddRange(new CounterCreationData[] {
													new CounterCreationData(Consts.TimeTakenCounter, string.Empty, PerformanceCounterType.NumberOfItems32),
													new CounterCreationData(Consts.TotalErrorCounter, string.Empty, PerformanceCounterType.NumberOfItems32),
													new CounterCreationData(Consts.TotalUnhandledErrorCounter, string.Empty, PerformanceCounterType.NumberOfItems32),
													new CounterCreationData(Consts.TotalFailedAutomationCounter, string.Empty, PerformanceCounterType.NumberOfItems32),
													new CounterCreationData(Consts.TotalAutomationRequestCounter, string.Empty, PerformanceCounterType.NumberOfItems32),
													new CounterCreationData(Consts.TotalSucceededAutomationCounter, string.Empty, PerformanceCounterType.NumberOfItems32)
				                               	});

				PerformanceCounterCategory.Create(Consts.PerformanceCounterCategory, Consts.PerformanceCounterCategory,
												 PerformanceCounterCategoryType.MultiInstance, CounterDataCollection);
			}

			// Let's ensure category exists and create custom performance counters
			if (PerformanceCounterCategory.Exists(Consts.PerformanceCounterCategory)) {
				CountersAvailable.Add(PerfCounter.TimeTaken, new PerformanceCounter(Consts.PerformanceCounterCategory, Consts.TimeTakenCounter, Consts.PerformanceCounterCategory, false));
				CountersAvailable.Add(PerfCounter.ErrorCount, new PerformanceCounter(Consts.PerformanceCounterCategory, Consts.TotalErrorCounter, Consts.PerformanceCounterCategory, false));
				CountersAvailable.Add(PerfCounter.UnhandledExceptions, new PerformanceCounter(Consts.PerformanceCounterCategory, Consts.TotalUnhandledErrorCounter, Consts.PerformanceCounterCategory, false));
				CountersAvailable.Add(PerfCounter.FailedAutomations, new PerformanceCounter(Consts.PerformanceCounterCategory, Consts.TotalFailedAutomationCounter, Consts.PerformanceCounterCategory, false));
				CountersAvailable.Add(PerfCounter.TotalAutomationRequests, new PerformanceCounter(Consts.PerformanceCounterCategory, Consts.TotalAutomationRequestCounter, Consts.PerformanceCounterCategory, false));
				CountersAvailable.Add(PerfCounter.SucceededAutomations, new PerformanceCounter(Consts.PerformanceCounterCategory, Consts.TotalSucceededAutomationCounter, Consts.PerformanceCounterCategory, false));
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool isDisposing) {
			if (!IsDisposed) {
				if (isDisposing) {
					if (CountersAvailable != null && CountersAvailable.Count > 0)
						CountersAvailable.ToList().ForEach(x => {
							x.Value.Close();
							x.Value.Dispose();
						});
				}
				IsDisposed = true;
			}
		}
	}
}
