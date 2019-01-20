using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;

using Goblinary.WikiData.Model;
using Goblinary.WikiData.SqlServer;
using Goblinary.Website.CustomNode;

namespace Goblinary.Website
{
	public partial class RecipeDetails : System.Web.UI.Page
	{
		private string recipeName;
		private int? upgrade;
		private Recipe recipe;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.recipeName = this.Request.QueryString["recipe"] != null ? Uri.UnescapeDataString(this.Request.QueryString["recipe"]) : null;
			if (this.Request.QueryString["upgrade"] != null)
			{
				int upgradeTry;
				if (int.TryParse(this.Request.QueryString["upgrade"], out upgradeTry))
				{
					this.upgrade = (int?) upgradeTry;
				}
			}
            
			if (!this.IsPostBack)
			{
				using (WikiDataContext context = new WikiDataContext())
				{
					var recipes = (
							from r in context.Set<Recipe>()
							where r.Name == recipeName
							select r
						).ToList();

					this.recipe = recipes[0];

					this.Page.MetaDescription += String.Format(" Recipe details for {0}.", this.recipe.Name);
					this.Page.Title = this.recipe.Name + " | Recipe";
					this.recipeTitle.InnerHtml = this.recipe.Name; // set the main header

					if (this.recipe.RecipeType.Name == "CraftingRecipe")
					{
						CraftingRecipe craftingRecipe = (CraftingRecipe)this.recipe;
						var upgradesList = (
								from r in craftingRecipe.OutputItem.RecipeOutputItemUpgrades
								select new
								{
									Value = r.Upgrade.ToString(),
									Text = string.Format("+{0}", r.Upgrade)
								}
							).ToList();
						this.upgradeDropDownList.DataSource = upgradesList;
						this.upgradeDropDownList.DataBind();
					}
					else
					{
						this.DropDownBlock.Visible = false;
					}

					Label dd = new Label();
					// temporary until we can get a real description from the database
					dd.Text = String.Format("Goblinworks has not provided a description for type: <b><a href=\"/RecipeList?type={0}\">{1}</a></b><br>", HttpUtility.UrlEncode(recipe.RecipeType.Name), recipe.RecipeType.DisplayName);
					RecipeBlock.Controls.Add(dd);

					ListView recipeDetailsListView = (ListView)Page.LoadControl("~/Controls/RecipeDetailsControls.ascx").FindControl("RecipeDetailsListView");
					recipeDetailsListView.DataSource = recipes;
					recipeDetailsListView.DataBind();
					this.RecipeBlock.Controls.Add(recipeDetailsListView);

					// generate text tree!
					this.AddNode(this.TreeView1.Nodes, this.recipe, true);
				}
			}
		}

		protected void upgradeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
		{
			Response.Redirect(String.Format("~/RecipeDetails?recipe={0}&upgrade={1}", HttpUtility.UrlEncode(this.recipeName), this.upgradeDropDownList.SelectedValue));
		}

		protected void upgradeDropDownList_PreRender(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(Request.QueryString["upgrade"]))
			{
				((DropDownList)sender).SelectedValue = Request.QueryString["upgrade"];
			}
		}

		private void AddNode(TreeNodeCollection nodes, RefiningRecipeIngredient ingredient)
		{
            CustomTreeNode node = new CustomTreeNode();
			node.ImageUrl = "/Images/s.png";
            node.cssClass = "StockNode IngredientNode";
			node.Text = string.Format("{0} <small>(Qty: {1})</small>", ingredient.Stock_Name, ingredient.Quantity);
			node.NavigateUrl = string.Format("/StockDetails?stock={0}", HttpUtility.UrlEncode(ingredient.Stock_Name));
			node.Expanded = false;
			nodes.Add(node);
		}

		private void AddNode(TreeNodeCollection nodes, CraftingRecipeIngredient ingredient)
		{
            CustomTreeNode node = new CustomTreeNode();
			node.ImageUrl = "/Images/c.png";
            node.cssClass = "ComponentNode IngredientNode";
			node.Text = string.Format("{0} <small>(Qty: {1})</small>", ingredient.Component_Name, ingredient.Quantity);
			node.NavigateUrl = string.Format("/ItemDetails?item={0}", HttpUtility.UrlEncode(ingredient.Component_Name));
			node.Expanded = false;
			nodes.Add(node);
			foreach (Recipe recipe in ingredient.Component.OutputItem.Recipes)
			{
				bool expand = false;
				if (recipe is RefiningRecipe && ((RefiningRecipe)recipe).Upgrade == this.upgrade)
				{
					expand = true;
				}
				this.AddNode(node.ChildNodes, recipe, expand);
			}
		}

		private void AddNode(TreeNodeCollection nodes, Recipe recipe, bool expanded = false)
		{
            CustomTreeNode node = new CustomTreeNode();
			node.ImageUrl = "/Images/r.png";
            node.cssClass = "RecipeNode IngredientNode";
			node.Text = string.Format("{0} <small>(Qty: {1})</small>", recipe.Name, recipe.QtyProduced);
			node.NavigateUrl = string.Format("/RecipeDetails?recipe={0}", HttpUtility.UrlEncode(recipe.Name));
			node.Expanded = expanded;
			nodes.Add(node);
			if (expanded)
			{
				TreeNode parent = node.Parent;
				while (parent != null)
				{
					parent.Expanded = true;
					parent = parent.Parent;
				}
			}
			foreach (RecipeIngredient ingredient in recipe.RecipeIngredients)
			{
				if (ingredient is RefiningRecipeIngredient)
				{
					this.AddNode(node.ChildNodes, (RefiningRecipeIngredient)ingredient);
				}
				else if (ingredient is CraftingRecipeIngredient)
				{
					this.AddNode(node.ChildNodes, (CraftingRecipeIngredient)ingredient);
				}
			}
		}
	}
}