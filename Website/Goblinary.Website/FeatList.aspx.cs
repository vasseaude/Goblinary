namespace Goblinary.Website
{
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
	using Goblinary.Website.Controls;
	using Goblinary.WikiData.Model;
	using Goblinary.WikiData.SqlServer;

	public partial class FeatList : System.Web.UI.Page
    {
		internal static string GetLink(EntityType featType)
		{
			return string.Format("<a href=\"/FeatList?type={0}\">{1}</a>", featType.Name, featType.DisplayName);
		}

        protected void Page_Load(object sender, EventArgs e)
        {
			if (!this.IsPostBack)
            {
				using (WikiDataContext context = new WikiDataContext())
				{
					string featTypeName = this.Request.QueryString["type"] != null ? this.Request.QueryString["type"] : "Feat";
					EntityType featType = (
							from et in context.Set<EntityType>()
							where et.BaseType_Name == "Feat" && et.Name == featTypeName
							select et
						).FirstOrDefault();

					Page.MetaDescription += String.Format(" Comprehensive list of all feats with type: {0}", featType.DisplayName);
					Page.Title = String.Format("{0} | Feats", featType.DisplayName);
					featTitle2.InnerText = String.Format(" - all {0}s", featType.DisplayName);

					GridView gridControl = (GridView)this.Page.LoadControl("~/Controls/Feat/" + featType.Name + ".ascx").FindControl("FeatsGridView");
					gridControl.DataSource = (
							from f in context.Set<Feat>()
								.Include(x => x.Effects)
								.Include(x => x.Keywords)
								.Include(x => x.Ranks)
								.Include("Effects.EffectDescription")
							from pm in f.FeatType.ParentMappings
							where pm.ParentType_Name == featType.Name
							select f
						).ToList();
					gridControl.RowDataBound += new GridViewRowEventHandler(gridControl_RowDataBound);
					gridControl.DataBind();

					gridControl.HeaderRow.TableSection = TableRowSection.TableHeader;
					gridControl.Attributes.Add("tableName", "a");

					this.RanksBlock.Controls.Add(gridControl);
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
                PlaceHolder featEffectsPlaceholder = (PlaceHolder)e.Row.FindControl("FeatEffectsPlaceholder");
                if (featEffectsPlaceholder != null)
                {
                    ListView featEffectsListView = (ListView)this.Page.LoadControl("~/Controls/FeatEffects.ascx").FindControl("FeatEffectsListView");
					featEffectsListView.DataSource = (
							from fe in ((Feat)e.Row.DataItem).Effects
							orderby FeatEffects.GetEffectTypeSortOrder(fe.EffectType)
							select new
							{
								EffectType = fe.EffectType,
								Description = FeatEffects.GetFeatEffectDescription(fe.EffectDescription, fe.EffectType)
							}
						).ToList();
                    featEffectsListView.DataBind();
                    featEffectsPlaceholder.Controls.Add(featEffectsListView);
                }

                ListView featKeywordsListView = (ListView)e.Row.FindControl("FeatKeywordsListView");
                if (featKeywordsListView != null)
                {
                    featKeywordsListView.DataSource = DataBinder.Eval(e.Row.DataItem, "Keywords");
                    featKeywordsListView.DataBind();
                }
            }
        }

        protected void featTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("~/FeatList?type={0}", this.featTypeList.SelectedValue));
        }

		protected void featTypeList_PreRender(object sender, EventArgs e)
		{
			this.featTypeList.SelectedValue = this.Request.QueryString["type"] != null ? this.Request.QueryString["type"] : "Feat";
		}
    }
}