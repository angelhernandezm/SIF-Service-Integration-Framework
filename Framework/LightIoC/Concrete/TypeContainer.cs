using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Threading;
using Pohgee.Framework.LightIoC.Abstractions;
using Pohgee.Framework.LightIoC.Configuration;

namespace Pohgee.Framework.LightIoC.Concrete {
	/// <summary>
	/// 
	/// </summary>
	public sealed class TypeContainer : ITypeContainer {
		#region "Members"

		private static volatile TypeContainer singleton;
		private static readonly object syncRoot = new object();

		#endregion

		#region "Events"

		/// <summary>
		/// Occurs when [on life span expired].
		/// </summary>
		public static event LifeSpanEventhandler OnLifeSpanExpired;

		#endregion

		#region "Ctors"

		/// <summary>
		/// Prevents a default instance of the <see cref="TypeContainer"/> class from being created.
		/// </summary>
		private TypeContainer(IGenericLogger logger) {
			Logger = logger;
		}

		#endregion

		#region "Properties"


		/// <summary>
		/// Gets the current.
		/// </summary>
		/// <value>
		/// The current.
		/// </value>
		public static TypeContainer Current {
			get {
				if (singleton == null) {
					lock (syncRoot) {
						if (singleton == null)
							singleton = new TypeContainer(new IoCLogger()) {
								Registry = new Dictionary<TypeInformation, object>()
							};

						singleton.RetrieveDependencies();
					}
				}
				return singleton;
			}
		}


		/// <summary>
		/// Gets the registry.
		/// </summary>
		/// <value>
		/// The registry.
		/// </value>
		internal Dictionary<TypeInformation, object> Registry {
			get;
			private set;
		}

