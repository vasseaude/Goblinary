namespace Goblinary.WikiData.Model
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class AchievementGroup
	{
		[Key]
		public string Name { get; set; }
		[Required]
		public string BaseTypeName { get; set; }
		[Required]
		public string AchievementTypeName { get; set; }

		[ForeignKey("BaseType_Name, AchievementType_Name")]
		public virtual EntityType AchievementType { get; set; }

		[InverseProperty("AchievementGroup")]
		public virtual List<Achievement> Achievements { get; set; }
	}
}
