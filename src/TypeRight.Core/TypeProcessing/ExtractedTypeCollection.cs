using TypeRight.CodeModel;
using System.Collections.Generic;
using System.Collections;
using TypeRight.TypeFilters;
using TypeRight.Attributes;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// Collection of extracted types
	/// </summary>
	public class ExtractedTypeCollection : IEnumerable<ExtractedType>
	{
		private static TypeFilter s_mvcActionFilter = new IsOfTypeFilter(typeof(ScriptActionAttribute).FullName);

		private readonly TypeTable _typeTable;
		private readonly ProcessorSettings _settings;
		private List<MvcControllerInfo> _controllers;
		private readonly Dictionary<string, INamedType> _controllerTypes = new Dictionary<string, INamedType>();

		/// <summary>
		/// Creates a new extracted type collection from the given package
		/// </summary>
		/// <param name="settings">The settings</param>
		public ExtractedTypeCollection(ProcessorSettings settings)
		{
			_settings = settings ?? new ProcessorSettings();
			_typeTable = new TypeTable(settings);

		}

		/// <summary>
		/// Registers a type to the optional target path
		/// </summary>
		/// <param name="namedType">The type to register</param>
		/// <param name="targetPath">The optional target path, relative to the root of the project</param>
		public void RegisterType(INamedType namedType, string targetPath = null)
		{
			if (!_typeTable.ContainsNamedType(namedType))
			{
				_typeTable.AddNamedType(namedType, targetPath);
			}
		}

		/// <summary>
		/// Registers the
		/// </summary>
		/// <param name="controllerType"></param>
		public void RegisterController(INamedType controllerType)
		{
			if (!_controllerTypes.ContainsKey(controllerType.FullName))
			{
				_controllerTypes.Add(controllerType.FullName, controllerType);
			}			
		}
		
		/// <summary>
		/// Gets all of the extracted controllers. 
		/// </summary>
		/// <returns></returns>
		public IEnumerable<MvcControllerInfo> GetMvcControllers()
		{
			EnsureCompiledControllers();
			return _controllers;
		}

		private void EnsureCompiledControllers()
		{
			if (_controllers == null)
			{
				_controllers = new List<MvcControllerInfo>();
				foreach (INamedType controllerType in _controllerTypes.Values)
				{
					MvcControllerInfo controllerInfo = new MvcControllerInfo(controllerType, s_mvcActionFilter, _typeTable);
					if (controllerInfo.Actions.Count > 0)
					{
						_controllers.Add(controllerInfo);
					}
				}
			}
		}

		/// <summary>
		/// Gets the enumerator for this collection
		/// </summary>
		/// <returns></returns>
		public IEnumerator<ExtractedType> GetEnumerator()
		{
			return _typeTable.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
