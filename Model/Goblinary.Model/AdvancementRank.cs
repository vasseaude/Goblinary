namespace Goblinary.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class AdvancementRank
	{
		[Key, Column(Order = 1)]
		public string Advancement_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Rank { get; set; }
		public int? ExpCost { get; set; }
		public int? CoinCost { get; set; }

		[ForeignKey("Advancement_Name")]
		public virtual Advancement Advancement { get; set; }
	}
}
