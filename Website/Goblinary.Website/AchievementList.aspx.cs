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

	using Goblinary.WikiData.Model;
	using Goblinary.WikiData.SqlServer;

	public partial class AchievementList : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!this.IsPostBack)
            {
                using (WikiDataContext context = new WikiDataContext())
                {
                    string achievementTypeName = this.Request.QueryString["type"] != null ? this.Request.QueryString["type"] : "Achievement";
                    EntityType achievementType = (
                            from et in context.Set<EntityType>()
                            where et.BaseType_Name == "Achievement" && et.Name == achievementTypeName
                            select et
                        ).FirstOrDefault();

                    Page.MetaDescription += String.Format(" List of all achievements with type: {0}", achievementType.DisplayName);
                    Page.Title = String.Format("{0} | Achievements", achievementType.DisplayName);
                    achievementTitle2.InnerText = String.Format(" - all {0}s", achievementType.DisplayName);

                    GridView gridControl = (GridView)Page.LoadControl("~/Controls/AchievementListControls.ascx").FindControl("AchievementsGridView");
                    gridControl.DataSource = (
                            from ag in context.Set<AchievementGroup>()
                                .Include(x => x.Achievements)
                                .Include("Achievements.Ranks")
							from pm in ag.AchievementType.ParentMappings
							where pm.ParentType_Name == achievementTypeName
                            orderby ag.Name
                            select ag
                        ).ToList();
                    gridControl.RowDataBound += new GridViewRowEventHandler(gridControl_RowDataBound);
                    gridControl.DataBind();

                    gridControl.HeaderRow.TableSection = TableRowSection.TableHeader;
                    gridControl.Attributes.Add("tableName", "a");

                    AchievementListDiv.Controls.Add(gridControl);
                }
                // insert search notes
                Table notesTable = (Table)Page.LoadControl("~/Controls/TablesorterNotes.ascx").FindControl("TablesorterNotesTable");
                notes.Controls.Add(notesTable);
            }
		}

		protected void gridControl_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				ListView rankCategoryPointsListView = (ListView)e.Row.FindControl("RankCategoryPointsListView");
				var currentAchievement = (AchievementGroup)e.Row.DataItem;
				rankCategoryPointsListView.DataSource = (
						from a in currentAchievement.Achievements
						from ar in a.Ranks
						where ar is CategoryBonusAchievementRank
						from c in ((CategoryBonusAchievementRank)ar).CategoryBonuses
						select c
					).Distinct(new AchievementRankCategoryBonusComparer()).ToList();
				rankCategoryPointsListView.DataBind();
			}
		}

        protected void achievementTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("~/AchievementList?type={0}", this.achievementTypeList.SelectedValue));
        }

        protected void achievementTypeList_PreRender(object sender, EventArgs e)
        {
            this.achievementTypeList.SelectedValue = this.Request.QueryString["type"] != null ? this.Request.QueryString["type"] : "Achievement";
        }
	}
}