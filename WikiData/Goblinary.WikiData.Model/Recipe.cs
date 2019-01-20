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

	public abstract class Recipe : IEntity
	{
		public Recipe()
		{
			this.RecipeIngredients = new List<RecipeIngredient>();
		}

		[Key]
		public string Name { get; set; }
		[Required]
		public string BaseType_Name { get; set; }
		[Required]
		public string RecipeType_Name { get; set; }
		[Required]
		public string Feat_Name { get; set; }
		[Required]
		public int? Feat_Rank { get; set; }
		[Required]
		public int? Tier { get; set; }
		[Required]
		public string OutputItem_Name { get; set; }
		[Required]
		public int? QtyProduced { get; set; }
		[Required]
		public int? BaseCraftingSeconds { get; set; }
		[Required]
		public int? Quality { get; set; }
		[Required]
		public string AchievementType_Name { get; set; }

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
		public string Recipe_Name { get; set; }
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
		public string Stock_Name { get; set; }

		[ForeignKey("Stock_Name")]
		public virtual Stock Stock { get; set; }
	}

	public class CraftingRecipeIngredient : RecipeIngredient
	{
		[Required]
		public string Component_Name { get; set; }

		[ForeignKey("Component_Name")]
		public virtual Component Component { get; set; }
	}
}
