namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class FeatRankTrainerLevel
	{
		[Key, Column(Order = 1)]
		public string Feat_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Feat_Rank { get; set; }
		[Key, Column(Order = 3)]
		public string Trainer_Name { get; set; }
		[Required]
		public int? Level { get; set; }

		[ForeignKey("Feat_Name")]
		public virtual AdvancementFeat AdvancementFeat { get; set; }
		[ForeignKey("Trainer_Name")]
		public virtual Trainer Trainer { get; set; }
	}
}
