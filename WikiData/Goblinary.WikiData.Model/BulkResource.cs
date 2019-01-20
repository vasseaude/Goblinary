namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;

	public class BulkResource
	{
		public BulkResource()
		{
			this.HoldingUpgradeBonuses = new List<HoldingUpgradeBulkResourceBonus>();
			this.HoldingUpgradeRequirements = new List<HoldingUpgradeBulkResourceRequirement>();
			this.OutpostBulkResources = new List<OutpostBulkResource>();
		}

		[Key]
		public string Name { get; set; }

		[InverseProperty("BulkResource")]
		public virtual List<HoldingUpgradeBulkResourceBonus> HoldingUpgradeBonuses { get; private set; }
		[InverseProperty("BulkResource")]
		public virtual List<HoldingUpgradeBulkResourceRequirement> HoldingUpgradeRequirements { get; private set; }
		[InverseProperty("BulkResource")]
		public virtual List<OutpostBulkResource> OutpostBulkResources { get; private set; }
	}

	public class BulkRating
	{
		public BulkRating()
		{
			this.HexBulkRatings = new List<HexBulkRating>();
			this.OutpostBulkRatings = new List<OutpostBulkResource>();
		}

		[Key]
		public string Name { get; set; }

		[InverseProperty("BulkRating")]
		public virtual List<HexBulkRating> HexBulkRatings { get; private set; }
		[InverseProperty("BulkRating")]
		public virtual List<OutpostBulkResource> OutpostBulkRatings { get; private set; }
	}
}
