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
    public partial class RecipeList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			if (!this.IsPostBack)
			{
				using (WikiDataContext context = new WikiDataContext())
				{
					string recipeTypeName = this.Request.QueryString["type"] != null ? this.Request.QueryString["type"] : "Recipe";
					EntityType recipeType = (
							from et in context.Set<EntityType>()
							where et.BaseType_Name == "Recipe" && et.Name == recipeTypeName
							select et
						).FirstOrDefault();

					this.Page.MetaDescription += String.Format(" List of all recipes with type: {0}", recipeType.DisplayName);
					this.Page.Title = String.Format("{0} | Recipes", recipeType.DisplayName);
					this.recipeTitle2.InnerText = String.Format(" - all {0}s", recipeType.DisplayName);

					GridView gridControl = (GridView)Page.LoadControl("~/Controls/Recipe/" + recipeType.Name + ".ascx").FindControl("RecipesGridView");
					gridControl.DataSource = (
							from r in context.Set<Recipe>()
							from pm in r.RecipeType.ParentMappings
							where pm.ParentType_Name == recipeType.Name
							orderby r.Name
							select r
						).ToList();
					gridControl.RowDataBound += new GridViewRowEventHandler(gridControl_RowDataBound);
					gridControl.DataBind();

					gridControl.HeaderRow.TableSection = TableRowSection.TableHeader;
					gridControl.Attributes.Add("tableName", "a");

					this.RecipeListDiv.Controls.Add(gridControl);
				}
			}

            // insert search notes
            Table notesTable = (Table)Page.LoadControl("~/Controls/TablesorterNotes.ascx").FindControl("TablesorterNotesTable");
            notes.Controls.Add(notesTable);
        }

        protected void gridControl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void recipeTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("~/RecipeList?type={0}", this.recipeTypeList.SelectedValue));
        }

        protected void recipeTypeList_PreRender(object sender, EventArgs e)
        {
			this.recipeTypeList.SelectedValue = this.Request.QueryString["type"] != null ? this.Request.QueryString["type"] : "Item";
        }
    }
}