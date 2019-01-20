namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	using Goblinary.Common;

	public class AchievementGroup
	{
		[Key]
		public string Name { get; set; }
		[Required]
		public string BaseType_Name { get; set; }
		[Required]
		public string AchievementType_Name { get; set; }

		[ForeignKey("BaseType_Name, AchievementType_Name")]
		public virtual EntityType AchievementType { get; set; }

		[InverseProperty("AchievementGroup")]
		public virtual List<Achievement> Achievements { get; set; }
	}
}
