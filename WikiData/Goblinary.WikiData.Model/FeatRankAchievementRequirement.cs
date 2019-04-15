namespace Goblinary.WikiData.Model
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class FeatRankAchievementRequirement : IFeatRankRequirement
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
		public string AchievementName { get; set; }
		[Required]
		public int? Achievement_Rank { get; set; }

		[ForeignKey("Feat_Name, Feat_Rank")]
		public virtual FeatRank FeatRank { get; set; }
		[ForeignKey("Achievement_Name, Achievement_Rank")]
		public virtual AchievementRank AchievementRank { get; set; }

		[NotMapped]
		string IFeatRankFact.FactName
		{
			get => AchievementName;
		    set => AchievementName = value;
		}

		[NotMapped]
		int? IFeatRankRequirement.Value
		{
			get => Achievement_Rank;
		    set { Achievement_Rank = value; }
		}

		public static Func<FeatRankAchievementRequirement, string> ToStringMethod { get; set; }
		public override string ToString() => ToStringMethod != null ? ToStringMethod(this) : base.ToString();
	}
}
