namespace Goblinary.WikiData.Model
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class FeatRankAbilityRequirement : IFeatRankRequirement
	{
		[Key, Column(Order = 1)]
		public string FeatName { get; set; }
		[Key, Column(Order = 2)]
		public int? Feat_Rank { get; set; }
		[Key, Column(Order = 3)]
		public int? RequirementNo { get; set; }
		[Key, Column(Order = 4)]
		public int? OptionNo { get; set; }
		[Required]
		public string AbilityName { get; set; }
		[Required]
		public int? Value { get; set; }

		[ForeignKey("Feat_Name, Feat_Rank")]
		public FeatRank FeatRank { get; set; }

		[ForeignKey("Ability_Name")]
		public Ability Ability { get; set; }

		[NotMapped]
		string IFeatRankFact.FactName
		{
			get => AbilityName;
		    set => AbilityName = value;
		}

		public static Func<FeatRankAbilityRequirement, string> ToStringMethod { get; set; }
		public override string ToString()
		{
			return ToStringMethod != null ? ToStringMethod(this) : base.ToString();
		}
	}
}
