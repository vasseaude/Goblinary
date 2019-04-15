namespace Goblinary.WikiData.Model
{

	public interface IAchievementRankFact
	{
		string AchievementName { get; set; }
		int? Achievement_Rank { get; set; }
	}

	public interface IAchievementRankCategoryBonus : IAchievementRankFact
	{
		int? BonusNo { get; set; }
		string CategoryName { get; set; }
		int? Bonus { get; set; }
	}

	public interface IAchievementRankRequirement : IAchievementRankFact
	{
		int? RequirementNo { get; set; }
		int? OptionNo { get; set; }
	}

	public interface IAchievementRankFeatRequirement : IAchievementRankRequirement
	{
		string FeatName { get; set; }
		int? Feat_Rank { get; set; }
	}

	public interface IAchievementRankFlagRequirement : IAchievementRankRequirement
	{
		string FlagName { get; set; }
	}
}
