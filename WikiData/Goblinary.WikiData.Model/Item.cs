namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public abstract class Item : IEntity
	{
	    [Key]
		public string Name { get; set; }
		[Required]
		public string BaseType_Name { get; set; }
		[Required]
		public string ItemType_Name { get; set; }
		[Required]
		public int? Tier { get; set; }
		public decimal? Encumbrance { get; set; }
		public string Description { get; set; }

		[ForeignKey("BaseType_Name, ItemType_Name")]
		public virtual EntityType ItemType { get; set; }

		[InverseProperty("Item")]
		public virtual RecipeOutputItem OutputItem { get; set; }

		private List<Recipe> _recipes;
		[NotMapped]
		public List<Recipe> Recipes => _recipes ?? (_recipes = OutputItem == null ? new List<Recipe>() : OutputItem.Recipes);

	    private List<RecipeOutputItemUpgradeKeyword> keywords;
		[NotMapped]
		public List<RecipeOutputItemUpgradeKeyword> Keywords => keywords ?? (keywords = OutputItem == null
		                                                            ? new List<RecipeOutputItemUpgradeKeyword>()
		                                                            : (
		                                                                from iu in OutputItem.RecipeOutputItemUpgrades
		                                                                from uk in iu.RecipeOutputItemUpgradeKeywords
		                                                                orderby uk.KeywordKindName, uk.Upgrade
		                                                                select uk
		                                                            ).ToList());
	}

	public abstract class Equipment : Item
	{
		[Required]
		public string ItemCategoryName { get; set; }
	}

	public class Armor : Equipment
	{
		[Required]
		public string ArmorTypeName { get; set; }
		[Required]
		public string MainRoleName { get; set; }

		[ForeignKey("MainRole_Name")]
		public Role MainRole { get; set; }
	}

	public class Weapon : Equipment
	{
		[Required]
		public string WeaponTypeName { get; set; }

		[ForeignKey("WeaponType_Name")]
		public virtual WeaponType WeaponType { get; set; }
	}

	public class Gear : Equipment
	{
		[Required]
		public string GearTypeName { get; set; }

		[ForeignKey("GearType_Name")]
		public virtual GearType GearType { get; set; }
	}

	public class Implement : Equipment
	{
		[Required]
		public string ImplementTypeName { get; set; }
	}

	public class AmmoContainer : Equipment
	{
		[Required]
		public string AmmoContainerTypeName { get; set; }
	}

	public class ConsumableItem : Equipment
	{
		[Required]
		public string ConsumableName { get; set; }

		[ForeignKey("Consumable_Name")]
		public virtual Consumable Consumable { get; set; }
	}

	public class Ammo : Equipment
	{
		[Required]
		public string AmmoTypeName { get; set; }
	}

	public class Mule : Equipment
	{
	}

	public class CampKit : Equipment
	{
		[Required]
		public string CampName { get; set; }

		[ForeignKey("Camp_Name")]
		public virtual Camp Camp { get; set; }
	}

	public class HoldingKit : Equipment
	{
		[Required]
		public string HoldingName { get; set; }

		[ForeignKey("Holding_Name")]
		public virtual Holding Holding { get; set; }
	}

	public class OutpostKit : Equipment
	{
		[Required]
		public string OutpostName { get; set; }

		[ForeignKey("Outpost_Name")]
		public virtual Outpost Outpost { get; set; }
	}

	public class WeaponCoating : Equipment
	{
		[Required]
		public string WeaponCoatingTypeName { get; set; }
	}

	public class Ingredient : Item
	{
		[Required]
		public string VarietyName { get; set; }
	}

	public class Component : Ingredient
	{
		public Component()
		{
			CraftingRecipeIngredients = new List<CraftingRecipeIngredient>();
		}

		[InverseProperty("Component")]
		public virtual List<CraftingRecipeIngredient> CraftingRecipeIngredients { get; set; }

		private List<Recipe> _recipes;
		[NotMapped]
		public List<Recipe> RelatedRecipes => _recipes ?? (_recipes = (
		                                          from cri in CraftingRecipeIngredients
		                                          select cri.Recipe).Distinct().ToList());
	}

	public abstract class StockItem : Ingredient
	{
	    protected StockItem()
		{
			StockItemStocks = new List<StockItemStock>();
		}

		[InverseProperty("StockItem")]
		public virtual List<StockItemStock> StockItemStocks { get; set; }

		private List<Recipe> _recipes;
		[NotMapped]
		public List<Recipe> RelatedRecipes => _recipes ?? (_recipes = (
		                                          from sis in StockItemStocks
		                                          from rri in sis.Stock.RefiningRecipeIngredients
		                                          select rri.Recipe
		                                      ).Distinct().ToList());
	}

	public class Resource : StockItem
	{
		[Required]
		public string ResourceTypeName { get; set; }
	}

	public class Salvage : StockItem
	{
	}
}
