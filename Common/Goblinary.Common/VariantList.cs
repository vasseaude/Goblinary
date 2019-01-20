namespace Goblinary.Common
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public interface IListWrapper<TDerived>
	{
		IList<TDerived> InnerList { get; }
	}

	public class VariantList<TBase, TDerived> : IList<TBase>, IListWrapper<TDerived>
			where TDerived : TBase
	{
		public VariantList(IList<TDerived> list)
		{
			if (list is IListWrapper<TDerived>)
				this.innerList = ((IListWrapper<TDerived>)list).InnerList;
			else
				this.innerList = list;
		}

		private IList<TDerived> innerList;

		IList<TDerived> IListWrapper<TDerived>.InnerList { get { return this.innerList; } }

		public int IndexOf(TBase item)
		{
			return this.innerList.IndexOf((TDerived)item);
		}

		public void Insert(int index, TBase item)
		{
			this.innerList.Insert(index, (TDerived)item);
		}

		public void RemoveAt(int index)
		{
			this.innerList.RemoveAt(index);
		}

		public TBase this[int index]
		{
			get { return (TBase)this.innerList[index]; }
			set
			{
				this.innerList[index] = (TDerived)value;
			}
		}

		public void Add(TBase item)
		{
			this.innerList.Add((TDerived)item);
		}

		public void Clear()
		{
			this.innerList.Clear();
		}

		public bool Contains(TBase item)
		{
			return this.innerList.Contains((TDerived)item);
		}

		public void CopyTo(TBase[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public int Count
		{
			get { return this.innerList.Count; }
		}

		public bool IsReadOnly
		{
			get { return this.innerList.IsReadOnly; }
		}

		public bool Remove(TBase item)
		{
			return this.innerList.Remove((TDerived)item);
		}

		public IEnumerator<TBase> GetEnumerator()
		{
			return (IEnumerator<TBase>)this.innerList.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ((System.Collections.IEnumerable)this.innerList).GetEnumerator();
		}
	}
}
