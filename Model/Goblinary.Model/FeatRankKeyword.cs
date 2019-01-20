namespace Goblinary.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class FeatRankKeyword
	{
		[Key, Column(Order = 1)]
		public string Feat_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Feat_Rank { get; set; }
		[Key, Column(Order = 3)]
		public int? KeywordNo { get; set; }
		[Required]
		public string KeywordType_Name { get; set; }
		[Required]
		public string Keyword_Name { get; set; }

		[ForeignKey("Feat_Name")]
		public virtual Feat Feat { get; set; }
		[ForeignKey("Feat_Name, Feat_Rank")]
		public virtual FeatRank FeatRank { get; set; }
		[ForeignKey("KeywordType_Name, Keyword_Name")]
		public virtual Keyword Keyword { get; set; }
	}
}
