using Newtonsoft.Json.Serialization;
using System.Linq;
using TypeRight.CodeModel;
using TypeRight.TypeFilters;

namespace TypeRight.ScriptWriting
{
	public enum NamingStrategyType
	{
		None,
		Camel
	}

	public abstract class NamingStrategy
	{
		private static readonly NamingStrategy _default = Create(NamingStrategyType.None);
		public static NamingStrategy Default => _default;

		public abstract string GetPropertyName(IProperty property);

		public abstract string GetName(string name);

		public static NamingStrategy Create(NamingStrategyType type)
		{
			NamingStrategy defaultStrat;
			switch (type)
			{
				case NamingStrategyType.Camel:
					defaultStrat = new CamelCaseNamingStrategy();
					break;
				default:
					defaultStrat = new NullNamingStrategy();
					break;
			}

			return new TryNameOverrideStrategy(defaultStrat);
		}


		private class CamelCaseNamingStrategy : NamingStrategy
		{
			private static readonly Newtonsoft.Json.Serialization.CamelCaseNamingStrategy s_camelCaseResolver = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy();
			public override string GetPropertyName(IProperty property) => GetName(property.Name);

			public override string GetName(string name)
			{
				return s_camelCaseResolver.GetPropertyName(name, false);
			}
		}

		private class NullNamingStrategy : NamingStrategy
		{
			public override string GetPropertyName(IProperty property) => GetName(property.Name);

			public override string GetName(string name) => name;
		}

		private class TryNameOverrideStrategy : NamingStrategy
		{
			private static readonly TypeFilter s_systemTextJsonAttributeFilter
				= new IsOfTypeFilter(KnownTypes.SystemTextJsonPropertyName);

			private static readonly TypeFilter s_newtonsoftJsonAttributeFilter
				= new IsOfAnyTypeFilter(KnownTypes.NewtonsoftJsonPropertyName_v12, KnownTypes.NewtonsoftJsonPropertyName_pre_v12);

			private readonly NamingStrategy _defaultStrategy;

			public TryNameOverrideStrategy(NamingStrategy defaultStrategy)
			{
				_defaultStrategy = defaultStrategy;
			}

			public override string GetPropertyName(IProperty property)
			{
				string name;
				if (TryFindNewtonsoftOverride(property, out name))
				{
					return name;
				}
				else if (TryFindSystemTextOverride(property, out name))
				{
					return name;
				}
				return _defaultStrategy.GetPropertyName(property);
			}

			public override string GetName(string name) => _defaultStrategy.GetName(name);

			private bool TryFindNewtonsoftOverride(IProperty property, out string name)
			{
				name = null;
				var attr = property.Attributes.FirstOrDefault(a => s_newtonsoftJsonAttributeFilter.Matches(a.AttributeType));
				if (attr == null)
				{
					return false;
				}

				string jsonPropName = nameof(JsonProperty.PropertyName);
				if (!attr.NamedArguments.ContainsKey(jsonPropName))
				{
					return false;
				}

				name = attr.NamedArguments[jsonPropName] as string;
				return true;
			}

			private bool TryFindSystemTextOverride(IProperty property, out string name)
			{
				name = null;
				var attr = property.Attributes.FirstOrDefault(a => s_systemTextJsonAttributeFilter.Matches(a.AttributeType));
				if (attr == null)
				{
					return false;
				}
				name = attr.ConstructorArguments[0] as string;
				return true;
			}

		}
	}
}
