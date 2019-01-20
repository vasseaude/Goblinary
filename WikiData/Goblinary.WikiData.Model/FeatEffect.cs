namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class FeatEffect : IEffectReference
	{
		[Key, Column(Order = 1)]
		public string Feat_Name { get; set; }
		[Key, Column(Order = 2)]
		public string EffectType { get; set; }
		[Key, Column(Order = 3)]
		public int? EffectNo { get; set; }
		[Required]
		public string EffectDescription_Text { get; set; }

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
