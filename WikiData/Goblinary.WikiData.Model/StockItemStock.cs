namespace Goblinary.WikiData.Model
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class StockItemStock
	{
		[Key, Column(Order = 1)]
		public string StockItemName { get; set; }
		[Key, Column(Order = 2)]
		public string StockName { get; set; }

		[ForeignKey("StockItem_Name")]
		public virtual StockItem StockItem { get; set; }
		[ForeignKey("Stock_Name")]
		public virtual Stock Stock { get; set; }
	}
}
