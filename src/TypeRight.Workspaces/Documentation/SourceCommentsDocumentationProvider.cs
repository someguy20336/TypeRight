using TypeRight.CodeModel;
using Microsoft.CodeAnalysis;
using System.Xml;

namespace TypeRight.Workspaces
{
	/// <summary>
	/// Gets documentation from the source code
	/// </summary>
	internal class SourceCommentsDocumentationProvider : DocumentationProvider
	{
		/// <summary>
		/// The symbol to get the documentation for
		/// </summary>
		/// <param name="sym">The symbol</param>
		/// <returns>The XML Documentation</returns>
		internal override XmlDocumentation GetDocumentationForSymbol(ISymbol sym)
		{
			return new SourceCommentsXmlDocumentation(sym);
		}
	}

	/// <summary>
	/// XML documentation from the source code
	/// </summary>
	internal class SourceCommentsXmlDocumentation : XmlDocumentation
	{
		/// <summary>
		/// Creates a new xml docs object for a given symbol
		/// </summary>
		/// <param name="fromSymbol">The symbol to use</param>
		public SourceCommentsXmlDocumentation(ISymbol fromSymbol)
		{
			string comments = fromSymbol.GetDocumentationCommentXml();
			if (!string.IsNullOrWhiteSpace(comments))
			{
				try
				{
					XmlDocument doc = new XmlDocument();
					doc.LoadXml(comments);

					// Summary
					string text = doc.SelectSingleNode("member/summary")?.InnerText ?? "";
					Summary = text.Trim();

					text = doc.SelectSingleNode("member/returns")?.InnerText ?? "";
					Returns = text.Trim();

					foreach (XmlNode oneParamNode in doc.SelectNodes("member/param"))
					{
						ParamListProtected.Add(oneParamNode.Attributes["name"].Value, oneParamNode.InnerText);
					}
				}
				catch
				{
					// can't do anything... the XML is malformed
				}
			}
		}
	}
}
