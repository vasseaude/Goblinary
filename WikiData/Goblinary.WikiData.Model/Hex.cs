namespace Goblinary.WikiData.Model
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class Hex
	{
		public Hex()
		{
			ResourceRatings = new List<HexBulkRating>();
		}

		[Key, Column(Order = 1)]
		public int? Longitude { get; set; }
		[Key, Column(Order = 2)]
		public int? Latitude { get; set; }
		[Required]
		public string RegionName { get; set; }
		[Required]
		public string TerrainTypeName { get; set; }
		[Required]
		public string HexTypeName { get; set; }

		[InverseProperty("Hex")]
		public virtual List<HexBulkRating> ResourceRatings { get; }
	}
}
