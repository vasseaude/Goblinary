namespace Goblinary.WikiData.Model
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public class Keyword
	{
		public Keyword()
		{
			FeatRankKeywords = new List<FeatRankKeyword>();
			RecipeOutputItemUpgradeKeywords = new List<RecipeOutputItemUpgradeKeyword>();
		}

		[Key, Column(Order = 1)]
		public string KeywordTypeName { get; set; }
		[Key, Column(Order = 2)]
		public string Name { get; set; }
		[Required]
		public string ValueName { get; set; }
		public string Notes { get; set; }

		[ForeignKey("KeywordType_Name")]
		public KeywordType KeywordType { get; set; }

		[InverseProperty("Keyword")]
		public List<FeatRankKeyword> FeatRankKeywords { get; set; }
		[InverseProperty("Keyword")]
		public List<RecipeOutputItemUpgradeKeyword> RecipeOutputItemUpgradeKeywords { get; set; }

		private List<Feat> _sourceFeats;
		[NotMapped]
		public List<Feat> SourceFeats => _sourceFeats ?? (_sourceFeats = (
		                                     from frk in FeatRankKeywords
		                                     where frk.FeatRank.Feat.FeatType.Name == KeywordType.SourceFeatTypeName
		                                           || frk.FeatRank.Feat.GetType().IsSubclassOf(KeywordType.SourceFeatType)
		                                     orderby frk.Feat_Rank, frk.FeatName
		                                     select frk.FeatRank.Feat
		                                 ).Distinct().ToList());

	    private List<Feat> _matchingFeats;
		[NotMapped]
		public List<Feat> MatchingFeats
		{
			get
			{
			    if (_matchingFeats != null || KeywordType.MatchingFeatType == null) return _matchingFeats;
			    _matchingFeats = (
			        from frk in FeatRankKeywords
			        where frk.FeatRank.Feat.FeatType.Name == KeywordType.MatchingFeatTypeName
			              || frk.FeatRank.Feat.GetType().IsSubclassOf(KeywordType.MatchingFeatType)
			        orderby frk.Feat_Rank, frk.FeatName
			        select frk.FeatRank.Feat
			    ).Distinct().ToList();
			    return _matchingFeats;
			}
		}

		private List<Item> _matchingItems;
		[NotMapped]
		public List<Item> MatchingItems
		{
			get
			{
			    if (_matchingItems != null || RecipeOutputItemUpgradeKeywords.Count <= 0) return _matchingItems;
			    _matchingItems = (
			        from ik in RecipeOutputItemUpgradeKeywords
			        orderby ik.RecipeOutputItemUpgrade.RecipeOutputItem.Item.Tier, ik.ItemName
			        select ik.RecipeOutputItemUpgrade.RecipeOutputItem.Item
			    ).Distinct().ToList();
			    return _matchingItems;
			}
		}
	}
}
