namespace Goblinary.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public abstract class AchievementLevelFact
	{
		[Key, Column(Order = 1)]
		public string Achievement_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Achievement_Level { get; set; }
		[Key, Column(Order = 3)]
		public int? FactNo { get; set; }
		[Key, Column(Order = 4)]
		public int? OptionNo { get; set; }

		[ForeignKey("Achievement_Name")]
		public virtual Achievement Achievement { get; set; }
		[ForeignKey("Achievement_Level")]
		public virtual AchievementLevel AchievementLevel { get; set; }
	}

	public class AchievementLevelCategoryBonus
	{
		[Required]
		public string BonusCategory_Name { get; set; }
		[Required]
		public int? BonusCategoryPoints { get; set; }

		[ForeignKey("BonusCategory_Name")]
		public virtual AchievementCategory BonusCategory { get; set; }
	}

	public class AchievementLevelAdvancementRequirement
	{
		[Required]
		public string RequiredAdvancement_Name { get; set; }
		[Required]
		public int? RequiredAdvancement_Rank { get; set; }

		[ForeignKey("Advancement_Name")]
		public virtual Advancement Advancement { get; set; }
		[ForeignKey("Advancement_Rank")]
		public virtual AdvancementRank AdvancementRank { get; set; }
	}
}