		/// <summary>
		/// Gets the dependencies.
		/// </summary>
		/// <value>
		/// The dependencies.
		/// </value>
		private List<Assembly> Dependencies {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		/// <value>
		/// The logger.
		/// </value>
		private IGenericLogger Logger {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the instance span checker.
		/// </summary>
		/// <value>
		/// The instance span checker.
		/// </value>
		private System.Timers.Timer InstanceSpanChecker {
			get;
			set;
		}

		#endregion


		/// <summary>
		/// Retrieves the dependencies.
		/// </summary>
		private void RetrieveDependencies() {
			var appFolder = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

			// Let's load the assemblies in bin folder (even if not referenced in solution)
			var binaries = Directory.EnumerateFiles(appFolder, "*.dll").ToList();

			if (binaries.Count > 0)
				binaries.ForEach(x => Assembly.LoadFrom(x));

			Dependencies = AppDomain.CurrentDomain.GetAssemblies()
							.Where(x => Path.GetDirectoryName(new Uri(x.CodeBase).LocalPath)
							  .Equals(appFolder, StringComparison.OrdinalIgnoreCase)).ToList();
		}

		/// <summary>
		/// Ares the registrations and mappings ok.
		/// </summary>
		/// <param name="registrations">The registrations.</param>
		/// <param name="mappings">The mappings.</param>
		/// <returns></returns>
		private bool AreRegistrationsAndMappingsOk(ConfigurationElementCollection registrations, ConfigurationElementCollection mappings) {
			return (registrations != null && mappings != null && registrations.Count > 0 && registrations.Count.Equals(mappings.Count));
		}

		/// <summary>
		/// Pres the JIT methods.
		/// </summary>
		/// <param name="target">The target.</param>
		private void PreJitMethods(Type target) {
			if (target != null) {
				ThreadPool.QueueUserWorkItem((object stateInfo) => target.GetMethods(Consts.JitMethodFlag).ToList().ForEach(x => {
					if (!x.IsGenericMethod)
						RuntimeHelpers.PrepareMethod(x.MethodHandle);
					else
						RuntimeHelpers.PrepareMethod(x.MethodHandle, new RuntimeTypeHandle[] { x.DeclaringType.TypeHandle });

				}));
			}
		}


		/// <summary>
		/// Determines whether [is registered already] [the specified new registration].
		/// </summary>
		/// <param name="newRegistration">The new registration.</param>
		/// <returns>
		///   <c>true</c> if [is registered already] [the specified new registration]; otherwise, <c>false</c>.
		/// </returns>
		private bool IsRegisteredAlready(TypeInformation newRegistration) {
			var retval = false;

			if (newRegistration != null && Registry.Count > 0)
				retval = Registry.Keys.Count(x => string.Equals(x.Abstraction.Name, newRegistration.Abstraction.Name, StringComparison.OrdinalIgnoreCase) &&
											 UseFQNInsteadOfShortName(x.Concrete, newRegistration.Concrete) &&
											 string.Equals(x.Name, newRegistration.Name, StringComparison.OrdinalIgnoreCase)) > 0;
			return retval;
		}

		/// <summary>
		/// Registrations the helper.
		/// </summary>
		/// <param name="newRegistration">The new registration.</param>
		/// <param name="concrete">The concrete.</param>
		/// <param name="createInstance">if set to <c>true</c> [create instance].</param>
		/// <param name="isPrejittingEnabled">if set to <c>true</c> [is prejitting enabled].</param>
		/// <param name="ctorArgs">The ctor args.</param>
		/// <returns></returns>
		private void RegistrationHelper(TypeInformation newRegistration, Type concrete, bool createInstance = false, bool isPrejittingEnabled = true, object[] ctorArgs = null) {
			if ((newRegistration.Concrete = concrete) != null && !IsRegisteredAlready(newRegistration)) {
				try {
					if (isPrejittingEnabled)
						PreJitMethods(concrete);

					newRegistration.Concrete = concrete;

					lock (Registry) {
						Registry.Add(newRegistration, createInstance ? newRegistration.Concrete.DynamicallyCreateInstance(ctorArgs) : null);
					}
				} catch (Exception ex) {
					Logger.Log(ex);
					throw;
				}
			}
		}

		/// <summary>
		/// Instances the span manager.
		/// </summary>
		/// <param name="mappings">The mappings.</param>
		private void InstanceSpanManager(IEnumerable<MappingConfigElement> mappings) {
			List<MappingConfigElement> items;

			if (mappings != null && (items = mappings.ToList()).Count > 0) {
				var max = items.Max(x => int.Parse(x.LifeSpan));

				if (max > 0) {
					InstanceSpanChecker = new System.Timers.Timer(max * 1000) {
						Enabled = true
					};

					InstanceSpanChecker.Elapsed += (sender, e) => {
						var selected = Registry.Where(x => x.Key.LifeSpan > 0 && x.Key.CreatedOn.AddSeconds(x.Key.LifeSpan).Subtract(DateTime.Now).Milliseconds < 0).ToList();

						if (selected.Count > 0) {
							selected.ForEach(z => {
								if (z.Value != null && z.Value is IDisposable)
									((IDisposable)z.Value).Dispose();

								lock (Registry) {
									Registry.Remove(z.Key);
								}

								if (OnLifeSpanExpired != null)
									OnLifeSpanExpired(z.Key);
							});

						} else {
							// Shutdown InstanceSpanManager
							InstanceSpanChecker.Enabled = false;
							InstanceSpanChecker.Dispose();
						}
					};
				}
			}
		}


		/// <summary>
		/// Registers the type.
		/// </summary>
		/// <param name="useConfig">if set to <c>true</c> [use config].</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void RegisterType(bool useConfig) {
			Assembly tempAsm = null;
			var notFound = new List<string>();
			Type concrete = null, abstraction = null;
			var config = ConfigurationManager.GetSection(Consts.ConfigSectionName) as ConfigReader;

			if (AreRegistrationsAndMappingsOk(config.Registrations, config.Mappings)) {
				var mappings = config.Mappings.Cast<MappingConfigElement>();
				var registrations = config.Registrations.Cast<RegistrationConfigElement>();

				registrations.ToList().ForEach(x => {
					var newRegistration = new TypeInformation() {
						Name = x.Name
					};

					var mapping = mappings.FirstOrDefault(z => z.Abstraction.Equals(x.Name, StringComparison.OrdinalIgnoreCase));

					if (mapping != null) {
						bool instanceRequired, isPrejittingEnabled;
						newRegistration.LifeSpan = int.Parse(mapping.LifeSpan);
						bool.TryParse(mapping.InstanceRequired, out instanceRequired);
						bool.TryParse(config.PreJitting.Enabled, out isPrejittingEnabled);

						if (!string.IsNullOrEmpty(x.AssemblyFQN) &&  // Let's ensure the assembly specified exists
							(tempAsm = Dependencies.FirstOrDefault(y => y.FullName.Equals(x.AssemblyFQN, StringComparison.OrdinalIgnoreCase))) != null) {
							concrete = FindFirstConcreteImplementation(mapping.Abstraction, mapping.To, out abstraction);
							newRegistration.Abstraction = abstraction;
						} else {
							// Otherwise, we'll find the first occurrence of a type implementing the specified interface
							concrete = FindFirstConcreteImplementation(mapping.Abstraction, mapping.To, out abstraction);
							newRegistration.Abstraction = abstraction;
						}

						RegistrationHelper(newRegistration, concrete, instanceRequired, isPrejittingEnabled, null);
					} else
						notFound.Add(string.Format(Consts.MappingInformationMissing, x.Name));
				});

				InstanceSpanManager(mappings);

				// If there's been errors, we'll notify by throwing an exception
				if (notFound.Count > 0) {
					throw new NotSupportedException(notFound.TokenizeItemsInList(Consts.GenericMappingErrorSummary));
				}
			} else
				throw new ArgumentException(Consts.MappingRegistrationMismatch);
		}

		/// <summary>
		/// Registers the type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="concreteObj">The concrete object.</param>
		public void RegisterType<T>(object concreteObj) {
			var abstraction = typeof(T);

			if (concreteObj == null)
				throw new NullReferenceException(Consts.NullParametersException);

			if (!(concreteObj is T))
				throw new TypeMismatchException(string.Format(Consts.ConcreteClassDoesntImplementInterface,
					new object[] { concreteObj.GetType().Name, abstraction.Name }));

			var found = Registry.Keys.FirstOrDefault(x => string.Equals(x.Abstraction.Name, abstraction.Name, StringComparison.OrdinalIgnoreCase));

			if (found == null)
				Registry.Add(new TypeInformation() {
					Abstraction = abstraction, Name = abstraction.Name
				}, concreteObj);
		}

		/// <summary>
		/// Uses the short name of the FQN instead of.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="className">Name of the class.</param>
		/// <returns></returns>
		private bool UseFQNInsteadOfShortName(Type target, string className) {
			var retval = false;

			if (target != null && !string.IsNullOrEmpty(className)) {
				retval = className.IndexOf('.') >= 0
							? !string.IsNullOrEmpty(target.FullName) && target.FullName.Equals(className, StringComparison.OrdinalIgnoreCase)
							: target.Name.Equals(className, StringComparison.OrdinalIgnoreCase);
			}

			return retval;
		}

		/// <summary>
		/// Uses the short name of the FQN instead of.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="registration">The registration.</param>
		/// <returns></returns>
		private bool UseFQNInsteadOfShortName(Type target, Type registration) {
			var retval = false;

			if (target != null && registration != null) { // We try finding our concrete by its FQN otherwise we try using its short name
				if (!(retval = !string.IsNullOrEmpty(target.FullName) && target.FullName.Equals(registration.FullName, StringComparison.OrdinalIgnoreCase)))
					retval = !string.IsNullOrEmpty(target.Name) && target.Name.Equals(registration.Name, StringComparison.OrdinalIgnoreCase);
			}

			return retval;
		}


		/// <summary>
		/// Finds the first concrete implementation.
		/// </summary>
		/// <param name="interfaceName">Name of the interface.</param>
		/// <param name="concreteName">Name of the concrete.</param>
		/// <param name="abstraction">The abstraction.</param>
		/// <returns></returns>
		private Type FindFirstConcreteImplementation(string interfaceName, string concreteName, out Type abstraction) {
			Type retval = null, concrete = null, tempAbstraction = null;

			Dependencies.ForEach(p => p.GetTypes().FirstOrDefault(b => {
				var found = false;
				if (concrete == null && b.IsClass && (tempAbstraction = b.GetInterface(interfaceName)) != null &&
					UseFQNInsteadOfShortName(b, concreteName)) {
					concrete = retval = b;
					found = true;
				}
				return found;
			}));

			abstraction = tempAbstraction;

			return retval;
		}


		/// <summary>
		/// Registers the type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="concreteName">Name of the concrete.</param>
		/// <param name="createInstance">if set to <c>true</c> [create instance].</param>
		/// <param name="ctorArgs">The ctor args.</param>
		/// <param name="isPrejittingEnabled">if set to <c>true</c> [is prejitting enabled].</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void RegisterType<T>(string concreteName, bool createInstance = false, object[] ctorArgs = null, bool isPrejittingEnabled = true) {
			if (!string.IsNullOrEmpty(concreteName)) {
				Type concrete = null, abstraction = null;
				var newRegistration = new TypeInformation() {
					Name = typeof(T).Name
				};

				newRegistration.Concrete = concrete = FindFirstConcreteImplementation(typeof(T).Name, concreteName, out abstraction);
				newRegistration.Abstraction = abstraction;

				RegistrationHelper(newRegistration, concrete, createInstance, isPrejittingEnabled, ctorArgs);
			} else
				throw new ArgumentException(Consts.MissingConcreteName);
		}

