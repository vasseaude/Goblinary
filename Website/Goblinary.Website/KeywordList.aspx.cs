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
    public partial class KeywordList : System.Web.UI.Page
    {
		protected void Page_Load(object sender, EventArgs e)
		{
			Page.MetaDescription += " List of all keywords.";
			GridView gridControl = (GridView)Page.LoadControl("~/Controls/KeywordListControls.ascx").FindControl("KeywordsGridView");
			using (WikiDataContext context = new WikiDataContext())
			{
				gridControl.DataSource = (
						from k in context.Set<Keyword>()
						orderby k.Name
						select k
					).ToList();
				gridControl.DataBind();
				gridControl.HeaderRow.TableSection = TableRowSection.TableHeader;
				gridControl.Attributes.Add("tableName", "a");
				KeywordListDiv.Controls.Add(gridControl);
			}
		}
    }
}