namespace Goblinary.WikiData.Model
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class Condition
	{
		[Key]
		public string Name { get; set; }
	}

	public class EffectCondition : Condition
	{
		[Required]
		public string EffectName { get; set; }

		[ForeignKey("Effect_Name")]
		public virtual Effect Effect { get; set; }
	}
}
