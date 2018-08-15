using System;

namespace Epic.Internals.Shared.ClientScriptGeneration
{
    /// <summary>
	/// This attribute marks an object for extraction as a client script
    /// object to help with intellisense
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class ClientScriptObjectAttribute : ClientScriptExtractedAttribute
    {
        /// <summary>
        /// Gets or sets a type that this current type can also
        /// represent.  This is used for linking extracted property types
        /// with a type that was also extracted.  This is typically used when
        /// a more general type is used instead of the current type.
        /// Example: FilterValue[TType] is extracted, but it's base type, FilterValue,
        /// is generally used in code to keep it more generic.  Adding FilterValue as the
        /// AliasType will allow the generator to properly link these two together.
        /// </summary>
        public Type SurrogateType { get; set; }

        /// <summary>
		/// Creates a new client script object generator with the default namespace
        /// </summary>
        public ClientScriptObjectAttribute()
            : this("")
        {
        }

        /// <summary>
		/// Creates a new client script object generator with the given namespace
        /// </summary>
        /// <param name="overrideNamespace">The namespace (with no period at the end)</param>
        public ClientScriptObjectAttribute(string overrideNamespace)
            : base(overrideNamespace)
        {
        }
    }
}
