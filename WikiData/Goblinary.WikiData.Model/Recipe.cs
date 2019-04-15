namespace Goblinary.WikiData.Model
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public abstract class Recipe : IEntity
	{
	    protected Recipe()
		{
			RecipeIngredients = new List<RecipeIngredient>();
		}

		[Key]
		public string Name { get; set; }
		[Required]
		public string BaseTypeName { get; set; }
		[Required]
		public string RecipeTypeName { get; set; }
		[Required]
		public string FeatName { get; set; }
		[Required]
		public int? Feat_Rank { get; set; }
		[Required]
		public int? Tier { get; set; }
		[Required]
		public string OutputItemName { get; set; }
		[Required]
		public int? QtyProduced { get; set; }
		[Required]
		public int? BaseCraftingSeconds { get; set; }
		[Required]
		public int? Quality { get; set; }
		[Required]
		public string AchievementTypeName { get; set; }

		[ForeignKey("BaseType_Name, RecipeType_Name")]
		public virtual EntityType RecipeType { get; set; }
		[ForeignKey("Feat_Name, Feat_Rank")]
		public virtual FeatRank FeatRank { get; set; }
		[ForeignKey("OutputItem_Name")]
		public virtual RecipeOutputItem OutputItem { get; set; }

		[InverseProperty("Recipe")]
		public virtual List<RecipeIngredient> RecipeIngredients { get; set; }
	}

	public class RefiningRecipe : Recipe
	{
		[Required]
		public int? Upgrade { get; set; }
	}

	public class CraftingRecipe : Recipe
	{
	}

	public abstract class RecipeIngredient
	{
		[Key, Column(Order = 1)]
		public string RecipeName { get; set; }
		[Key, Column(Order = 2)]
		public int? IngredientNo { get; set; }
		[Required]
		public int Quantity { get; set; }

		[ForeignKey("Recipe_Name")]
		public virtual Recipe Recipe { get; set; }
	}

	public class RefiningRecipeIngredient : RecipeIngredient
	{
		[Required]
		public string StockName { get; set; }

		[ForeignKey("Stock_Name")]
		public virtual Stock Stock { get; set; }
	}

	public class CraftingRecipeIngredient : RecipeIngredient
	{
		[Required]
		public string ComponentName { get; set; }

		[ForeignKey("Component_Name")]
		public virtual Component Component { get; set; }
	}
}
