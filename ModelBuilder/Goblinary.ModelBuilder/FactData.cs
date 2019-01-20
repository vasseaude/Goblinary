namespace Goblinary.ModelBuilder
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	internal class FactData
	{
		internal static List<FactData> GetFacts(string factsText)
		{
			List<FactData> facts = new List<FactData>();
			int factNo = 1;
			foreach (string fact in factsText.Replace(" or ", "|").Replace(", ", ",").Split(',').Where(x => !string.IsNullOrEmpty(x)))
			{
				int optionNo = 1;
				foreach (string option in fact.Split('|'))
				{
					string[] parts = option.Split('=');
					facts.Add(new FactData
					{
						FactNo = factNo,
						OptionNo = optionNo++,
						Name = parts[0].Trim(),
						Value = parts[1].Trim()
					});
				}
				factNo++;
			}
			return facts;
		}

		internal static string GetFactString(DataRow row, IEnumerable<string> cats)
		{
			StringBuilder sb = new StringBuilder();
			string pad = "";
			foreach (var cat in cats)
			{
				if (row.Table.Columns.Contains(cat) && row[cat] != DBNull.Value)
				{
					sb.AppendFormat("{0}{1}={2}", pad, cat, row[cat]);
					pad = ", ";
				}
			}
			return sb.ToString();
		}

		public int FactNo { get; set; }
		public int OptionNo { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
	}
}
