namespace Goblinary.WikiData.Model
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class FeatEffect : IEffectReference
	{
		[Key, Column(Order = 1)]
		public string FeatName { get; set; }
		[Key, Column(Order = 2)]
		public string EffectType { get; set; }
		[Key, Column(Order = 3)]
		public int? EffectNo { get; set; }
		[Required]
		public string EffectDescriptionText { get; set; }

		[ForeignKey("Feat_Name")]
		public virtual Feat Feat { get; set; }
		[ForeignKey("EffectDescription_Text")]
		public virtual EffectDescription EffectDescription { get; set; }

		public static Func<FeatEffect, string> ToStringMethod { get; set; }
		public override string ToString()
		{
			return ToStringMethod != null ? ToStringMethod(this) : base.ToString();
		}
	}
}
