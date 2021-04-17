using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using TypeRight.CodeModel;

namespace TypeRight.Workspaces
{
	/// <summary>
	/// This class provides documentation for external refrences
	/// </summary>
	internal class ExternalReferenceDocumentationProvider : DocumentationProvider
	{
		/// <summary>
		/// The private cache of assemblies and their documentation
		/// </summary>
		private Dictionary<string, XmlDocument> _assemblyDocCache = new Dictionary<string, XmlDocument>();

		private Compilation _compilation;

		/// <summary>
		/// Creates a new external reference doc provider
		/// </summary>
		/// <param name="comp">The associated compilation</param>
		public ExternalReferenceDocumentationProvider(Compilation comp)
		{
			_compilation = comp;
		}

		/// <summary>
		/// Trys to add documentation for the given reference
		/// </summary>
		/// <param name="refName">The assembly name of the reference</param>
		/// <param name="refPath">The reference path</param>
		public void TryAddMetadataReference(string refName, string refPath)
		{
			// TODO: could maybe check GAC assemblies here? C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6
			if (!_assemblyDocCache.ContainsKey(refName))
			{
				// See if the XML file exists at that location
				string xmlPath = refPath.Replace(".dll", ".xml");  // Is this.... too simple?
				if (File.Exists(xmlPath))
				{
					XmlDocument doc = new XmlDocument();
					doc.Load(xmlPath);
					_assemblyDocCache.Add(refName, doc);
				}
				else
				{
					_assemblyDocCache.Add(refName, null);
				}
			}
		}

		/// <summary>
		/// Gets the documentation for the given symbol
		/// </summary>
		/// <param name="sym">The symbol</param>
		/// <returns>the XML documentation</returns>
		internal override XmlDocumentation GetDocumentationForSymbol(ISymbol sym)
		{
			// Make sure the reference is added to the doc provider
			MetadataReference reference = _compilation.GetMetadataReference(sym.ContainingAssembly);

			// Display appears to be a filepath, but I guess i don't know for sure... but it works.
			TryAddMetadataReference(sym.ContainingAssembly.ToDisplayString(), reference.Display);

			string assemblyName = sym.ContainingAssembly.ToDisplayString();
			if (!_assemblyDocCache.ContainsKey(assemblyName) || _assemblyDocCache[assemblyName] == null)
			{
				return ExternalReferenceXmlDocumentation.Empty;  // Empty doc
			}
			XmlDocument doc = _assemblyDocCache[assemblyName];

			// Find the member documentation (based on kind)
			string memberName = GetMemberXmlName(sym);
			if (string.IsNullOrEmpty(memberName))
			{
				return ExternalReferenceXmlDocumentation.Empty;
			}

			// Get node
			XmlNode node = doc.SelectSingleNode($"//member[@name='{memberName}']");
			if (node == null)
			{
				return ExternalReferenceXmlDocumentation.Empty;
			}

			// return doc.
			return new ExternalReferenceXmlDocumentation(node);
		}

		/// <summary>
		/// Gets the XML name of the member
		/// </summary>
		/// <param name="sym">The symbol</param>
		/// <returns>The XML name</returns>
		private string GetMemberXmlName(ISymbol sym)
		{
			if (sym.Kind == SymbolKind.Property)
			{
				return $"P:{sym.ToDisplayString()}";
			}
			else if (sym.Kind == SymbolKind.Field)
			{
				return $"F:{sym.ToDisplayString()}";
			}
			else if (sym.Kind == SymbolKind.NamedType)
			{
				return $"T:{sym.ToDisplayString()}";
			}
			return "";
		}
	}

	/// <summary>
	/// XML Documentation for an external reference
	/// </summary>
	class ExternalReferenceXmlDocumentation : XmlDocumentation
	{
		/// <summary>
		/// Gets the empty documentation
		/// </summary>
		public static readonly ExternalReferenceXmlDocumentation Empty = new ExternalReferenceXmlDocumentation();

		/// <summary>
		/// Creates an empty documentation
		/// </summary>
		private ExternalReferenceXmlDocumentation()
		{
			// empty ctor
		}

		/// <summary>
		/// Creates xml doc from an documentation node for the member
		/// </summary>
		/// <param name="fromXmlNode">the xml node</param>
		public ExternalReferenceXmlDocumentation(XmlNode fromXmlNode)
		{
			Summary = GetTextIfExists(fromXmlNode, "summary");
			Returns = GetTextIfExists(fromXmlNode, "returns");

			XmlNodeList paramList = fromXmlNode.SelectNodes("param");
			foreach (XmlNode paramNode in paramList)
			{
				string name = paramNode.Attributes["name"]?.Value;
				if (!string.IsNullOrEmpty(name) && !ParamListProtected.ContainsKey(name))
				{
					ParamListProtected.Add(name, CleanWhitespace(paramNode.InnerText));
				}
			}
		}

		/// <summary>
		/// Gets the doc text if it exists
		/// </summary>
		/// <param name="fromXmlNode">The node to get it from</param>
		/// <param name="elementName">the name of the element to get</param>
		/// <returns>the text</returns>
		private string GetTextIfExists(XmlNode fromXmlNode, string elementName)
		{
			XmlNode elemNode = fromXmlNode.SelectSingleNode(elementName);
			if (elemNode == null)
			{
				return "";
			}
			return CleanWhitespace(elemNode.InnerText);  // slim down whitespace
		}

		/// <summary>
		/// Cleans whitespace from the XML doc
		/// </summary>
		/// <param name="str">The string to clean</param>
		/// <returns>The cleaned whitespace</returns>
		private string CleanWhitespace(string str) => Regex.Replace(str, @"\s+", " ").Trim();
	}
}