		/// <summary>
		/// Registers the types.
		/// </summary>
		/// <param name="types">The types.</param>
		/// <param name="createInstances">if set to <c>true</c> [create instances].</param>
		/// <param name="isPrejittingEnabled">if set to <c>true</c> [is prejitting enabled].</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void RegisterTypes(Dictionary<Type, Type> types, bool createInstances = false, bool isPrejittingEnabled = true) {
			var errors = new List<string>();

			if (types != null) {
				if (types.Count > 0) {
					types.ToList().ForEach(x => {
						if (IsValidMapping(x)) {
							var newRegistration = new TypeInformation() {
								Name = x.Key.Name, Abstraction = x.Key, Concrete = x.Value
							};

							if (!IsRegisteredAlready(newRegistration)) {
								if (isPrejittingEnabled)
									PreJitMethods(newRegistration.Concrete);

								lock (Registry) {
									Registry.Add(newRegistration, createInstances ? newRegistration.Concrete.DynamicallyCreateInstance() : null);
								}
							}
						} else
							errors.Add(x.Key != null && x.Value != null ?
							 string.Format(Consts.ConcreteClassDoesntImplementInterface, new object[] { x.Key.Name, x.Value.Name }) :
							 Consts.NullParametersException);
					});

					// If there's been errors, we'll notify by throwing an exception
					if (errors.Count > 0) {
						throw new NotSupportedException(errors.TokenizeItemsInList(Consts.GenericMappingErrorSummary));
					}
				} else
					throw new TargetParameterCountException(Consts.TypeDictionaryIsEmpty);
			} else
				throw new NoNullAllowedException(Consts.TypeDictionaryIsNull);
		}


