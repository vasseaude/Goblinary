namespace Goblinary.CharacterData.Model
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

	public class Character
	{
		public Character()
		{
			this.FeatRanks = new List<CharacterFeatRank>();
			this.AchievementRanks = new List<CharacterAchievementRank>();
		}

		[Key]
		public int ID { get; set; }
		[Required]
		public string User_ID { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string ShareStatus { get; set; }
		[Required]
		public string ShareSeed { get; set; }

		[InverseProperty("Character")]
		public virtual List<CharacterFeatRank> FeatRanks { get; private set; }
		[InverseProperty("Character")]
		public virtual List<CharacterAchievementRank> AchievementRanks { get; private set; }
	}
}
