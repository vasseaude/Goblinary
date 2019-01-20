namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class Trainer
	{
		public Trainer()
		{
			this.FeatRankTrainerLevels = new List<FeatRankTrainerLevel>();
		}

		[Key]
		public string Name { get; set; }

		[InverseProperty("Trainer")]
		public virtual List<FeatRankTrainerLevel> FeatRankTrainerLevels { get; private set; }
		[InverseProperty("Trainer")]
		public virtual List<HoldingUpgradeTrainerLevel> HoldingUpgradeTrainerLevels { get; private set; }
	}
}
