namespace Goblinary.WikiData.Model
{

	public interface IFeatRankFact
	{
		string FeatName { get; set; }
		int? Feat_Rank { get; set; }
		int? OptionNo { get; set; }
		string FactName { get; set; }
	}

	public interface IFeatRankBonus : IFeatRankFact
	{
		int? BonusNo { get; set; }
		decimal? Bonus { get; set; }
	}

	public interface IFeatRankRequirement : IFeatRankFact
	{
		int? RequirementNo { get; set; }
		int? Value { get; set; }
	}
}
