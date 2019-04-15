namespace Goblinary.Common
{
	using System;
	using System.Collections.Generic;


	public interface IListWrapper<TDerived>
	{
		IList<TDerived> InnerList { get; }
	}

	public class VariantList<TBase, TDerived> : IList<TBase>, IListWrapper<TDerived>
			where TDerived : TBase
	{
		public VariantList(IList<TDerived> list)
		{
		    _innerList = list is IListWrapper<TDerived> wrapper ? wrapper.InnerList : list;
		}

		private readonly IList<TDerived> _innerList;

		IList<TDerived> IListWrapper<TDerived>.InnerList => _innerList;

	    public int IndexOf(TBase item) => _innerList.IndexOf((TDerived)item);

	    public void Insert(int index, TBase item)
		{
			_innerList.Insert(index, (TDerived)item);
		}

		public void RemoveAt(int index)
		{
			_innerList.RemoveAt(index);
		}

		public TBase this[int index]
		{
			get => (TBase)_innerList[index];
		    set => _innerList[index] = (TDerived)value;
		}

		public void Add(TBase item)
		{
			_innerList.Add((TDerived)item);
		}

		public void Clear()
		{
			_innerList.Clear();
		}

		public bool Contains(TBase item)
		{
			return _innerList.Contains((TDerived)item);
		}

		public void CopyTo(TBase[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public int Count => _innerList.Count;

	    public bool IsReadOnly => _innerList.IsReadOnly;

	    public bool Remove(TBase item)
		{
			return _innerList.Remove((TDerived)item);
		}

		public IEnumerator<TBase> GetEnumerator()
		{
			return (IEnumerator<TBase>)_innerList.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ((System.Collections.IEnumerable)_innerList).GetEnumerator();
		}
	}
}
