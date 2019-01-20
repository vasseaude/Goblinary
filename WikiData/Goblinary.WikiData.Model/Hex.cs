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

	public class Hex
	{
		public Hex()
		{
			this.ResourceRatings = new List<HexBulkRating>();
		}

		[Key, Column(Order = 1)]
		public int? Longitude { get; set; }
		[Key, Column(Order = 2)]
		public int? Latitude { get; set; }
		[Required]
		public string Region_Name { get; set; }
		[Required]
		public string TerrainType_Name { get; set; }
		[Required]
		public string HexType_Name { get; set; }

		[InverseProperty("Hex")]
		public virtual List<HexBulkRating> ResourceRatings { get; private set; }
	}
}
