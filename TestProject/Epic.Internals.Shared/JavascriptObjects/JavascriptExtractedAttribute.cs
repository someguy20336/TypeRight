using System;

namespace Epic.Internals.Shared.JavascriptObjects
{
    /// <summary>
    /// This is a base class for all attributes that will generate some sort 
    /// of javascript. 
    /// </summary>
    public abstract class JavascriptExtractedAttribute : Attribute
    {

        /// <summary>
        /// Gets or sets the namespace to use as an override.  If null or empty,
        /// the default namespace will be used
        /// </summary>
        public string OverrideNamespace { get; set; }

        /// <summary>
        /// Creates a new javascript extracted attribute with the default namespace
        /// </summary>
        public JavascriptExtractedAttribute()
            : this("")
        {
        }

        /// <summary>
        /// Creates a new javascript extracted attribute with the given namespace
        /// </summary>
        /// <param name="overrideNamespace">The namespace (with no period at the end)</param>
        public JavascriptExtractedAttribute(string overrideNamespace)
        {
            OverrideNamespace = overrideNamespace;
        }
    }
}
