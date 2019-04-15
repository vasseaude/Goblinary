namespace Goblinary.WikiData.Model
{

	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;


	public class AchievementRankCategoryBonusComparer : IEqualityComparer<AchievementRankCategoryBonus>
	{
		public bool Equals(AchievementRankCategoryBonus x, AchievementRankCategoryBonus y)
		{
			return y != null && (x != null && x.CategoryName == y.CategoryName);
		}

		public int GetHashCode(AchievementRankCategoryBonus obj)
		{
			return obj.CategoryName.GetHashCode();
		}
	}

	[Table("AchievementRankCategoryBonuses")]
	public class AchievementRankCategoryBonus : IAchievementRankCategoryBonus
	{
		[Key, Column(Order = 1)]
		public string AchievementName { get; set; }
		[Key, Column(Order = 2)]
		public int? Achievement_Rank { get; set; }
		[Key, Column(Order = 3)]
		public int? BonusNo { get; set; }
		[Required]
		public string CategoryName { get; set; }
		[Required]
		public int? Bonus { get; set; }

		[ForeignKey("Achievement_Name, Achievement_Rank")]
		public virtual CategoryBonusAchievementRank AchievementRank { get; set; }
	}
}
