using TypeRight.CodeModel;
using TypeRight.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// Collection of extracted types
	/// </summary>
	public class ExtractedTypeCollection
	{
		private TypeTable _typeTable;

		private List<MvcControllerInfo> _controllers = new List<MvcControllerInfo>();

		/// <summary>
		/// Creates a new extracted type collection from the given package
		/// </summary>
		/// <param name="package">the package</param>
		/// <param name="settings">The settings</param>
		public ExtractedTypeCollection(ScriptPackage package, ProcessorSettings settings)
		{
			settings = settings ?? new ProcessorSettings();
			_typeTable = new TypeTable(package, settings);

			foreach (INamedType controllerType in package.MvcControllers)
			{
				MvcControllerInfo controllerInfo = new MvcControllerInfo(controllerType, settings.MvcActionFilter, _typeTable);
				if (controllerInfo.Actions.Count > 0)
				{
					_controllers.Add(controllerInfo);
				}
			}
		}
		
		/// <summary>
		/// Gets all of the extracted reference types
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ExtractedReferenceType> GetReferenceTypes() => _typeTable.GetReferenceTypes();

		/// <summary>
		/// Gets all of the extracted Enums
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ExtractedEnumType> GetEnums() => _typeTable.GetEnums();

		/// <summary>
		/// Gets all of the extracted controllers
		/// </summary>
		/// <returns></returns>
		public IEnumerable<MvcControllerInfo> GetMvcControllers() => _controllers;
	}
}
