namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	using Goblinary.Common;

	public class StockItemStock
	{
		[Key, Column(Order = 1)]
		public string StockItem_Name { get; set; }
		[Key, Column(Order = 2)]
		public string Stock_Name { get; set; }

		[ForeignKey("StockItem_Name")]
		public virtual StockItem StockItem { get; set; }
		[ForeignKey("Stock_Name")]
		public virtual Stock Stock { get; set; }
	}
}
