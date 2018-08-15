using System;

namespace Epic.Internals.Shared.ClientScriptGeneration
{
    /// <summary>
	/// Marks an enum for client script extraction
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class ClientScriptEnumAttribute : ClientScriptExtractedAttribute
    {

        /// <summary>
		/// Creates a new client script enum with the default namespace
        /// </summary>
        public ClientScriptEnumAttribute()
            : this("")
        {
        }

        /// <summary>
		/// Creates a new client script enum with the given namespace
        /// </summary>
        /// <param name="overrideNamespace">The namespace (with no period at the end)</param>
        public ClientScriptEnumAttribute(string overrideNamespace)
            : base(overrideNamespace)
        {
        }
    }

    /// <summary>
    /// This attribute is used for enum fields to provide any
    /// extra data to the client script enum
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class JavascriptEnumFieldDataAttribute : Attribute
    {
        /// <summary>
        /// Gets an array of the extra data.  Right now, it isn't too smart.  It just expects
        /// things like:
        /// 
        /// "data1: 8"
        /// "data2: 12"
        /// </summary>
        public string[] ExtraData { get; private set; }

        /// <summary>
        /// Creates a new field data attribute with the given data
        /// </summary>
        /// <param name="extraData">The extra data to use</param>
        public JavascriptEnumFieldDataAttribute(params string[] extraData)
        {
            ExtraData = extraData;
        }
    }

}
