namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class EffectDescription
	{
		public EffectDescription()
		{
			FeatEffects = new List<FeatEffect>();
			FeatRankEffects = new List<FeatRankEffect>();
		}

		[Key]
		public string Text { get; set; }
		[Required]
		public string FormattedDescription { get; set; }
		[Required]
		public string EffectName { get; set; }
		public string Magnitude { get; set; }
		public string Duration { get; set; }
		public string Chance { get; set; }
		public string Distance { get; set; }
		public string Target { get; set; }
		public string Discriminator { get; set; }
		public string ConditionName { get; set; }
		public string ConditionTarget { get; set; }

		[ForeignKey("Effect_Name")]
		public Effect Effect { get; set; }
		[ForeignKey("Condition_Name")]
		public Condition Condition { get; set; }

		[InverseProperty("EffectDescription")]
		public List<FeatEffect> FeatEffects { get; set; }
		[InverseProperty("EffectDescription")]
		public List<FeatRankEffect> FeatRankEffects { get; set; }

		public static Func<EffectDescription, string> ToStringMethod { get; set; }
		public override string ToString()
		{
			return ToStringMethod != null ? ToStringMethod(this) : base.ToString();
		}
	}
}
