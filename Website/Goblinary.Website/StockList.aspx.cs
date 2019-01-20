using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Goblinary.WikiData.Model;
using Goblinary.WikiData.SqlServer;

namespace Goblinary.Website
{
    public partial class StockList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.MetaDescription += " List of all stocks.";
            GridView gridControl = (GridView)Page.LoadControl("~/Controls/StockListControls.ascx").FindControl("StocksGridView");
            using (WikiDataContext context = new WikiDataContext())
            {
                gridControl.DataSource = (
                        from s in context.Set<Stock>()
                        orderby s.Name
                        select s
                    ).ToList();
                gridControl.DataBind();
                gridControl.HeaderRow.TableSection = TableRowSection.TableHeader;
                gridControl.Attributes.Add("tableName", "a");
                this.StockListDiv.Controls.Add(gridControl);
            }
        }
    }
}