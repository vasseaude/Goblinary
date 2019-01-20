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
    public partial class AbilityList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.MetaDescription += " List of all ability scores.";

            GridView gridControl = (GridView)Page.LoadControl("~/Controls/AbilityListControls.ascx").FindControl("AbilitiesGridView");
			using (WikiDataContext context = new WikiDataContext())
			{
				gridControl.DataSource = (
						from a in context.Set<Ability>()
						orderby a.Name
						select a
					).ToList();
				gridControl.DataBind();
				gridControl.HeaderRow.TableSection = TableRowSection.TableHeader;
				gridControl.Attributes.Add("tableName", "a");
				this.AbilityListDiv.Controls.Add(gridControl);
			}
        }
    }
}