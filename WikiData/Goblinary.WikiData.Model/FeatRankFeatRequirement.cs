namespace Goblinary.WikiData.Model
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class FeatRankFeatRequirement : IFeatRankRequirement
	{
		[Key, Column(Order = 1)]
		public string FeatName { get; set; }
		[Key, Column(Order = 2)]
		public int? Feat_Rank { get; set; }
		[Key, Column(Order = 3)]
		public int? RequirementNo { get; set; }
		[Key, Column(Order = 4)]
		public int? OptionNo { get; set; }
		[Required]
		public string RequiredFeat_Name { get; set; }
		[Required]
		public int? RequiredFeat_Rank { get; set; }

		[ForeignKey("Feat_Name, Feat_Rank")]
		public virtual FeatRank FeatRank { get; set; }
		[ForeignKey("RequiredFeat_Name, RequiredFeat_Rank")]
		public virtual FeatRank RequiredFeatRank { get; set; }

		[NotMapped]
		string IFeatRankFact.FactName
		{
			get => RequiredFeat_Name;
		    set => RequiredFeat_Name = value;
		}

		[NotMapped]
		int? IFeatRankRequirement.Value
		{
			get => RequiredFeat_Rank;
		    set => RequiredFeat_Rank = value;
		}

		public static Func<FeatRankFeatRequirement, string> ToStringMethod { get; set; }
		public override string ToString() => ToStringMethod != null ? ToStringMethod(this) : base.ToString();
	}
}
