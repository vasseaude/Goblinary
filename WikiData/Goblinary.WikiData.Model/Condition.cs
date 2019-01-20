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

	public class Condition
	{
		[Key]
		public string Name { get; set; }
	}

	public class EffectCondition : Condition
	{
		[Required]
		public string Effect_Name { get; set; }

		[ForeignKey("Effect_Name")]
		public virtual Effect Effect { get; set; }
	}
}
