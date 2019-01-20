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
    public partial class StockDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string QS_Stock_Name = HttpUtility.UrlDecode(Request.QueryString["stock"]);
            Page.MetaDescription += String.Format(" Stock details for {0}.", QS_Stock_Name);
            Page.Title = QS_Stock_Name + " | Stock"; // shouldn't be the querystring - needs to change later
            stockTitle.InnerHtml = QS_Stock_Name; // shouldn't be the querystring - needs to change later
            Label noItemsLabel = new Label();
            noItemsLabel.Text = "No items to show in this list.";
            Label noRecipesLabel = new Label();
            noRecipesLabel.Text = "No recipes to show in this list.";

            using (WikiDataContext context = new WikiDataContext())
            {
                var stockItemsThatCount = (
                    from si in context.Set<StockItem>()
                    from sis in si.StockItemStocks
                    where sis.Stock_Name == QS_Stock_Name
                    select si).ToList();
                if (stockItemsThatCount.Count() > 0)
                {
                    GridView gridControl1 = (GridView)Page.LoadControl("~/Controls/StockDetailsControls.ascx").FindControl("ItemsGridView");
                    gridControl1.ID = "ItemsGridView";
                    gridControl1.DataSource = stockItemsThatCount;
                    gridControl1.RowDataBound += new GridViewRowEventHandler(gridControl1_RowDataBound);
                    gridControl1.DataBind();
                    if (gridControl1.Rows.Count > 0) { gridControl1.HeaderRow.TableSection = TableRowSection.TableHeader; }
                    gridControl1.Attributes.Add("tableName", "a");
                    ItemsBlock.Controls.Add(gridControl1);
                }
                else
                {
                    ItemsBlock.Controls.Add(noItemsLabel);
                }
                var recipesRequiringStock = (
                    from rri in context.Set<RefiningRecipeIngredient>()
                    where rri.Stock_Name == QS_Stock_Name
                    select rri.Recipe
                    ).ToList();
                if (recipesRequiringStock.Count() > 0)
                {
                    GridView gridControl2 = (GridView)Page.LoadControl("~/Controls/StockDetailsControls.ascx").FindControl("RecipesGridView");
                    gridControl2.ID = "RecipesGridView";
                    gridControl2.DataSource = recipesRequiringStock;
                    //gridControl2.RowDataBound += new GridViewRowEventHandler(gridControl2_RowDataBound);
                    gridControl2.DataBind();
                    if (gridControl2.Rows.Count > 0) { gridControl2.HeaderRow.TableSection = TableRowSection.TableHeader; }
                    gridControl2.Attributes.Add("tableName", "b");
                    RecipesBlock.Controls.Add(gridControl2);
                }
                else
                {
                    RecipesBlock.Controls.Add(noRecipesLabel);
                }
            }
            // insert search notes
            Table notesTable = (Table)Page.LoadControl("~/Controls/TablesorterNotes.ascx").FindControl("TablesorterNotesTable");
            notes.Controls.Add(notesTable);
        }
        protected void gridControl1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var currentStockItem = (StockItem)e.Row.DataItem;
                ListView stocksListView = (ListView)e.Row.FindControl("StocksListView");
                stocksListView.DataSource = (
                    from sis in currentStockItem.StockItemStocks
                    orderby sis.Stock_Name
                    select sis).ToList();
                stocksListView.DataBind();
            }
        }
        protected void gridControl2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    var currentFeatRank2 = (FeatRank)e.Row.DataItem;
            //    ListView rankAbilityRequirementsListView = (ListView)e.Row.FindControl("RankAbilityRequirementsListView");
            //    rankAbilityRequirementsListView.DataSource = (
            //        from ar in currentFeatRank2.AbilityRequirements
            //        orderby ar.RequirementNo, ar.OptionNo
            //        select new
            //        {
            //            Requirement = String.Format("{0}<a href=\"/AbilityDetails?ability={1}\">{2}</a> {3}", (ar.OptionNo == 1 ? "" : " \u21B3 or "), HttpUtility.UrlEncode(ar.Ability_Name), ar.Ability_Name, ar.Value)
            //        });
            //    rankAbilityRequirementsListView.DataBind();
            //}
        }
    }
}