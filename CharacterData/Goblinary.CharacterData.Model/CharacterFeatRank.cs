﻿namespace Goblinary.CharacterData.Model
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

	public class CharacterFeatRank
	{
		[Key, Column(Order = 1)]
		public int Character_ID { get; set; }
		[Key, Column(Order = 2)]
		public string Feat_Name { get; set; }
		public int? TrainedRank { get; set; }
		public int? WishListRank { get; set; }

		[ForeignKey("Character_ID")]
		public virtual Character Character { get; set; }
	}
}
