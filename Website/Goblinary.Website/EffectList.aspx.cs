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
    public partial class EffectList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.MetaDescription += " List of all effects.";
            GridView gridControl = (GridView)Page.LoadControl("~/Controls/EffectListControls.ascx").FindControl("EffectsGridView");
			using (WikiDataContext context = new WikiDataContext())
			{
                gridControl.DataSource = (
						from ef in context.Set<Effect>()
						orderby ef.Name
						select ef).ToList();
				gridControl.DataBind();
				gridControl.HeaderRow.TableSection = TableRowSection.TableHeader;
				gridControl.Attributes.Add("tableName", "a");
				EffectListDiv.Controls.Add(gridControl);
			}
            // insert search notes
            Table notesTable = (Table)Page.LoadControl("~/Controls/TablesorterNotes.ascx").FindControl("TablesorterNotesTable");
            notes.Controls.Add(notesTable);
        }
    }
}