		/// <summary>
		/// Determines whether [is valid mapping] [the specified pair].
		/// </summary>
		/// <param name="pair">The pair.</param>
		/// <returns>
		///   <c>true</c> if [is valid mapping] [the specified pair]; otherwise, <c>false</c>.
		/// </returns>
		private bool IsValidMapping(KeyValuePair<Type, Type> pair) {
			return (pair.Key != null && pair.Value != null && pair.Value.GetInterface(pair.Key.Name, true) != null);
		}

		/// <summary>
		/// Registers the type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TV">The type of the V.</typeparam>
		/// <param name="abstraction">The abstraction.</param>
		/// <param name="concrete">The concrete.</param>
		/// <param name="createInstance">if set to <c>true</c> [create instance].</param>
		/// <param name="ctorArgs">The ctor args.</param>
		/// <param name="isPrejittingEnabled">if set to <c>true</c> [is prejitting enabled].</param>
		/// <exception cref="TypeMismatchException"></exception>
		public void RegisterType<T, TV>(T abstraction, TV concrete, bool createInstance = false, object[] ctorArgs = null, bool isPrejittingEnabled = true) {
			var newRegistration = new TypeInformation() {
				Abstraction = typeof(T), Concrete = typeof(TV), Name = typeof(T).Name
			};

			if (newRegistration.Concrete.IsImplemented(newRegistration.Abstraction)) {
				if (!IsRegisteredAlready(newRegistration)) {
					if (isPrejittingEnabled)
						PreJitMethods(newRegistration.Concrete);

					lock (Registry) {
						Registry.Add(newRegistration, createInstance ? newRegistration.Concrete.DynamicallyCreateInstance(ctorArgs) : null);
					}
				}
			} else
				throw new TypeMismatchException(string.Format(Consts.ConcreteClassDoesntImplementInterface,
					new object[] { newRegistration.Concrete.Name, newRegistration.Abstraction.Name }));
		}

