namespace Goblinary.WikiData.Model
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class EntityList<T> : List<T>
	{
		public override string ToString()
		{
			var pad = "";
			var sb = new StringBuilder();
			foreach (var item in from i in this
			    select i.ToString())
			{
				sb.AppendFormat("{0}{1}", pad, item);
				pad = "<br/>";
			}
			return sb.ToString();
		}
	}
}
