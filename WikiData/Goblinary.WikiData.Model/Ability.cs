namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;

	using Goblinary.Common;

	public class Ability
	{
		[Key]
		public string Name { get; set; }

		[InverseProperty("Ability")]
		public List<FeatRankAbilityBonus> FeatRankAbilityBonuses { get; set; }

		[InverseProperty("Ability")]
		public List<FeatRankAbilityRequirement> FeatRankAbilityRequirements { get; set; }

		public static Func<Ability, string> ToStringMethod { get; set; }
		public override string ToString()
		{
			return ToStringMethod != null ? ToStringMethod(this) : base.ToString();
		}
	}
}