		/// <summary>
		/// Registers the types.
		/// </summary>
		/// <param name="types">The types.</param>
		/// <param name="createInstances">if set to <c>true</c> [create instances].</param>
		/// <param name="isPrejittingEnabled">if set to <c>true</c> [is prejitting enabled].</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void RegisterTypes(Dictionary<Type, string> types, bool createInstances = false, bool isPrejittingEnabled = true) {
			var errors = new List<string>();

			if (types != null) {
				if (types.Count > 0) {
					types.ToList().ForEach(x => {
						Type tempType;
						var newRegistration = new TypeInformation() {
							Name = x.Key.Name, Abstraction = x.Key, Concrete = FindFirstConcreteImplementation(x.Key.Name, x.Value, out tempType)
						};

						if (newRegistration.Concrete != null && !IsRegisteredAlready(newRegistration)) {
							if (isPrejittingEnabled)
								PreJitMethods(newRegistration.Concrete);

							lock (Registry) {
								Registry.Add(newRegistration, createInstances ? newRegistration.Concrete.DynamicallyCreateInstance() : null);
							}
						} else
							errors.Add(string.Format(Consts.ConcreteClassDoesntImplementInterface, new object[] { x.Key.Name, x.Value }));
					});

					// If there's been errors, we'll notify by throwing an exception
					if (errors.Count > 0) {
						throw new NotSupportedException(errors.TokenizeItemsInList(Consts.GenericMappingErrorSummary));
					}
				} else
					throw new TargetParameterCountException(Consts.TypeDictionaryIsEmpty);
			} else
				throw new NoNullAllowedException(Consts.TypeDictionaryIsNull);
		}

		/// <summary>
		/// Resolves this instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="protectWithACriticalSection">if set to <c>true</c> [protect with A critical section].</param>
		/// <returns></returns>
		public T Resolve<T>(bool protectWithACriticalSection = false) {
			object retval = null;

			if (Registry.Count > 0) {
				var entry = Registry.Keys.FirstOrDefault(x => x.Abstraction.Name.Equals(typeof(T).Name, StringComparison.OrdinalIgnoreCase));

				if (entry != null) {
					if (!protectWithACriticalSection) {
						retval = Registry[entry] ?? entry.Concrete.DynamicallyCreateInstance();
					} else {
						lock (Registry) {
							retval = Registry[entry] ?? entry.Concrete.DynamicallyCreateInstance();
						}
					}
				}
			}

			return (T)retval;
		}
	}
}