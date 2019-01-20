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

	public class EffectTerm
	{
		public EffectTerm()
		{
			this.Effects = new List<Effect>();
		}

		[Key]
		public string Term { get; set; }
		[Required]
		public string EffectType_Name { get; set; }
		[Required]
		public string Description { get; set; }
		public string MathSpecifics { get; set; }
		public string Channel_Name { get; set; }

		[InverseProperty("EffectTerm")]
		public virtual List<Effect> Effects { get; set; }
	}
}
