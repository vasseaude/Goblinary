namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class FeatRankCategoryRequirement : IFeatRankRequirement
	{
		[Key, Column(Order = 1)]
		public string Feat_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Feat_Rank { get; set; }
		[Key, Column(Order = 3)]
		public int? RequirementNo { get; set; }
		[Key, Column(Order = 4)]
		public int? OptionNo { get; set; }
		[Required]
		public string Category_Name { get; set; }
		[Required]
		public int? Value { get; set; }

		[ForeignKey("Feat_Name, Feat_Rank")]
		public virtual FeatRank FeatRank { get; set; }

		[NotMapped]
		string IFeatRankFact.FactName
		{
			get { return this.Category_Name; }
			set { this.Category_Name = value; }
		}

		public static Func<FeatRankCategoryRequirement, string> ToStringMethod { get; set; }
		public override string ToString()
		{
			return ToStringMethod != null ? ToStringMethod(this) : base.ToString();
		}
	}
}
