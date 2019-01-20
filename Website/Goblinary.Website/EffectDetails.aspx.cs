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

	using Goblinary.Website.Controls;
	using Goblinary.WikiData.Model;
	using Goblinary.WikiData.SqlServer;

	public partial class EffectDetails : System.Web.UI.Page
    {
		//public static string GetLink(IEffectReference effectReference)
		//{
		//	string effectLink = string.Format("<a class=\"{2}_EffectCSS\" href=\"/EffectDetails?effect={0}\">{1}</a>",
		//		HttpUtility.UrlEncode(effectReference.EffectDescription.Effect_Name),
		//		effectReference.EffectDescription.Effect_Name,
		//		effectReference.EffectType);
		//	return string.Format(effectReference.EffectDescription.FormattedDescription,
		//		effectLink,
		//		"",
		//		"<br/>&nbsp;&nbsp;&nbsp;&nbsp;");
		//}
		public static string GetLink(Effect effect, string effectType = "Standard")
		{
			return string.Format("<a class=\"{2}_EffectCSS\" href=\"/EffectDetails?effect={0}\">{1}</a>",
				HttpUtility.UrlEncode(effect.Name),
				effect.Name,
				effectType);
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            string QS_Effect_Name = HttpUtility.UrlDecode(Request.QueryString["effect"]);
            Page.MetaDescription += String.Format(" Effect details for {0}.", QS_Effect_Name);
            Page.Title = QS_Effect_Name + " | Effect"; // shouldn't be the querystring - needs to change later
            featTitle.InnerHtml = QS_Effect_Name; // shouldn't be the querystring - needs to change later

            Label noFeatsLabel1 = new Label();
            noFeatsLabel1.Text = "No feats to show in this list.";
            Label noFeatsLabel2 = new Label();
            noFeatsLabel2.Text = "No feats to show in this list.";
            Label noFeatsLabel3 = new Label();
            noFeatsLabel3.Text = "No feat ranks to show in this list.";
            using (WikiDataContext context = new WikiDataContext())
            {
                var effect = (
                        from ef in context.Set<Effect>()
                        where ef.Name == QS_Effect_Name
                        select ef
                    ).First();

                if (effect != null)
                {
                    Label EffectDescriptionLabel = new Label();
                    EffectDescriptionLabel.CssClass = "descriptionLabel";

                    if (effect.EffectTerm != null)
                    {
                        EffectDescriptionLabel.Text = effect.EffectTerm.Description;
                        EffectBlock.Controls.Add(EffectDescriptionLabel);
                        ListView effectListView = (ListView)Page.LoadControl("~/Controls/EffectDetailsControls.ascx").FindControl("EffectListView");
                        effectListView.DataSource = new List<EffectTerm>() { effect.EffectTerm };
                        effectListView.DataBind();
                        EffectBlock.Controls.Add(effectListView);
                    }
                    else
                    {
                        EffectDescriptionLabel.Text = "Goblinworks has not provided a description for this effect.";
                        EffectBlock.Controls.Add(EffectDescriptionLabel);
                    }

                    var featsWithEffect = (
                            from ed in effect.EffectDescriptions
                            from fe in ed.FeatEffects
                            orderby fe.Feat.Name
                            select fe.Feat).Distinct().ToList();
                    if (featsWithEffect.Count > 0)
                    {
                        GridView gridControl1 = (GridView)Page.LoadControl("~/Controls/EffectDetailsControls.ascx").FindControl("FeatsGridView");
                        gridControl1.ID = "FeatsGridView1";
                        gridControl1.DataSource = featsWithEffect;
                        gridControl1.RowDataBound += new GridViewRowEventHandler(gridControl1_RowDataBound);
                        gridControl1.DataBind();
                        if (gridControl1.Rows.Count > 0) { gridControl1.HeaderRow.TableSection = TableRowSection.TableHeader; }
                        gridControl1.Attributes.Add("tableName", "a");
                        FeatsBlock1.Controls.Add(gridControl1);
                    }
                    else
                    {
                        FeatsBlock1.Controls.Add(noFeatsLabel1);
                    }

					// TODO: Fix query for Feats that capitalize on a Condition once Conditions are in.
					//var featsWithState = (
					//	from sd in effect.StateDescriptions
					//	from fe in sd.FeatEffects
					//	orderby fe.Feat.Name
					//	select fe.Feat).Distinct().ToList();
					//if (featsWithState.Count > 0)
					//{
					//	GridView gridControl2 = (GridView)Page.LoadControl("~/Controls/EffectDetailsControls.ascx").FindControl("FeatsGridView");
					//	gridControl2.ID = "FeatsGridView2";
					//	gridControl2.DataSource = featsWithState;
					//	gridControl2.RowDataBound += new GridViewRowEventHandler(gridControl2_RowDataBound);
					//	gridControl2.DataBind();
					//	if (gridControl2.Rows.Count > 0) { gridControl2.HeaderRow.TableSection = TableRowSection.TableHeader; }
					//	gridControl2.Attributes.Add("tableName", "b");
					//	FeatsBlock2.Controls.Add(gridControl2);
					//}
					//else
					//{
						FeatsBlock2.Controls.Add(noFeatsLabel2);
					//}

                    var featsWithRanksThatApplyState = (
                        from ed in effect.EffectDescriptions
                        from fre in ed.FeatRankEffects
                        orderby fre.FeatRank.Feat_Name, fre.FeatRank.Rank
                        select fre.FeatRank).Distinct().ToList();
                    if (featsWithRanksThatApplyState.Count > 0)
                    {
                        GridView gridControl3 = (GridView)Page.LoadControl("~/Controls/EffectDetailsControls.ascx").FindControl("FeatsGridView3");
                        gridControl3.ID = "FeatsGridView3";
                        gridControl3.DataSource = featsWithRanksThatApplyState;
                        gridControl3.RowDataBound += new GridViewRowEventHandler(gridControl3_RowDataBound);
                        gridControl3.DataBind();
                        if (gridControl3.Rows.Count > 0) { gridControl3.HeaderRow.TableSection = TableRowSection.TableHeader; }
                        gridControl3.Attributes.Add("tableName", "c");
                        FeatsBlock3.Controls.Add(gridControl3);
                    }
                    else
                    {
                        FeatsBlock3.Controls.Add(noFeatsLabel3);
                    }
                }
            }
            // insert search notes
            Table notesTable = (Table)Page.LoadControl("~/Controls/TablesorterNotes.ascx").FindControl("TablesorterNotesTable");
            notes.Controls.Add(notesTable);
        }

        protected void gridControl1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ListView featEffectsListView = (ListView)Page.LoadControl("~/Controls/FeatEffects.ascx").FindControl("FeatEffectsListView");
				featEffectsListView.DataSource = (
						from fe in ((Feat)e.Row.DataItem).Effects
						orderby FeatEffects.GetEffectTypeSortOrder(fe.EffectType)
						select new
						{
							EffectType = fe.EffectType,
							Description = FeatEffects.GetFeatEffectDescription(fe.EffectDescription, fe.EffectType)
						}
					);
                featEffectsListView.DataBind();
                PlaceHolder FeatEffectsPlaceholder = (PlaceHolder)e.Row.FindControl("FeatEffectsPlaceholder");
                if (FeatEffectsPlaceholder != null)
                {
                    FeatEffectsPlaceholder.Controls.Add(featEffectsListView);
                }
            }
        }

        protected void gridControl2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ListView featEffectsListView = (ListView)Page.LoadControl("~/Controls/FeatEffects.ascx").FindControl("FeatEffectsListView");
				featEffectsListView.DataSource = (
						from fe in ((Feat)e.Row.DataItem).Effects
						orderby FeatEffects.GetEffectTypeSortOrder(fe.EffectType)
						select new
						{
							EffectType = fe.EffectType,
							Description = FeatEffects.GetFeatEffectDescription(fe.EffectDescription, fe.EffectType)
						}
					);
				featEffectsListView.DataBind();
				PlaceHolder FeatEffectsPlaceholder = (PlaceHolder)e.Row.FindControl("FeatEffectsPlaceholder");
                if (FeatEffectsPlaceholder != null)
                {
                    FeatEffectsPlaceholder.Controls.Add(featEffectsListView);
                }
            }
        }

        protected void gridControl3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var currentFeatRank = (FeatRank)e.Row.DataItem;
                ListView featEffectsListView = (ListView)Page.LoadControl("~/Controls/FeatEffects.ascx").FindControl("FeatEffectsListView");
				featEffectsListView.DataSource = (
						from fe in ((FeatRank)e.Row.DataItem).Effects
						orderby FeatEffects.GetEffectTypeSortOrder("FeatRank")
						select new
						{
							EffectType = "FeatRank",
							Description = FeatEffects.GetFeatEffectDescription(fe.EffectDescription, "FeatRank")
						}
					);
				featEffectsListView.DataBind();
                PlaceHolder featRankEffectsPlaceholder = (PlaceHolder)e.Row.FindControl("FeatRankEffectsPlaceholder");
                featRankEffectsPlaceholder.Controls.Add(featEffectsListView);
            }
        }
    }
}