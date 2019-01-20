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
    public partial class StructureList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (WikiDataContext context = new WikiDataContext())
            {
				string structureTypeName = this.Request.QueryString["type"] != null ? this.Request.QueryString["type"] : "Structure";
				EntityType structureType = (
						from et in context.Set<EntityType>()
						where et.BaseType_Name == "Structure" && et.Name == structureTypeName
						select et
					).FirstOrDefault();

				this.Page.MetaDescription += String.Format(" Comprehensive list of all structures with type: {0}", structureType.DisplayName);
				this.Page.Title = String.Format("{0} | Structures", structureType.DisplayName);
				//structureTitle2.InnerText = String.Format(" - all {0}s", structureType.DisplayName);

				GridView gridControl = (GridView)Page.LoadControl("~/Controls/Structure/" + structureType.Name + ".ascx").FindControl("StructuresGridView");
				var structuresList = (
						from s in context.Structures
						from pm in s.StructureType.ParentMappings
						where pm.ParentType_Name == structureType.Name
						orderby s.Name
						select s
					).ToList();
				var structuresData = (
					from s in structuresList
					select new
					{
						Name = s.Name,
						StructureType_Name = s.StructureType_Name,
						KitName = (s.Kit != null ? String.Format("<a href=\"/ItemDetails?item={0}\">{1}</a>", HttpUtility.UrlEncode(s.Kit.Name), s.Kit.Name) : null),
						Description = (s is Camp ? ((Camp)s).Description : null),
						Cooldown = (s is Camp ? ((Camp)s).Cooldown : null)
					}
					).ToList();
				gridControl.DataSource = structuresData;
                gridControl.DataBind();
                gridControl.HeaderRow.TableSection = TableRowSection.TableHeader;
                gridControl.Attributes.Add("tableName", "a");
                this.StructureListDiv.Controls.Add(gridControl);
            }
        }

        protected void structureTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("~/StructureList?type={0}", this.structureTypeList.SelectedValue));
        }

        protected void structureTypeList_PreRender(object sender, EventArgs e)
        {
            this.structureTypeList.SelectedValue = this.Request.QueryString["type"] != null ? this.Request.QueryString["type"] : "Structure";
        }
    }
}