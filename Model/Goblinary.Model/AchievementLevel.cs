namespace Goblinary.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class AchievementLevel
	{
		public AchievementLevel()
		{
			this.Facts = new List<AchievementLevelFact>();
		}

		[Key, Column(Order = 1)]
		public string Achievement_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Level { get; set; }
		[Required]
		public string DisplayName { get; set; }
		public string Description { get; set; }

		[ForeignKey("Achievement_Name")]
		public Achievement Achievement { get; set; }

		[InverseProperty("AchievementLevel")]
		public virtual List<AchievementLevelFact> Facts { get; set; }
	}

	public class CounterAchievementLevel : AchievementLevel
	{
		[Required]
		public int? CounterValue { get; set; }
	}

	public class CraftAchievementLevel : AchievementLevel
	{
		[Required]
		public int? UpgradePlus { get; set; }
	}
}
