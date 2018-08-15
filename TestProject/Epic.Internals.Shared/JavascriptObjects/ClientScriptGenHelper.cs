using Epic.Internals.Shared.Utils;
using System;

namespace Epic.Internals.Shared.JavascriptObjects
{
	/// <summary>
	/// Contains helper methods for client script generation
	/// </summary>
	internal class ClientScriptGenHelper
	{
		/// <summary>
		/// Attempts to find a matching extracted type so it can be used as the type
		/// in the extracted client script. 
		/// </summary>
		/// <param name="clientGen">The client script generator</param>
		/// <param name="type">The type to match</param>
		/// <returns>The fully qualified name of the matching type (namespace + typename), or null if one isn't found</returns>
		public static string TryFindExtractedType(ClientScriptGenerator<JavascriptObjectAttribute> clientGen, Type type)
		{
			string bestMatch = null;
			foreach (string oneNamespace in clientGen.NamespaceGroups.Keys)
			{
				foreach (TypeAttributePair<JavascriptObjectAttribute> typeAttrPair in clientGen.NamespaceGroups[oneNamespace])
				{
					//If they are equal, then we found the best match
					if (typeAttrPair.TargetType == type)
					{
						return oneNamespace + "." + TypeUtils.GetTypeName(typeAttrPair.TargetType);
					}
					else if (typeAttrPair.AttributeValue.SurrogateType == type)
					{
						//Next, check the alias types
						bestMatch = oneNamespace + "." + TypeUtils.GetTypeName(typeAttrPair.TargetType);
					}
					else if (string.IsNullOrEmpty(bestMatch) && type.IsSubclassOf(typeAttrPair.TargetType))
					{
						// good enough match
						bestMatch = oneNamespace + "." + TypeUtils.GetTypeName(typeAttrPair.TargetType);
					}
					else if (string.IsNullOrEmpty(bestMatch) && TypeUtils.IsSubclassOfRawGeneric(typeAttrPair.TargetType, type))
					{
						//Otherwise, if the target type is the parent of the type to find, than good enough match
						//for now, but only if we haven't found one yet
						bestMatch = oneNamespace + "." + TypeUtils.GetTypeName(typeAttrPair.TargetType);
					}
				}
			}
			return bestMatch;
		}
	}
}
