namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class AchievementRankCategoryBonusComparer : IEqualityComparer<AchievementRankCategoryBonus>
	{
		public bool Equals(AchievementRankCategoryBonus x, AchievementRankCategoryBonus y)
		{
			return x.Category_Name == y.Category_Name;
		}

		public int GetHashCode(AchievementRankCategoryBonus obj)
		{
			return obj.Category_Name.GetHashCode();
		}
	}

	[Table("AchievementRankCategoryBonuses")]
	public class AchievementRankCategoryBonus : IAchievementRankCategoryBonus
	{
		[Key, Column(Order = 1)]
		public string Achievement_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Achievement_Rank { get; set; }
		[Key, Column(Order = 3)]
		public int? BonusNo { get; set; }
		[Required]
		public string Category_Name { get; set; }
		[Required]
		public int? Bonus { get; set; }

		[ForeignKey("Achievement_Name, Achievement_Rank")]
		public virtual CategoryBonusAchievementRank AchievementRank { get; set; }
	}
}
