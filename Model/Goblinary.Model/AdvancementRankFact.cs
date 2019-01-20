namespace Goblinary.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public abstract class AdvancementRankFact
	{
		[Key, Column(Order = 1)]
		public string Advancement_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Advancement_Rank { get; set; }
		[Key, Column(Order = 3)]
		public int? FactNo { get; set; }
		[Key, Column(Order = 4)]
		public int? OptionNo { get; set; }

		[ForeignKey("Advancement_Name")]
		public virtual Advancement Advancement { get; set; }
		[ForeignKey("Advancement_Name, Advancement_Rank")]
		public virtual AdvancementRank AdvancementRank { get; set; }
	}

	public class AdvancementRankCategoryRequirement : AdvancementRankFact
	{
		[Required]
		public string RequiredCategory_Name { get; set; }
		[Required]
		public int? RequiredPoints { get; set; }

		[ForeignKey("RequiredCategory_Name")]
		public virtual AchievementCategory RequiredCategory { get; set; }
	}

	public class AdvancementRankAdvancementRequirement : AdvancementRankFact
	{
		[Required]
		public string RequiredAdvancement_Name { get; set; }
		[Required]
		public int? RequiredAdvancement_Rank { get; set; }

		[ForeignKey("RequiredAdvancement_Name, RequiredAdvancement_Rank")]
		public virtual AdvancementRank RequiredAdvancementRank { get; set; }
	}

	public class AdvancementRankAchievementRequirement : AdvancementRankFact
	{
		[Required]
		public string RequiredAchievement_Name { get; set; }
		[Required]
		public int? RequiredAchievement_Rank { get; set; }

		[ForeignKey("RequiredAchievement_Name, RequiredAchievement_Rank")]
		public virtual AchievementLevel RequiredAchievementRank { get; set; }
	}

	public class AdvancementRankAbilityRequirement : AdvancementRankFact
	{
		[Required]
		public string RequiredAbility_Name { get; set; }
		[Required]
		public int? RequiredScore { get; set; }

		[ForeignKey("RequiredAbility_Name")]
		public virtual Ability RequiredAbility { get; set; }
	}

	public class AdvancementRankAbilityBonus : AdvancementRankFact
	{
		[Required]
		public string BonusAbility_Name { get; set; }
		[Required]
		public decimal? BonusScore { get; set; }

		[ForeignKey("BonusAbility_Name")]
		public virtual Ability BonusAbility { get; set; }
	}
}
