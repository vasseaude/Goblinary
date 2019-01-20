namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;

	using Goblinary.Common;
	
	public class Keyword
	{
		public Keyword()
		{
			this.FeatRankKeywords = new List<FeatRankKeyword>();
			this.RecipeOutputItemUpgradeKeywords = new List<RecipeOutputItemUpgradeKeyword>();
		}

		[Key, Column(Order = 1)]
		public string KeywordType_Name { get; set; }
		[Key, Column(Order = 2)]
		public string Name { get; set; }
		[Required]
		public string Value_Name { get; set; }
		public string Notes { get; set; }

		[ForeignKey("KeywordType_Name")]
		public virtual KeywordType KeywordType { get; set; }

		[InverseProperty("Keyword")]
		public virtual List<FeatRankKeyword> FeatRankKeywords { get; set; }
		[InverseProperty("Keyword")]
		public virtual List<RecipeOutputItemUpgradeKeyword> RecipeOutputItemUpgradeKeywords { get; set; }

		private List<Feat> sourceFeats;
		[NotMapped]
		public List<Feat> SourceFeats
		{
			get
			{
				if (this.sourceFeats == null)
				{
					this.sourceFeats = (
							from frk in this.FeatRankKeywords
							where frk.FeatRank.Feat.FeatType.Name == this.KeywordType.SourceFeatType_Name
								|| frk.FeatRank.Feat.GetType().IsSubclassOf(this.KeywordType.SourceFeatType)
							orderby frk.Feat_Rank, frk.Feat_Name
							select frk.FeatRank.Feat
						).Distinct().ToList();
				}
				return this.sourceFeats;
			}
		}

		private List<Feat> matchingFeats;
		[NotMapped]
		public List<Feat> MatchingFeats
		{
			get
			{
				if (this.matchingFeats == null && this.KeywordType.MatchingFeatType != null)
				{
					this.matchingFeats = (
							from frk in this.FeatRankKeywords
							where frk.FeatRank.Feat.FeatType.Name == this.KeywordType.MatchingFeatType_Name
								|| frk.FeatRank.Feat.GetType().IsSubclassOf(this.KeywordType.MatchingFeatType)
							orderby frk.Feat_Rank, frk.Feat_Name
							select frk.FeatRank.Feat
						).Distinct().ToList();
				}
				return this.matchingFeats;
			}
		}

		private List<Item> matchingItems;
		[NotMapped]
		public List<Item> MatchingItems
		{
			get
			{
				if (this.matchingItems == null && this.RecipeOutputItemUpgradeKeywords.Count > 0)
				{
					this.matchingItems = (
							from ik in this.RecipeOutputItemUpgradeKeywords
							orderby ik.RecipeOutputItemUpgrade.RecipeOutputItem.Item.Tier, ik.Item_Name
							select ik.RecipeOutputItemUpgrade.RecipeOutputItem.Item
						).Distinct().ToList();
				}
				return this.matchingItems;
			}
		}
	}
}
