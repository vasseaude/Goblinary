namespace Goblinary.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class FeatRank
	{
		public FeatRank()
		{
			this.Effects = new List<FeatRankEffect>();
			this.Keywords = new List<FeatRankKeyword>();
		}

		[Key, Column(Order = 1)]
		public string Feat_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Rank { get; set; }

		[ForeignKey("Feat_Name")]
		public virtual Feat Feat { get; set; }

		[InverseProperty("FeatRank")]
		public virtual List<FeatRankEffect> Effects { get; private set; }
		[InverseProperty("FeatRank")]
		public virtual List<FeatRankKeyword> Keywords { get; private set; }
	}
}
