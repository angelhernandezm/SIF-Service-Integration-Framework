using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;

namespace Pohgee.Framework.LightIoC.Concrete {
	public static class Extensions {
		/// <summary>
		/// Determines whether the specified concrete is implemented.
		/// </summary>
		/// <param name="concrete">The concrete.</param>
		/// <param name="targetInterface">The target interface.</param>
		/// <returns>
		///   <c>true</c> if the specified concrete is implemented; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsImplemented(this Type concrete, Type targetInterface) {
			return (concrete != null && targetInterface != null &&
					concrete.GetInterfaces().FirstOrDefault(
						x => x.Name.Equals(targetInterface.Name, StringComparison.OrdinalIgnoreCase)) != null);
		}


		/// <summary>
		/// Dynamicallies the create instance.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		public static object DynamicallyCreateInstance(this Type target, object[] args = null) {
			object retval = null;

			if (target != null) {
				if (args == null)
					retval = target.GetConstructor(Type.EmptyTypes).SafelyInstantiateObject(args);
				else {
					var z = (from o in target.GetConstructors().Where(x => x.GetParameters().Count().Equals(args.Length))
							select new {
								Ctor = o,
								Params = o.GetParameters()
							}).FirstOrDefault();

					if (z != null)
						retval = z.Ctor.SafelyInstantiateObject(args);
				}
			} else throw  new NullReferenceException(Consts.TypeWasExpected);

			if (retval == null)
				throw new TypeLoadException(Consts.NotSuitableConstructorFound);

			return retval;
		}


		/// <summary>
		/// Dynamicallies the name of the find method by.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="methodName">Name of the method.</param>
		/// <returns></returns>
		public static Func<object, object[], object> DynamicallyFindMethodByName(this Type target, string methodName) {
			MethodInfo method;
			Func<object, object[], object> retval = null;

			if (target != null && !string.IsNullOrEmpty(methodName) &&
				(method = target.GetMethod(methodName, Consts.ReflectionMethodFlags)) != null) {
				// Instance methods only!!!
				if (!method.IsStatic)
					retval = method.Invoke;
				else
					throw new TypeMismatchException(Consts.StaticWasPassedInsteadOfInstance);
			}

			return retval;
		}

		/// <summary>
		/// Tokenizes the items in list.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="addHeaderMessage">The add header message.</param>
		/// <returns></returns>
		public static string TokenizeItemsInList(this IEnumerable<string> list, string addHeaderMessage = null) {
			var index = 0;
			List<string> items;
			var retval = new StringBuilder(!string.IsNullOrEmpty(addHeaderMessage) ? addHeaderMessage.Insert(addHeaderMessage.Length - 1, "\n\r") : string.Empty);

			if (list != null && (items = list.ToList()).Count > 0) {
				items.ForEach(x => retval.Append(string.Format(Consts.MappingGenericError, new object[] {++index, x })));
				retval.Append(string.Format(Consts.ErrorCountSummary, index));
			}

			return retval.ToString();
		}


		/// <summary>
		/// Safelies the instantiate object.
		/// </summary>
		/// <param name="ctor">The ctor.</param>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		public static object SafelyInstantiateObject(this ConstructorInfo ctor, object[] args = null) {
			object retval = null;

			try {
				retval = ctor.Invoke(args ?? null);
			} catch (MemberAccessException ex) {
				throw;
			} catch (ArgumentException ex) {
				throw;
			} catch (TargetInvocationException ex) {
				throw;
			} catch (TargetParameterCountException ex) {
				throw;
			} catch (NotSupportedException ex) {
				throw;
			} catch (SecurityException ex) {
				throw;
			} catch (NullReferenceException ex) {
				throw;
			}

			return retval;

		}
	}
}
