namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public interface IAchievementRankFact
	{
		string Achievement_Name { get; set; }
		int? Achievement_Rank { get; set; }
	}

	public interface IAchievementRankCategoryBonus : IAchievementRankFact
	{
		int? BonusNo { get; set; }
		string Category_Name { get; set; }
		int? Bonus { get; set; }
	}

	public interface IAchievementRankRequirement : IAchievementRankFact
	{
		int? RequirementNo { get; set; }
		int? OptionNo { get; set; }
	}

	public interface IAchievementRankFeatRequirement : IAchievementRankRequirement
	{
		string Feat_Name { get; set; }
		int? Feat_Rank { get; set; }
	}

	public interface IAchievementRankFlagRequirement : IAchievementRankRequirement
	{
		string Flag_Name { get; set; }
	}
}
