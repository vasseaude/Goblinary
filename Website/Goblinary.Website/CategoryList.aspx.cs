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
    public partial class CategoryList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.MetaDescription += " List of all categories.";
            GridView gridControl = (GridView)Page.LoadControl("~/Controls/CategoryListControls.ascx").FindControl("CategoriesGridView");
			using (WikiDataContext context = new WikiDataContext())
			{
                gridControl.DataSource = (
						from a in context.Set<AchievementRankCategoryBonus>()
						select new
						{
							Name = a.Category_Name
						}).Distinct().ToList();
				gridControl.DataBind();
				gridControl.HeaderRow.TableSection = TableRowSection.TableHeader;
				gridControl.Attributes.Add("tableName", "a");
				CategoryListDiv.Controls.Add(gridControl);
			}
        }
    }
}