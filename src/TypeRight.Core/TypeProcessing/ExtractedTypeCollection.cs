using TypeRight.CodeModel;
using System.Collections.Generic;
using System.Collections;
using TypeRight.TypeFilters;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// Collection of extracted types
	/// </summary>
	public class ExtractedTypeCollection : IEnumerable<ExtractedType>
	{
		private readonly TypeFactory _typeFactory;
		private List<MvcController> _controllers;
		private readonly Dictionary<string, INamedType> _controllerTypes = new Dictionary<string, INamedType>();

		/// <summary>
		/// Creates a new extracted type collection from the given package
		/// </summary>
		/// <param name="settings">The settings</param>
		public ExtractedTypeCollection(ProcessorSettings settings)
		{
			_typeFactory = new TypeFactory(settings);

		}

		/// <summary>
		/// Registers a type to the optional target path
		/// </summary>
		/// <param name="namedType">The type to register</param>
		/// <param name="targetPath">The optional target path, relative to the root of the project</param>
		public void RegisterType(INamedType namedType, string targetPath = null)
		{
			if (!_typeFactory.ContainsNamedType(namedType))
			{
				_typeFactory.RegisterNamedType(namedType, targetPath);
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
		public IEnumerable<MvcController> GetMvcControllers()
		{
			EnsureCompiledControllers();
			return _controllers;
		}

		private void EnsureCompiledControllers()
		{
			if (_controllers == null)
			{
				_controllers = new List<MvcController>();
				foreach (INamedType controllerType in _controllerTypes.Values)
				{
					MvcController controllerInfo = new MvcController(controllerType, CommonFilters.ScriptActionAttributeTypeFilter, _typeFactory);
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
			return _typeFactory.RegisteredTypes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
