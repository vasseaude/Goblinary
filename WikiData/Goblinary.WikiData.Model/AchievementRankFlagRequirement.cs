namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class AchievementRankFlagRequirement : IAchievementRankFlagRequirement
	{
		[Key, Column(Order = 1)]
		public string Achievement_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Achievement_Rank { get; set; }
		[Key, Column(Order = 3)]
		public int? RequirementNo { get; set; }
		[Key, Column(Order = 4)]
		public int? OptionNo { get; set; }
		[Required]
		public string Flag_Name { get; set; }

		[ForeignKey("Achievement_Name, Achievement_Rank")]
		public virtual MetaAchievementRank AchievementRank { get; set; }
	}
}
