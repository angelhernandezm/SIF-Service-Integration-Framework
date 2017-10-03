using System.Reflection;

namespace Pohgee.Framework.LightIoC.Concrete {
	internal class Consts {
		public const string ToPropName = "to";
		public const string NamePropName = "name";
		public const string TypePropName = "type";
		public const string MapElementName = "map";
		public const string EnabledPropName = "enabled";
		public const string LifeSpanPropName = "lifeSpan";
		public const string ConfigSectionName = "LightIoC";
		public const string RegisterElementName = "register";
		public const string MappingsPropertyName = "mappings";
		public const string AbstractionPropName = "abstraction";
		public const string AssemblyFqnPropName = "assemblyFQN";
		public const string PreJittingPropertyName = "Pre-Jitting";
		public const string RegistrationsPropertyName = "registrations";
		public const string InstanceRequiredPropName = "instanceRequired";
		public const string MappingGenericError = "Error: {0} - Description: {1}.\n\r";
		public const string ErrorCountSummary = "\n\rNumber of errors encountered: {0}\n\r";
		public const string TypeDictionaryIsNull = "Dictionary containing type information is null.";
		public const string TypeDictionaryIsEmpty = "Dictionary containing type information is empty.";
		public const BindingFlags JitMethodFlag = BindingFlags.DeclaredOnly | BindingFlags.NonPublic |
												  BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
		public const BindingFlags CtorBuildFlag = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
		public const string NullParametersException = "One of the parameters expected is null.";
		public const string MissingConcreteName = "Concrete implementation name was not provided.";
		public const string TypeWasExpected = "A Type was expected but a null object was passed instead";
		public const string DifferentTypeExpected = "An interface/class of type {0} was expected instead of {1}";
		public const string ConcreteImplementationNotFound = "Concrete implementation with name {0} was not found";
		public const string MappingInformationMissing = "Unable to find a concrete implementation for interface {0}.";
		public const string GenericMappingErrorSummary = "There have been a few mapping errors, please see details below:";
		public const string ConcreteClassDoesntImplementInterface = "The class specified {0} does not implement the {1} interface";
		public const string StaticWasPassedInsteadOfInstance = "An static method/property was passed when an instance was expected.";
		public const string MappingRegistrationMismatch = "There's a registration/mapping mismatch or the configuration section is missing.";
		public const string NotSuitableConstructorFound = "Unable to create instance of an object, because a suitable constructor was not found.";
		public const BindingFlags ReflectionMethodFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod;
	}
}
