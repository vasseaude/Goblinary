namespace Goblinary.WikiData.Model
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	using Common;

	public class FeatRank
	{
		public FeatRank()
		{
			AbilityBonuses = new EntityList<FeatRankAbilityBonus>();
			AbilityRequirements = new EntityList<FeatRankAbilityRequirement>();
			AchievementRequirements = new EntityList<FeatRankAchievementRequirement>();
			CategoryRequirements = new EntityList<FeatRankCategoryRequirement>();
			FeatRequirements = new EntityList<FeatRankFeatRequirement>();
			Effects = new EntityList<FeatRankEffect>();
			Keywords = new EntityList<FeatRankKeyword>();
		}

		[Key, Column(Order = 1)]
		public string FeatName { get; set; }
		[Key, Column(Order = 2)]
		[Presentation(DisplayOrder = 1)]
		public int? Rank { get; set; }
		[Required]
		[Presentation(DisplayName = "XP Cost")]
		public int? ExpCost { get; set; }
		[Required]
		[Presentation(DisplayName = "Coin Cost")]
		public int? CoinCost { get; set; }

		[ForeignKey("Feat_Name")]
		public Feat Feat { get; set; }

		[InverseProperty("FeatRank")]
		[Presentation(DisplayName = "Ability Bonuses")]
		public EntityList<FeatRankAbilityBonus> AbilityBonuses { get; set; }
		[InverseProperty("FeatRank")]
		[Presentation(DisplayName = "Ability Requirements")]
		public EntityList<FeatRankAbilityRequirement> AbilityRequirements { get; set; }
		[InverseProperty("FeatRank")]
		[Presentation(DisplayName = "Achievement Requirements")]
		public EntityList<FeatRankAchievementRequirement> AchievementRequirements { get; set; }
		[InverseProperty("FeatRank")]
		[Presentation(DisplayName = "Category Requirements")]
		public EntityList<FeatRankCategoryRequirement> CategoryRequirements { get; set; }
		[InverseProperty("FeatRank")]
		[Presentation(DisplayName = "Feat Requirements")]
		public EntityList<FeatRankFeatRequirement> FeatRequirements { get; set; }
		[InverseProperty("FeatRank")]
		[Presentation()]
		public EntityList<FeatRankEffect> Effects { get; set; }
		[InverseProperty("FeatRank")]
		[Presentation()]
		public EntityList<FeatRankKeyword> Keywords { get; set; }
	}
}
