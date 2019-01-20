using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;
using System.Reflection;

using Goblinary.Common;
using Goblinary.WikiData.Model;
using Goblinary.WikiData.SqlServer;

namespace Goblinary.Website
{
    public partial class ItemList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			if (!this.IsPostBack)
			{
				using (WikiDataContext context = new WikiDataContext())
				{
					string itemTypeName = this.Request.QueryString["type"] != null ? this.Request.QueryString["type"] : "Item";
					EntityType itemType = (
							from et in context.Set<EntityType>()
							where et.BaseType_Name == "Item" && et.Name == itemTypeName
							select et
						).FirstOrDefault();

					this.Page.MetaDescription += String.Format(" Comprehensive list of all items with type: {0}", itemType.DisplayName);
					this.Page.Title = String.Format("{0} | Items", itemType.DisplayName);
					this.itemTitle2.InnerText = String.Format(" - all {0}s", itemType.DisplayName);

					GridView gridControl = (GridView)Page.LoadControl("~/Controls/Item/" + itemType.Name + ".ascx").FindControl("ItemsGridView");
					gridControl.DataSource = (
							from i in context.Set<Item>()
							from pm in i.ItemType.ParentMappings
							where pm.ParentType_Name == itemType.Name
							orderby i.Name
							select i
						).ToList();
					gridControl.RowDataBound += new GridViewRowEventHandler(gridControl_RowDataBound);
					gridControl.DataBind();

					gridControl.HeaderRow.TableSection = TableRowSection.TableHeader;
					gridControl.Attributes.Add("tableName", "a");

					this.ItemListDiv.Controls.Add(gridControl);
				}
			}
            // insert search notes
            Table notesTable = (Table)Page.LoadControl("~/Controls/TablesorterNotes.ascx").FindControl("TablesorterNotesTable");
            notes.Controls.Add(notesTable);
        }

        protected void gridControl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ListView stockItemStocksListView = (ListView)e.Row.FindControl("StockItemStocksListView");
                if (stockItemStocksListView != null)
                {
                    stockItemStocksListView.DataSource = DataBinder.Eval(e.Row.DataItem, "StockItemStocks");
                    stockItemStocksListView.DataBind();
                }
            }
        }

        protected void itemTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
			Response.Redirect(String.Format("~/ItemList?type={0}", this.itemTypeList.SelectedValue));
        }

        protected void itemTypeList_PreRender(object sender, EventArgs e)
        {
			this.itemTypeList.SelectedValue = this.Request.QueryString["type"] != null ? this.Request.QueryString["type"] : "Item";
        }
    }
}