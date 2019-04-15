namespace Goblinary.WikiData.Model
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class EffectTerm
	{
		public EffectTerm()
		{
			Effects = new List<Effect>();
		}

		[Key]
		public string Term { get; set; }
		[Required]
		public string EffectTypeName { get; set; }
		[Required]
		public string Description { get; set; }
		public string MathSpecifics { get; set; }
		public string ChannelName { get; set; }

		[InverseProperty("EffectTerm")]
		public List<Effect> Effects { get; set; }
	}
}
