namespace Goblinary.WikiData.Model
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class Trainer
	{
		public Trainer()
		{
		    //HoldingUpgradeTrainerLevels = holdingUpgradeTrainerLevels;
		    FeatRankTrainerLevels = new List<FeatRankTrainerLevel>();
		}

		[Key]
		public string Name { get; set; }

		[InverseProperty("Trainer")]
		public virtual List<FeatRankTrainerLevel> FeatRankTrainerLevels { get; }
		[InverseProperty("Trainer")]
		public virtual List<HoldingUpgradeTrainerLevel> HoldingUpgradeTrainerLevels { get; }
	}
}
