namespace Goblinary.WikiData.Model
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public class RecipeOutputItem
	{
		public RecipeOutputItem()
		{
			RecipeOutputItemUpgrades = new List<RecipeOutputItemUpgrade>();
			Recipes = new List<Recipe>();
		}

		[Key, ForeignKey("Item")]
		public string ItemName { get; set; }

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
			RecipeOutputItemUpgradeKeywords = new List<RecipeOutputItemUpgradeKeyword>();
		}

		[Key, Column(Order = 1)]
		public string ItemName { get; set; }
		[Key, Column(Order = 2)]
		public int? Upgrade { get; set; }

		[ForeignKey("Item_Name")]
		public virtual RecipeOutputItem RecipeOutputItem { get; set; }

		[InverseProperty("RecipeOutputItemUpgrade")]
		public virtual List<RecipeOutputItemUpgradeKeyword> RecipeOutputItemUpgradeKeywords { get; set; }

		private Recipe _recipe;
		[NotMapped]
		public Recipe Recipe
		{
			get
			{
			    if (_recipe != null || !(RecipeOutputItem.Item is Component)) return _recipe;
			    _recipe = (
			        from r in RecipeOutputItem.Recipes
			        where r is RefiningRecipe
			              && ((RefiningRecipe)r).Upgrade == Upgrade
			        select r).FirstOrDefault();
			    return _recipe;
			}
		}
	}

	public class RecipeOutputItemUpgradeKeyword
	{
		[Key, Column(Order = 1)]
		public string ItemName { get; set; }
		[Key, Column(Order = 2)]
		public int? Upgrade { get; set; }
		[Key, Column(Order = 3)]
		public string KeywordKindName { get; set; }
		[Key, Column(Order = 4)]
		public int? KeywordNo { get; set; }
		[Required]
		public string KeywordTypeName { get; set; }
		[Required]
		public string KeywordName { get; set; }

		[ForeignKey("Item_Name, Upgrade")]
		public virtual RecipeOutputItemUpgrade RecipeOutputItemUpgrade { get; set; }
		[ForeignKey("KeywordType_Name, Keyword_Name")]
		public virtual Keyword Keyword { get; set; }
	}
}
