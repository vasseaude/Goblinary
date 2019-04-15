namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class AdvancementFeat
	{
		public AdvancementFeat()
		{
			Feats = new List<Feat>();
			FeatRankTrainerLevels = new List<FeatRankTrainerLevel>();
		}

		[Key]
		public string Name { get; set; }

		[InverseProperty("AdvancementFeat")]
		public virtual List<Feat> Feats { get; private set; }
		[InverseProperty("AdvancementFeat")]
		public virtual List<FeatRankTrainerLevel> FeatRankTrainerLevels { get; private set; }

		public static Func<AdvancementFeat, string> ToStringMethod { get; set; }
		public override string ToString()
		{
			return ToStringMethod != null ? ToStringMethod(this) : base.ToString();
		}
	}
}
