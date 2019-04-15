namespace Goblinary.WikiData.Model
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;


	public class BulkResource
	{
		public BulkResource()
		{
			HoldingUpgradeBonuses = new List<HoldingUpgradeBulkResourceBonus>();
			HoldingUpgradeRequirements = new List<HoldingUpgradeBulkResourceRequirement>();
			OutpostBulkResources = new List<OutpostBulkResource>();
		}

		[Key]
		public string Name { get; set; }

		[InverseProperty("BulkResource")]
		public virtual List<HoldingUpgradeBulkResourceBonus> HoldingUpgradeBonuses { get; }
		[InverseProperty("BulkResource")]
		public virtual List<HoldingUpgradeBulkResourceRequirement> HoldingUpgradeRequirements { get; }
		[InverseProperty("BulkResource")]
		public virtual List<OutpostBulkResource> OutpostBulkResources { get; }
	}

	public class BulkRating
	{
		public BulkRating()
		{
			HexBulkRatings = new List<HexBulkRating>();
			OutpostBulkRatings = new List<OutpostBulkResource>();
		}

		[Key]
		public string Name { get; set; }

		[InverseProperty("BulkRating")]
		public virtual List<HexBulkRating> HexBulkRatings { get; }
		[InverseProperty("BulkRating")]
		public virtual List<OutpostBulkResource> OutpostBulkRatings { get; }
	}
}
