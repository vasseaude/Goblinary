namespace Goblinary.WikiData.Model
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class Effect
	{
		public Effect()
		{
			EffectDescriptions = new List<EffectDescription>();
			EffectConditions = new List<EffectCondition>();
		}

		[Key]
		public string Name { get; set; }
		public string EffectTermTerm { get; set; }

		[ForeignKey("EffectTerm_Term")]
		public EffectTerm EffectTerm { get; set; }
		[InverseProperty("Effect")]
		public List<EffectDescription> EffectDescriptions { get; set; }
		[InverseProperty("Effect")]
		public List<EffectCondition> EffectConditions { get; set; }
	}
}
