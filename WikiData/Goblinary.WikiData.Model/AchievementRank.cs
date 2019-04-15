namespace Goblinary.WikiData.Model
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public abstract class AchievementRank
	{
		[Key, Column(Order = 1)]
		public string AchievementName { get; set; }
		[Key, Column(Order = 2)]
		public int? Rank { get; set; }
		[Required]
		public string DisplayName { get; set; }
		[Required]
		public string InfluenceGain { get; set; }
		public string Description { get; set; }

		[ForeignKey("Achievement_Name")]
		public virtual Achievement Achievement { get; set; }
	}

	public abstract class CategoryBonusAchievementRank : AchievementRank
	{
	    protected CategoryBonusAchievementRank() => CategoryBonuses = new List<AchievementRankCategoryBonus>();

        [InverseProperty("AchievementRank")]
		public virtual List<AchievementRankCategoryBonus> CategoryBonuses { get; set; }
	}

	public abstract class CounterAchievementRank : CategoryBonusAchievementRank
	{
		[Required]
		public string CounterName { get; set; }
		[Required]
		public int? Value { get; set; }
	}

	public class PlayerKillAchievementRank : CounterAchievementRank { }

	public class InteractionAchievementRank : CounterAchievementRank, IKeywordAchievementRank
	{
		public string InteractionKeywordName { get; set; }

		[NotMapped]
		string IKeywordAchievementRank.Keyword
		{
			get => InteractionKeywordName;
		    set => InteractionKeywordName = value;
		}
	}

	public class NPCKillAchievementRank : CounterAchievementRank, IKeywordAchievementRank
	{
		[Required]
		public string RaceName { get; set; }

		[NotMapped]
		string IKeywordAchievementRank.Keyword
		{
			get => RaceName;
		    set => RaceName = value;
		}
	}

	public class WeaponKillAchievementRank : CounterAchievementRank, IKeywordAchievementRank
	{
		[Required]
		public string WeaponProficiencyName { get; set; }

		[NotMapped]
		string IKeywordAchievementRank.Keyword
		{
			get => WeaponProficiencyName;
		    set => WeaponProficiencyName = value;
		}
	}

	public abstract class FlagAchievementRank : CategoryBonusAchievementRank
	{
		[Required]
		public string FlagName { get; set; }
	}

	public class SettlementLocationAchievementRank : FlagAchievementRank, IKeywordAchievementRank
	{
		[Required]
		public string SettlementName { get; set; }

		[NotMapped]
		string IKeywordAchievementRank.Keyword
		{
			get => SettlementName;
		    set => SettlementName = value;
		}
	}

	public class SpecialLocationAchievementRank : FlagAchievementRank, IKeywordAchievementRank
	{
		[Required]
		public string LocationName { get; set; }

		[NotMapped]
		string IKeywordAchievementRank.Keyword
		{
			get => LocationName;
		    set => LocationName = value;
		}
	}

	public abstract class CraftAchievementRank : FlagAchievementRank
	{
		[Required]
		public string FeatName { get; set; }
		[Required]
		public int? Tier { get; set; }
		[Required]
		public string RarityName { get; set; }
		[Required]
		public int? Upgrade { get; set; }

		[ForeignKey("Feat_Name")]
		public virtual Feat Feat { get; set; }
	}

	public class CraftingAchievementRank : CraftAchievementRank { }

	public class RefiningAchievementRank : CraftAchievementRank { }

	public sealed class MetaAchievementRank : CategoryBonusAchievementRank
	{
        public MetaAchievementRank() => RequiredFlags = new List<AchievementRankFlagRequirement>();

        [InverseProperty("AchievementRank")]
		public List<AchievementRankFlagRequirement> RequiredFlags { get; set; }
	}

	public sealed class FeatAchievementRank : AchievementRank
	{
        public FeatAchievementRank() => RequiredFeats = new List<AchievementRankFeatRequirement>();

        [InverseProperty("AchievementRank")]
		public List<AchievementRankFeatRequirement> RequiredFeats { get; set; }
	}
}
