namespace Goblinary.WikiData.Model
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class FeatRankKeyword
	{
		[Key, Column(Order = 1)]
		public string FeatName { get; set; }
		[Key, Column(Order = 2)]
		public int? Feat_Rank { get; set; }
		[Key, Column(Order = 3)]
		public int? KeywordNo { get; set; }
		[Required]
		public string KeywordTypeName { get; set; }
		[Required]
		public string KeywordName { get; set; }

		//[ForeignKey("Feat_Name, Feat_Rank")]
		public virtual FeatRank FeatRank { get; set; }
		//[ForeignKey("KeywordType_Name, Keyword_Name")]
		public virtual Keyword Keyword { get; set; }

		public static Func<FeatRankKeyword, string> ToStringMethod { get; set; }
		public override string ToString() => ToStringMethod != null ? ToStringMethod(this) : base.ToString();
	}
}
