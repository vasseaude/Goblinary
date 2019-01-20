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

	public class Stock
	{
		public Stock()
		{
			this.StockItemStocks = new List<StockItemStock>();
			this.RefiningRecipeIngredients = new List<RefiningRecipeIngredient>();
		}

		[Key]
		public string Name { get; set; }

		[InverseProperty("Stock")]
		public virtual List<StockItemStock> StockItemStocks { get; set; }
		[InverseProperty("Stock")]
		public virtual List<RefiningRecipeIngredient> RefiningRecipeIngredients { get; set; }
	}
}
