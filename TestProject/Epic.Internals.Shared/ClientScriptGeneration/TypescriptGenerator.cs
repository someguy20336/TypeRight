using Epic.Internals.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Epic.Internals.Shared.ClientScriptGeneration
{
	/// <summary>
	/// Represents a generator that will generate some sort of typescript
	/// </summary>
	/// <typeparam name="TAttributeType"></typeparam>
	public abstract class TypescriptGenerator<TAttributeType> : ClientScriptGenerator<TAttributeType> where TAttributeType : ClientScriptExtractedAttribute
	{

		/// <summary>
		/// Creates a new typescript generator
		/// </summary>
		/// <param name="assemblies">The assemblies to search</param>
		/// <param name="defaultNamespace">The default namespace to use</param>
		protected TypescriptGenerator(List<Assembly> assemblies, string defaultNamespace)
			: base(assemblies, defaultNamespace)
		{ }

		/// <summary>
		/// Writes the contents of the Typescript file
		/// </summary>
		protected override void Write()
		{
			foreach (string oneNamepace in NamespaceGroups.Keys)
			{
				//Add the module
				AddFormatLine(0, "module {0} {{", oneNamepace);
				AddLine(0, "");

				//Generate the Typescript for classes in that namespace
				List<TypeAttributePair<TAttributeType>> typesInNS = NamespaceGroups[oneNamepace];

				foreach (TypeAttributePair<TAttributeType> typeAttr in typesInNS)
				{
					string typeDesc = GetMemberDescription(typeAttr.TargetType);
					AddLine(1, "/**");
					AddFormatLine(1, " * {0}", typeDesc);
					AddFormatLine(1, " * Generated from {0}", typeAttr.TargetType.FullName);
					AddLine(1, " */");

					GenerateTypeDefLine(typeAttr.TargetType);
					AddLine(1, "");

					GenerateTypescriptForTypeMembers(typeAttr);

					AddLine(1, "}");

				}

				//Close the namespace
				AddLine(0, "}");
			}
		}

		/// <summary>
		/// Adds the definition for the type
		/// </summary>
		/// <param name="type">The type to make the definition for</param>
		protected abstract void GenerateTypeDefLine(Type type);

		/// <summary>
		/// Generates the members for a given type/attr pair
		/// </summary>
		/// <param name="typeAttr">The type/attr pair to generate</param>
		protected abstract void GenerateTypescriptForTypeMembers(TypeAttributePair<TAttributeType> typeAttr);

		/// <summary>
		/// Recompiles the typescript.  Got from here
		/// http://stackoverflow.com/questions/14046203/programatically-compile-typescript-in-c
		/// </summary>
		/// <param name="filename">the filename of the new typescript file</param>
		protected override void AfterWrite(string filename)
		{

			// this will invoke "tsc" passing the TS path and other
			// parameters defined in Options parameter
			Process p = new Process();

			ProcessStartInfo psi = new ProcessStartInfo("tsc", "\"" + filename + "\" --sourcemap ");
			// run without showing console windows
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;

            // redirects the compiler error output, so we can read
            // and display errors if any
            psi.RedirectStandardError = true;

            p.StartInfo = psi;

            p.Start();

            // reads the error output
            string msg = p.StandardError.ReadToEnd();

            // make sure it finished executing before proceeding 
            p.WaitForExit();

            // if there were errors, throw an exception
            if (!String.IsNullOrEmpty(msg))
                throw new InvalidOperationException(msg);


		}
	}
}
