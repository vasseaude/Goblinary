namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public interface IFeatRankFact
	{
		string Feat_Name { get; set; }
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
