using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;

using Goblinary.WikiData.Model;
using Goblinary.WikiData.SqlServer;

namespace Goblinary.Website
{
	public partial class AchievementDetails : System.Web.UI.Page
	{
		internal static string GetLink(Achievement achievement)
		{
			return string.Format("<a href=\"/AchievementDetails?achievement={0}\">{1}</a>", achievement.Name, achievement.AchievementGroup_Name);
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            string QS_Achievement_Name = HttpUtility.UrlDecode(Request.QueryString["achievement"]);
			using (WikiDataContext context = new WikiDataContext())
			{
				var achievements = (
						from ag in context.Set<AchievementGroup>()
						from a in ag.Achievements
						where ag.Name == QS_Achievement_Name
						select a
					).ToList();

					var achievement = achievements[0];
                    Page.MetaDescription += String.Format(" Achievement details and ranks for {0} - a {1}.", achievement.AchievementGroup.Name, achievement.AchievementGroup.AchievementType.DisplayName);
					Page.Title = achievement.AchievementGroup.Name + " | Achievement";
					achievementTitle.InnerHtml = achievement.AchievementGroup.Name;

					Label dd = new Label();
					// temporary until we can get a real DiscriminatorDescription from the database
                    dd.Text = String.Format("Goblinworks has not provided a description for type: <b><a href=\"/AchievementList?type={0}\">{1}</a></b>", HttpUtility.UrlEncode(achievement.AchievementGroup.AchievementType.Name), achievement.AchievementGroup.AchievementType.DisplayName);
					AchievementBlock.Controls.Add(dd);

					GridView gridControl = (GridView)Page.LoadControl("~/Controls/Achievement/" + achievement.AchievementGroup.AchievementType.Name + ".ascx").FindControl("AchievementRanksGridView");
					gridControl.DataSource = (
							from a in achievements
							from ar in a.Ranks
							select ar
						).ToList();
					gridControl.RowDataBound += new GridViewRowEventHandler(gridControl_RowDataBound);
					gridControl.DataBind();
					gridControl.HeaderRow.TableSection = TableRowSection.TableHeader;
					gridControl.Attributes.Add("tableName", "a");
					RanksBlock.Controls.Add(gridControl);
			}
            // insert search notes
            Table notesTable = (Table)Page.LoadControl("~/Controls/TablesorterNotes.ascx").FindControl("TablesorterNotesTable");
            notes.Controls.Add(notesTable);
        }

        protected void gridControl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ListView rankFeatRequirementsListView = (ListView)e.Row.FindControl("RankFeatRequirementsListView");
                if (rankFeatRequirementsListView != null)
                {
                    var currentFeatAchievementRank = (FeatAchievementRank)e.Row.DataItem;
                    rankFeatRequirementsListView.DataSource = (
                        from cr in currentFeatAchievementRank.RequiredFeats
                        orderby cr.RequirementNo, cr.OptionNo
                        select new
                        {
                            Requirement = String.Format("{0}<a href=\"/FeatDetails?feat={1}\">{2}</a> {3}", (cr.OptionNo == 1 ? "" : " \u21B3 or "), HttpUtility.UrlEncode(cr.Feat_Name), cr.Feat_Name, cr.Feat_Rank)
                        });
                    rankFeatRequirementsListView.DataBind();
                }
                ListView rankCategoryPointsListView = (ListView)e.Row.FindControl("RankCategoryPointsListView");
                if (rankCategoryPointsListView != null)
                {
                    var currentCategoryBonusAchievementRank = (CategoryBonusAchievementRank)e.Row.DataItem;
                    rankCategoryPointsListView.DataSource = (
                        from cr in currentCategoryBonusAchievementRank.CategoryBonuses
                        orderby cr.BonusNo
                        select new
                        {
                            Category = String.Format("<a href=\"/CategoryDetails?category={0}\">{1}</a> {2}", HttpUtility.UrlEncode(cr.Category_Name), cr.Category_Name, cr.Bonus)
                        });
                    rankCategoryPointsListView.DataBind();
                }
            }
        }
	}
}