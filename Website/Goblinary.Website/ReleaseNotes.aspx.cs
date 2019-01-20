using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Goblinary.Website
{
    public partial class ReleaseNotes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			this.Page.MetaDescription += " All release notes.";
        }
    }
}