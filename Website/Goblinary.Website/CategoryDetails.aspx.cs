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
    public partial class CategoryDetails : System.Web.UI.Page
    {
		internal static string GetLink(string categoryName)
		{
			return string.Format("<a href=\"/CategoryDetails?category={0}\">{0}</a>", categoryName);
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            string QS_Category_Name = HttpUtility.UrlDecode(Request.QueryString["category"]);
            Page.MetaDescription += String.Format(" Category details for {0}.", QS_Category_Name);
			Page.Title = QS_Category_Name + " | Category"; // shouldn't be the querystring - needs to change later
            categoryTitle.InnerHtml = QS_Category_Name; // shouldn't be the querystring - needs to change later
            Label noFeatsLabel = new Label();
            noFeatsLabel.Text = "No feats to show in this list.";
            Label noAchievementsLabel = new Label();
            noAchievementsLabel.Text = "No achievements to show in this list.";

            using (WikiDataContext context = new WikiDataContext())
            {
                var featsWithCategory = (
                    from f in context.Set<Feat>()
                    from r in f.Ranks
                    from cr in r.CategoryRequirements
                    where cr.Category_Name == QS_Category_Name
                    select f).Distinct().ToList();
                if (featsWithCategory.Count() > 0)
                {
                    GridView gridControl1 = (GridView)Page.LoadControl("~/Controls/CategoryDetailsControls.ascx").FindControl("FeatsGridView");
                    gridControl1.ID = "FeatsGridView";
                    gridControl1.DataSource = featsWithCategory;
                    gridControl1.DataBind();
                    if (gridControl1.Rows.Count > 0) { gridControl1.HeaderRow.TableSection = TableRowSection.TableHeader; }
                    gridControl1.Attributes.Add("tableName", "a");
                    FeatsBlock.Controls.Add(gridControl1);
                }
                else
                {
                    FeatsBlock.Controls.Add(noFeatsLabel);
                }
                var achievementsWithCategory = (
						from c in context.Set<AchievementRankCategoryBonus>()
						where c.Category_Name == QS_Category_Name
						select c.AchievementRank.Achievement.AchievementGroup
					).Distinct().ToList();

                if (achievementsWithCategory.Count() > 0)
                {
                    GridView gridControl2 = (GridView)Page.LoadControl("~/Controls/CategoryDetailsControls.ascx").FindControl("AchievementsGridView");
                    gridControl2.ID = "AchievementsGridView";
                    gridControl2.DataSource = achievementsWithCategory;
                    gridControl2.DataBind();
                    if (gridControl2.Rows.Count > 0) { gridControl2.HeaderRow.TableSection = TableRowSection.TableHeader; }
                    gridControl2.Attributes.Add("tableName", "b");
                    AchievementsBlock.Controls.Add(gridControl2);
                }
                else
                {
                    AchievementsBlock.Controls.Add(noAchievementsLabel);
                }
            }
            // insert search notes
            Table notesTable = (Table)Page.LoadControl("~/Controls/TablesorterNotes.ascx").FindControl("TablesorterNotesTable");
            notes.Controls.Add(notesTable);
        }
    }
}