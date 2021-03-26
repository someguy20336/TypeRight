using Newtonsoft.Json.Serialization;
using System.Linq;
using TypeRight.CodeModel;
using TypeRight.TypeFilters;

namespace TypeRight.ScriptWriting
{
	public enum PropertyNamingStrategyType
	{
		None,
		Camel
	}

	public abstract class PropertyNamingStrategy
	{
		private static readonly PropertyNamingStrategy _default = Create(PropertyNamingStrategyType.None);
		public static PropertyNamingStrategy Default => _default;

		public abstract string GetName(IProperty property);

		// TODO method to create "named" (i.e. using camel case)

		public static PropertyNamingStrategy Create(PropertyNamingStrategyType type)
		{
			PropertyNamingStrategy defaultStrat;
			switch (type)
			{
				case PropertyNamingStrategyType.Camel:
					defaultStrat = new CamelCasePropertyNamingStrategy();
					break;
				default:
					defaultStrat = new NullPropertyNamingStrategy();
					break;
			}

			return new TryNameOverrideStrategy(defaultStrat);
		}


		private class CamelCasePropertyNamingStrategy : PropertyNamingStrategy
		{
			private static readonly CamelCaseNamingStrategy s_camelCaseResolver = new CamelCaseNamingStrategy();
			public override string GetName(IProperty property)
			{
				return s_camelCaseResolver.GetPropertyName(property.Name, false);
			}
		}

		private class NullPropertyNamingStrategy : PropertyNamingStrategy
		{
			public override string GetName(IProperty property)
			{
				return property.Name;
			}
		}

		private class TryNameOverrideStrategy : PropertyNamingStrategy
		{
			private static readonly TypeFilter s_systemTextJsonAttributeFilter
				= new IsOfTypeFilter(KnownTypes.SystemTextJsonPropertyName);

			private static readonly TypeFilter s_newtonsoftJsonAttributeFilter
				= new IsOfAnyTypeFilter(KnownTypes.NewtonsoftJsonPropertyName_v12, KnownTypes.NewtonsoftJsonPropertyName_pre_v12);

			private readonly PropertyNamingStrategy _defaultStrategy;

			public TryNameOverrideStrategy(PropertyNamingStrategy defaultStrategy)
			{
				_defaultStrategy = defaultStrategy;
			}

			public override string GetName(IProperty property)
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
				return _defaultStrategy.GetName(property);
			}

			private bool TryFindNewtonsoftOverride(IProperty property, out string name)
			{
				name = null;
				var attr = property.Attributes.FirstOrDefault(a => s_newtonsoftJsonAttributeFilter.Evaluate(a.AttributeType));
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
				var attr = property.Attributes.FirstOrDefault(a => s_systemTextJsonAttributeFilter.Evaluate(a.AttributeType));
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
