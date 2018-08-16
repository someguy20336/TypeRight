using TypeRight.CodeModel;
using System.Collections;
using System.Collections.Generic;

namespace TypeRight.Packages
{
	/// <summary>
	/// Represents a collection of named type items.  Each "FullName" must be unique.
	/// </summary>
	/// <typeparam name="TItemType">The item type</typeparam>
	public class ScriptItemCollection<TItemType> : IEnumerable<TItemType> where TItemType : ITypeWithFullName
	{
		/// <summary>
		/// The private backing class of full names to the type
		/// </summary>
		private Dictionary<string, TItemType> _classIndex = new Dictionary<string, TItemType>();

		/// <summary>
		/// Gets the <typeparamref name="TItemType"/> with the given full name
		/// </summary>
		/// <param name="fullName">The full name to find</param>
		/// <returns>The item</returns>
		public TItemType this[string fullName]
		{
			get
			{
				if (ContainsItemWithName(fullName))
				{
					return _classIndex[fullName];
				}
				return default(TItemType);
			}
		}

		/// <summary>
		/// Gets whether this collection contains an item with the given name
		/// </summary>
		/// <param name="fullName">The name to check</param>
		/// <returns>True if it exists</returns>
		public bool ContainsItemWithName(string fullName)
		{
			return _classIndex.ContainsKey(fullName);
		}

		/// <summary>
		/// Gets the enumerator for this collection
		/// </summary>
		/// <returns>The <typeparamref name="TItemType"/> enumerator</returns>
		public IEnumerator<TItemType> GetEnumerator()
		{
			return _classIndex.Values.GetEnumerator();
		}

		/// <summary>
		/// Gets the enumerator for this collection
		/// </summary>
		/// <returns>The <typeparamref name="TItemType"/> enumerator</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return _classIndex.Values.GetEnumerator();
		}

		/// <summary>
		/// Adds the given item to this collection
		/// </summary>
		/// <param name="item">The item to add</param>
		internal void Add(TItemType item)
		{
			_classIndex.Add(item.FullName, item);
		}
	}
}
