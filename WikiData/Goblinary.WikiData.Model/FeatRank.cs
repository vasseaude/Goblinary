namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	using Goblinary.Common;

	public class FeatRank
	{
		public FeatRank()
		{
			this.AbilityBonuses = new EntityList<FeatRankAbilityBonus>();
			this.AbilityRequirements = new EntityList<FeatRankAbilityRequirement>();
			this.AchievementRequirements = new EntityList<FeatRankAchievementRequirement>();
			this.CategoryRequirements = new EntityList<FeatRankCategoryRequirement>();
			this.FeatRequirements = new EntityList<FeatRankFeatRequirement>();
			this.Effects = new EntityList<FeatRankEffect>();
			this.Keywords = new EntityList<FeatRankKeyword>();
		}

		[Key, Column(Order = 1)]
		public string Feat_Name { get; set; }
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
		public virtual Feat Feat { get; set; }

		[InverseProperty("FeatRank")]
		[Presentation(DisplayName = "Ability Bonuses")]
		public virtual EntityList<FeatRankAbilityBonus> AbilityBonuses { get; set; }
		[InverseProperty("FeatRank")]
		[Presentation(DisplayName = "Ability Requirements")]
		public virtual EntityList<FeatRankAbilityRequirement> AbilityRequirements { get; set; }
		[InverseProperty("FeatRank")]
		[Presentation(DisplayName = "Achievement Requirements")]
		public virtual EntityList<FeatRankAchievementRequirement> AchievementRequirements { get; set; }
		[InverseProperty("FeatRank")]
		[Presentation(DisplayName = "Category Requirements")]
		public virtual EntityList<FeatRankCategoryRequirement> CategoryRequirements { get; set; }
		[InverseProperty("FeatRank")]
		[Presentation(DisplayName = "Feat Requirements")]
		public virtual EntityList<FeatRankFeatRequirement> FeatRequirements { get; set; }
		[InverseProperty("FeatRank")]
		[Presentation()]
		public virtual EntityList<FeatRankEffect> Effects { get; set; }
		[InverseProperty("FeatRank")]
		[Presentation()]
		public virtual EntityList<FeatRankKeyword> Keywords { get; set; }
	}
}
