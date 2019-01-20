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

namespace Goblinary.Website
{
    public partial class ItemDetails : System.Web.UI.Page
    {
		private class ItemUpgradeInfo
		{
			public string ItemName { get; set; }
			public int? Upgrade { get; set; }
			public string RecipeName { get; set; }
			public RecipeOutputItemUpgrade ItemUpgrade { get; set; }
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            string QS_Item_Name = HttpUtility.UrlDecode(Request.QueryString["item"]);
			using (WikiDataContext context = new WikiDataContext())
			{
				var items = (
					from i in context.Set<Item>()
					where i.Name == QS_Item_Name
					select i).ToList();
				var item = items[0];
				Page.MetaDescription += String.Format(" Item details for {0}.", item.Name);
				Page.Title = item.Name + " | Item";
				itemTitle.InnerHtml = item.Name; // set the main header

				Label dd = new Label();
				// temporary until we can get a real description from the database
				dd.Text = String.Format("Goblinworks has not provided a description for type: <b><a href=\"/ItemList?type={0}\">{1}</a></b><br>", HttpUtility.UrlEncode(item.ItemType_Name), item.ItemType.DisplayName);
				ItemBlock.Controls.Add(dd);

				Label ItemDescriptionLabel = new Label();
				ItemDescriptionLabel.CssClass = "descriptionLabel";

				if (!string.IsNullOrEmpty(item.Description))
				{
					ItemDescriptionLabel.Text = item.Description;
					ItemBlock.Controls.Add(ItemDescriptionLabel);
				}
				else
				{
					ItemDescriptionLabel.Text = "Goblinworks has not provided a description for this item.";
					ItemBlock.Controls.Add(ItemDescriptionLabel);
				}
				ListView itemListView = (ListView)Page.LoadControl("~/Controls/Item/" + item.ItemType_Name + ".ascx").FindControl("ItemListView");
				itemListView.DataSource = new List<Item>() { item };
				itemListView.ItemDataBound += new EventHandler<ListViewItemEventArgs>(itemListView_ItemDataBound);
				itemListView.DataBind();
				ItemBlock.Controls.Add(itemListView);

				if (item.OutputItem != null)
				{
					GridView gridControl = (GridView)Page.LoadControl("~/Controls/Item/" + item.ItemType_Name + ".ascx").FindControl("ItemBonusesGridView");
					gridControl.DataSource = (
						from u in item.OutputItem.RecipeOutputItemUpgrades
						select new ItemUpgradeInfo
						{
							ItemName = u.Item_Name,
							Upgrade = u.Upgrade,
                            RecipeName = u.Recipe != null ? String.Format("<a href='/RecipeDetails?recipe={0}'>{1}</a>", HttpUtility.UrlEncode(u.Recipe.Name), u.Recipe.Name) : "",
							ItemUpgrade = u
						}).ToList();
					gridControl.RowDataBound += new GridViewRowEventHandler(gridControl_RowDataBound);
					gridControl.DataBind();
					gridControl.HeaderRow.TableSection = TableRowSection.TableHeader;
					gridControl.Attributes.Add("tableName", "a");
					UpgradesBlock.Controls.Add(gridControl);
				}
			}
            // insert search notes
            Table notesTable = (Table)Page.LoadControl("~/Controls/TablesorterNotes.ascx").FindControl("TablesorterNotesTable");
            notes.Controls.Add(notesTable);
        }

        protected void itemListView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListView itemRecipesListView = (ListView)e.Item.FindControl("ItemRecipesListView");
                if (itemRecipesListView != null)
                {
                    var currentItem = (Item)e.Item.DataItem;
                    itemRecipesListView.DataSource = currentItem.Recipes;
                    itemRecipesListView.DataBind();
                }
                ListView stockItemRelatedRecipesListView = (ListView)e.Item.FindControl("StockItemRelatedRecipesListView");
                if (stockItemRelatedRecipesListView != null)
                {
                    var currentStockItem = (StockItem)e.Item.DataItem;
                    stockItemRelatedRecipesListView.DataSource = currentStockItem.RelatedRecipes;
                    stockItemRelatedRecipesListView.DataBind();
                }
                ListView componentRelatedRecipesListView = (ListView)e.Item.FindControl("ComponentRelatedRecipesListView");
                if (componentRelatedRecipesListView != null)
                {
                    var currentComponent = (Component)e.Item.DataItem;
                    componentRelatedRecipesListView.DataSource = currentComponent.RelatedRecipes;
                    componentRelatedRecipesListView.DataBind();
                }
                ListView stockItemStocksListView = (ListView)e.Item.FindControl("StockItemStocksListView");
                if (stockItemStocksListView != null)
                {
                    var currentStockItem = (StockItem)e.Item.DataItem;
                    stockItemStocksListView.DataSource = currentStockItem.StockItemStocks;
                    stockItemStocksListView.DataBind();
                }
            }
        }

        protected void gridControl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ListView itemKeywordsListView = (ListView)e.Row.FindControl("ItemKeywordsListView");
                if (itemKeywordsListView != null)
                {
                    var currentItemUpgrade = (ItemUpgradeInfo)e.Row.DataItem;
                    itemKeywordsListView.DataSource = (
                            from k in currentItemUpgrade.ItemUpgrade.RecipeOutputItemUpgradeKeywords
                            select new
                            {
                                Keyword_Name = k.Keyword_Name,
                                KeywordType_Name = k.KeywordType_Name
                            }).ToList();
                    itemKeywordsListView.DataBind();
                }
            }
        }
    }
}