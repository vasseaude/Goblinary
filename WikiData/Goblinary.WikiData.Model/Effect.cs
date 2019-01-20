namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	using Goblinary.Common;

	public class Effect
	{
		public Effect()
		{
			this.EffectDescriptions = new List<EffectDescription>();
			this.EffectConditions = new List<EffectCondition>();
		}

		[Key]
		public string Name { get; set; }
		public string EffectTerm_Term { get; set; }

		[ForeignKey("EffectTerm_Term")]
		public virtual EffectTerm EffectTerm { get; set; }
		[InverseProperty("Effect")]
		public virtual List<EffectDescription> EffectDescriptions { get; set; }
		[InverseProperty("Effect")]
		public virtual List<EffectCondition> EffectConditions { get; set; }
	}
}
