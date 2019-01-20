namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public abstract class AchievementRank
	{
		[Key, Column(Order = 1)]
		public string Achievement_Name { get; set; }
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
		public CategoryBonusAchievementRank()
		{
			this.CategoryBonuses = new List<AchievementRankCategoryBonus>();
		}

		[InverseProperty("AchievementRank")]
		public virtual List<AchievementRankCategoryBonus> CategoryBonuses { get; set; }
	}

	public abstract class CounterAchievementRank : CategoryBonusAchievementRank
	{
		[Required]
		public string Counter_Name { get; set; }
		[Required]
		public int? Value { get; set; }
	}

	public class PlayerKillAchievementRank : CounterAchievementRank { }

	public class InteractionAchievementRank : CounterAchievementRank, IKeywordAchievementRank
	{
		public string InteractionKeyword_Name { get; set; }

		[NotMapped]
		string IKeywordAchievementRank.Keyword
		{
			get { return this.InteractionKeyword_Name; }
			set { this.InteractionKeyword_Name = value; }
		}
	}

	public class NPCKillAchievementRank : CounterAchievementRank, IKeywordAchievementRank
	{
		[Required]
		public string Race_Name { get; set; }

		[NotMapped]
		string IKeywordAchievementRank.Keyword
		{
			get { return this.Race_Name; }
			set { this.Race_Name = value; }
		}
	}

	public class WeaponKillAchievementRank : CounterAchievementRank, IKeywordAchievementRank
	{
		[Required]
		public string WeaponProficiency_Name { get; set; }

		[NotMapped]
		string IKeywordAchievementRank.Keyword
		{
			get { return this.WeaponProficiency_Name; }
			set { this.WeaponProficiency_Name = value; }
		}
	}

	public abstract class FlagAchievementRank : CategoryBonusAchievementRank
	{
		[Required]
		public string Flag_Name { get; set; }
	}

	public class SettlementLocationAchievementRank : FlagAchievementRank, IKeywordAchievementRank
	{
		[Required]
		public string Settlement_Name { get; set; }

		[NotMapped]
		string IKeywordAchievementRank.Keyword
		{
			get { return this.Settlement_Name; }
			set { this.Settlement_Name = value; }
		}
	}

	public class SpecialLocationAchievementRank : FlagAchievementRank, IKeywordAchievementRank
	{
		[Required]
		public string Location_Name { get; set; }

		[NotMapped]
		string IKeywordAchievementRank.Keyword
		{
			get { return this.Location_Name; }
			set { this.Location_Name = value; }
		}
	}

	public abstract class CraftAchievementRank : FlagAchievementRank
	{
		[Required]
		public string Feat_Name { get; set; }
		[Required]
		public int? Tier { get; set; }
		[Required]
		public string Rarity_Name { get; set; }
		[Required]
		public int? Upgrade { get; set; }

		[ForeignKey("Feat_Name")]
		public virtual Feat Feat { get; set; }
	}

	public class CraftingAchievementRank : CraftAchievementRank { }

	public class RefiningAchievementRank : CraftAchievementRank { }

	public class MetaAchievementRank : CategoryBonusAchievementRank
	{
		public MetaAchievementRank()
		{
			this.RequiredFlags = new List<AchievementRankFlagRequirement>();
		}

		[InverseProperty("AchievementRank")]
		public virtual List<AchievementRankFlagRequirement> RequiredFlags { get; set; }
	}

	public class FeatAchievementRank : AchievementRank
	{
		public FeatAchievementRank()
		{
			this.RequiredFeats = new List<AchievementRankFeatRequirement>();
		}

		[InverseProperty("AchievementRank")]
		public virtual List<AchievementRankFeatRequirement> RequiredFeats { get; set; }
	}
}
