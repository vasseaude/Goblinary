﻿namespace Goblinary.WikiData.Model
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class HexBulkRating
	{
		[Key, Column(Order = 1)]
		public int? Hex_Longitude { get; set; }
		[Key, Column(Order = 2)]
		public int? Hex_Latitude { get; set; }
		[Key, Column(Order = 3)]
		public string BulkRating_Name { get; set; }
		[Required]
		public int? Rating { get; set; }

		[ForeignKey("Hex_Longitude, Hex_Latitude")]
		public virtual Hex Hex { get; set; }
		[ForeignKey("BulkRating_Name")]
		public virtual BulkRating BulkRating { get; set; }
	}
}
