namespace Goblinary.WikiData.Model
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class FeatRankEffect : IEffectReference
	{
		[Key, Column(Order = 1)]
		public string FeatName { get; set; }
		[Key, Column(Order = 2)]
		public int? Feat_Rank { get; set; }
		[Key, Column(Order = 3)]
		public int? EffectNo { get; set; }
		[Required]
		public string EffectDescriptionText { get; set; }

		[ForeignKey("Feat_Name, Feat_Rank")]
		public virtual FeatRank FeatRank { get; set; }
		[ForeignKey("EffectDescription_Text")]
		public virtual EffectDescription EffectDescription { get; set; }

		public static Func<FeatRankEffect, string> ToStringMethod { get; set; }
		public override string ToString() => ToStringMethod != null ? ToStringMethod(this) : base.ToString();
	}
}
