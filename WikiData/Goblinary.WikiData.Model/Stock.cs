namespace Goblinary.WikiData.Model
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class Stock
	{
		public Stock()
		{
			StockItemStocks = new List<StockItemStock>();
			RefiningRecipeIngredients = new List<RefiningRecipeIngredient>();
		}

		[Key]
		public string Name { get; set; }

		[InverseProperty("Stock")]
		public virtual List<StockItemStock> StockItemStocks { get; set; }
		[InverseProperty("Stock")]
		public virtual List<RefiningRecipeIngredient> RefiningRecipeIngredients { get; set; }
	}
}
