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

	public class RecipeOutputItem
	{
		public RecipeOutputItem()
		{
			this.RecipeOutputItemUpgrades = new List<RecipeOutputItemUpgrade>();
			this.Recipes = new List<Recipe>();
		}

		[Key, ForeignKey("Item")]
		public string Item_Name { get; set; }

		//[ForeignKey("Item_Name")]
		public virtual Item Item { get; set; }

		[InverseProperty("RecipeOutputItem")]
		public virtual List<RecipeOutputItemUpgrade> RecipeOutputItemUpgrades { get; set; }
		[InverseProperty("OutputItem")]
		public virtual List<Recipe> Recipes { get; set; }
	}

	public class RecipeOutputItemUpgrade
	{
		public RecipeOutputItemUpgrade()
		{
			this.RecipeOutputItemUpgradeKeywords = new List<RecipeOutputItemUpgradeKeyword>();
		}

		[Key, Column(Order = 1)]
		public string Item_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Upgrade { get; set; }

		[ForeignKey("Item_Name")]
		public virtual RecipeOutputItem RecipeOutputItem { get; set; }

		[InverseProperty("RecipeOutputItemUpgrade")]
		public virtual List<RecipeOutputItemUpgradeKeyword> RecipeOutputItemUpgradeKeywords { get; set; }

		private Recipe recipe;
		[NotMapped]
		public Recipe Recipe
		{
			get
			{
				if (this.recipe == null && this.RecipeOutputItem.Item is Component)
				{
					this.recipe = (
						from r in this.RecipeOutputItem.Recipes
						where r is RefiningRecipe
							&& ((RefiningRecipe)r).Upgrade == this.Upgrade
						select r).FirstOrDefault();
				}
				return this.recipe;
			}
		}
	}

	public class RecipeOutputItemUpgradeKeyword
	{
		[Key, Column(Order = 1)]
		public string Item_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Upgrade { get; set; }
		[Key, Column(Order = 3)]
		public string KeywordKind_Name { get; set; }
		[Key, Column(Order = 4)]
		public int? KeywordNo { get; set; }
		[Required]
		public string KeywordType_Name { get; set; }
		[Required]
		public string Keyword_Name { get; set; }

		[ForeignKey("Item_Name, Upgrade")]
		public virtual RecipeOutputItemUpgrade RecipeOutputItemUpgrade { get; set; }
		[ForeignKey("KeywordType_Name, Keyword_Name")]
		public virtual Keyword Keyword { get; set; }
	}
}
