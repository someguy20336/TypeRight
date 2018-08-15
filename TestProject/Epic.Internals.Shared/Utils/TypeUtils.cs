using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Epic.Internals.Shared.Utils
{
    /// <summary>
    /// This class has a bunch of utilities commonly used
    /// with Types
    /// </summary>
    /// <typeparam name="TType">The Type we are using the utility for</typeparam>
    public class TypeUtils<TType>
    {

        /// <summary>
        /// Gets the name of a property (typesafe, so you don't have to hard code 
        /// a string)
        /// Look out of nameof, which seems to be coming in a later C# version.
        /// https://msdn.microsoft.com/en-us/library/dn986596.aspx
        /// </summary>
        /// <typeparam name="TProp">The type of the property you are checking (usually 
        /// you don't need to explicity define this)</typeparam>
        /// <param name="expression">the expression, which just selects the property name
        /// (for example, ex => ex.Property)</param>
        /// <returns>The property name</returns>
        public static string NameOfProperty<TProp>(Expression<Func<TType, TProp>> expression)
        {
            var body = expression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("'expression' should be a member expression");
            return body.Member.Name;
        }

        /// <summary>
        /// Checks if <typeparamref name="TType"/> is a numeric type
        /// </summary>
        /// <returns>true if the type is numeric</returns>
        public static bool IsNumericType()
        {
            Type type = typeof(TType);
            return TypeUtils.IsNumericType(type);
        }
    }

    /// <summary>
    /// Utilities commonly used with Types (not strongly typed)
    /// </summary>
    public static class TypeUtils
    {
        /// <summary>
        /// Gets the default value for a given type (at runtime). 
        /// Normally, at compile time, you would use default(type), but
        /// we can't really do that at run-time
        /// </summary>
        /// <param name="type">The type to get the default value for</param>
        /// <returns>The default value</returns>
        public static object GetDefaultValue(this Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);

            return null;
        }

        /// <summary>
        /// Checks to see if the given type is of IList
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>true if the type is IList</returns>
        public static bool IsList(Type type)
        {
            Type foundInterface = type.GetInterface("IList");

            return foundInterface != null;
        }

        /// <summary>
        /// Checks if the given type is a numeric type
        /// </summary>
        /// <param name="type">The type of check</param>
        /// <returns>true if the type is numeric</returns>
        public static bool IsNumericType(Type type)
        {
            if (type == null) return false;

            // from http://stackoverflow.com/a/5182747/172132 
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }
            return false;
        }

        /// <summary>
        /// Determines whether a specific type is an object type.
        /// Basically, this is anything that isn't a primitive type or string
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>True if it is an object type</returns>
        public static bool IsClass(Type type)
        {
            if (type.IsPrimitive)
            {
                return false;
            }
            else if (type.Equals(typeof(string)))
            {
                return false;
            }
            return true;
        }

		/// <summary>
		/// Returns true if the type toCheck is derived from the given generic.  For example:
		/// 
		///		toCheck : generic[string]
		///	
		/// To check inherits from the generic with the type param = string.  this checks for that
		/// </summary>
		/// <param name="generic">The generic class</param>
		/// <param name="toCheck">The type to check</param>
		/// <returns>True if toCheck inherics from the generic type</returns>
		public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
		{
			while (toCheck != null && toCheck != typeof(object))
			{
				var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
				if (generic == cur)
				{
					return true;
				}
				toCheck = toCheck.BaseType;
			}
			return false;
		}

		/// <summary>
		/// Gets a friendly name of type, removing any generic artifacts
		/// </summary>
		/// <param name="type">The type to retrieve the name for</param>
		/// <returns>The type name</returns>
		public static string GetTypeName(Type type)
		{
			//Remove the ticks for generic types
			return type.Name.Split('`')[0];
		}
    }
}
