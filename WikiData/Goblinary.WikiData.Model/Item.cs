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

	public abstract class Item : IEntity
	{
		public Item()
		{
		}

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

		private List<Recipe> recipes;
		[NotMapped]
		public List<Recipe> Recipes
		{
			get
			{
				if (this.recipes == null)
				{
					this.recipes = this.OutputItem == null ? new List<Recipe>() : this.OutputItem.Recipes;
				}
				return this.recipes;
			}
		}

		private List<RecipeOutputItemUpgradeKeyword> keywords;
		[NotMapped]
		public List<RecipeOutputItemUpgradeKeyword> Keywords
		{
			get
			{
				if (this.keywords == null)
				{
					this.keywords = this.OutputItem == null ? new List<RecipeOutputItemUpgradeKeyword>()
						: (
							from iu in this.OutputItem.RecipeOutputItemUpgrades
							from uk in iu.RecipeOutputItemUpgradeKeywords
							orderby uk.KeywordKind_Name, uk.Upgrade
							select uk
						).ToList();
				}
				return this.keywords;
			}
		}
	}

	public abstract class Equipment : Item
	{
		[Required]
		public string ItemCategory_Name { get; set; }
	}

	public class Armor : Equipment
	{
		[Required]
		public string ArmorType_Name { get; set; }
		[Required]
		public string MainRole_Name { get; set; }

		[ForeignKey("MainRole_Name")]
		public Role MainRole { get; set; }
	}

	public class Weapon : Equipment
	{
		[Required]
		public string WeaponType_Name { get; set; }

		[ForeignKey("WeaponType_Name")]
		public virtual WeaponType WeaponType { get; set; }
	}

	public class Gear : Equipment
	{
		[Required]
		public string GearType_Name { get; set; }

		[ForeignKey("GearType_Name")]
		public virtual GearType GearType { get; set; }
	}

	public class Implement : Equipment
	{
		[Required]
		public string ImplementType_Name { get; set; }
	}

	public class AmmoContainer : Equipment
	{
		[Required]
		public string AmmoContainerType_Name { get; set; }
	}

	public class ConsumableItem : Equipment
	{
		[Required]
		public string Consumable_Name { get; set; }

		[ForeignKey("Consumable_Name")]
		public virtual Consumable Consumable { get; set; }
	}

	public class Ammo : Equipment
	{
		[Required]
		public string AmmoType_Name { get; set; }
	}

	public class Mule : Equipment
	{
	}

	public class CampKit : Equipment
	{
		[Required]
		public string Camp_Name { get; set; }

		[ForeignKey("Camp_Name")]
		public virtual Camp Camp { get; set; }
	}

	public class HoldingKit : Equipment
	{
		[Required]
		public string Holding_Name { get; set; }

		[ForeignKey("Holding_Name")]
		public virtual Holding Holding { get; set; }
	}

	public class OutpostKit : Equipment
	{
		[Required]
		public string Outpost_Name { get; set; }

		[ForeignKey("Outpost_Name")]
		public virtual Outpost Outpost { get; set; }
	}

	public class WeaponCoating : Equipment
	{
		[Required]
		public string WeaponCoatingType_Name { get; set; }
	}

	public class Ingredient : Item
	{
		[Required]
		public string Variety_Name { get; set; }
	}

	public class Component : Ingredient
	{
		public Component()
		{
			this.CraftingRecipeIngredients = new List<CraftingRecipeIngredient>();
		}

		[InverseProperty("Component")]
		public virtual List<CraftingRecipeIngredient> CraftingRecipeIngredients { get; set; }

		private List<Recipe> recipes;
		[NotMapped]
		public List<Recipe> RelatedRecipes
		{
			get
			{
				if (this.recipes == null)
				{
					this.recipes = (
						from cri in this.CraftingRecipeIngredients
						select cri.Recipe).Distinct().ToList();
				}
				return this.recipes;
			}
		}
	}

	public abstract class StockItem : Ingredient
	{
		public StockItem()
		{
			this.StockItemStocks = new List<StockItemStock>();
		}

		[InverseProperty("StockItem")]
		public virtual List<StockItemStock> StockItemStocks { get; set; }

		private List<Recipe> recipes;
		[NotMapped]
		public List<Recipe> RelatedRecipes
		{
			get
			{
				if (this.recipes == null)
				{
					this.recipes = (
							from sis in this.StockItemStocks
							from rri in sis.Stock.RefiningRecipeIngredients
							select rri.Recipe
						).Distinct().ToList();
				}
				return this.recipes;
			}
		}
	}

	public class Resource : StockItem
	{
		[Required]
		public string ResourceType_Name { get; set; }
	}

	public class Salvage : StockItem
	{
	}
}
