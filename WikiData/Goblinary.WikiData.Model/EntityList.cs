namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class EntityList<T> : List<T>
	{
		public override string ToString()
		{
			string pad = "";
			StringBuilder sb = new StringBuilder();
			foreach (string item in (
					from i in this
					select i.ToString()
				))
			{
				sb.AppendFormat("{0}{1}", pad, item);
				pad = "<br/>";
			}
			return sb.ToString();
		}
	}
}
