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
	public partial class AbilityDetails : System.Web.UI.Page
	{
		private class AbilityGrantingFeatRank
		{
			public string Feat_Name { get; set; }
			public Feat Feat { get; set; }
			public int? Rank { get; set; }
			public List<FeatRankAbilityBonus> AbilityBonuses { get; set; }
			public decimal? AbilityBonusPerXP { get; set; }
		}

		public static string GetLink(Ability ability)
		{
			return string.Format("<a href=\"/AbilityDetails?ability={0}\">{0}</a>", ability.Name);
		}

		private string abilityName;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.abilityName = HttpUtility.UrlDecode(Request.QueryString["ability"]);
			Page.MetaDescription += String.Format(" Ability Score details for {0}.", this.abilityName);
			Page.Title = this.abilityName + " | Ability Score"; // shouldn't be the querystring - needs to change later
			abilityTitle.InnerHtml = this.abilityName; // shouldn't be the querystring - needs to change later
			Label noFeatsLabel = new Label();
			noFeatsLabel.Text = "No feat ranks to show in this list.";

			using (WikiDataContext context = new WikiDataContext())
			{
				var ranksGrantingAbility = (
						from f in context.Set<Feat>()
						from r in f.Ranks
						from ab in r.AbilityBonuses
						where ab.Ability_Name == this.abilityName
						let bonus = r.AbilityBonuses.Where(x => x.Ability_Name == this.abilityName).FirstOrDefault()
						select new AbilityGrantingFeatRank
						{
							Feat_Name = r.Feat_Name,
							Feat = r.Feat,
							Rank = r.Rank,
							AbilityBonuses = r.AbilityBonuses,
							AbilityBonusPerXP = bonus != null && r.ExpCost > 0 && bonus.Bonus > 0 ? bonus.Bonus / r.ExpCost * 1000000 : null
						}
					).ToList();
				if (ranksGrantingAbility.Count() > 0)
				{
					GridView gridControl1 = (GridView)Page.LoadControl("~/Controls/AbilityDetailsControls.ascx").FindControl("BonusGridView");
					gridControl1.ID = "BonusGridView";
					gridControl1.DataSource = ranksGrantingAbility;
					gridControl1.RowDataBound += new GridViewRowEventHandler(gridControl1_RowDataBound);
					gridControl1.DataBind();
					if (gridControl1.Rows.Count > 0) { gridControl1.HeaderRow.TableSection = TableRowSection.TableHeader; }
					gridControl1.Attributes.Add("tableName", "a");
					RankBonusBlock.Controls.Add(gridControl1);
				}
				else
				{
					RankBonusBlock.Controls.Add(noFeatsLabel);
				}
				var ranksRequiringAbility = (
					from f in context.Set<Feat>()
					from r in f.Ranks
					from ar in r.AbilityRequirements
					where ar.Ability_Name == this.abilityName
					select r).Distinct().ToList();
				if (ranksRequiringAbility.Count() > 0)
				{
					GridView gridControl2 = (GridView)Page.LoadControl("~/Controls/AbilityDetailsControls.ascx").FindControl("RequirementGridView");
					gridControl2.ID = "RequirementGridView";
					gridControl2.DataSource = ranksRequiringAbility;
					gridControl2.RowDataBound += new GridViewRowEventHandler(gridControl2_RowDataBound);
					gridControl2.DataBind();
					if (gridControl2.Rows.Count > 0) { gridControl2.HeaderRow.TableSection = TableRowSection.TableHeader; }
					gridControl2.Attributes.Add("tableName", "b");
					RankRequirementBlock.Controls.Add(gridControl2);
				}
				else
				{
					RankRequirementBlock.Controls.Add(noFeatsLabel);
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
				var currentFeatRank1 = (AbilityGrantingFeatRank)e.Row.DataItem;
				ListView rankAbilityBonusesListView = (ListView)e.Row.FindControl("RankAbilityBonusesListView");
				rankAbilityBonusesListView.DataSource = (
					from ab in currentFeatRank1.AbilityBonuses
					orderby ab.Ability_Name == this.abilityName ? 1 : 2, ab.Ability_Name
					select new
					{
						Bonus = String.Format("{0}<a href=\"/AbilityDetails?ability={1}\">{2}</a> {3}", (ab.OptionNo == 1 ? "" : " \u21B3 or "), HttpUtility.UrlEncode(ab.Ability_Name), ab.Ability_Name, ab.Bonus)
					});
				rankAbilityBonusesListView.DataBind();
			}
		}
		protected void gridControl2_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				var currentFeatRank2 = (FeatRank)e.Row.DataItem;
				ListView rankAbilityRequirementsListView = (ListView)e.Row.FindControl("RankAbilityRequirementsListView");
				rankAbilityRequirementsListView.DataSource = (
					from ar in currentFeatRank2.AbilityRequirements
					orderby ar.RequirementNo, ar.OptionNo
					select new
					{
						Requirement = String.Format("{0}<a href=\"/AbilityDetails?ability={1}\">{2}</a> {3}", (ar.OptionNo == 1 ? "" : " \u21B3 or "), HttpUtility.UrlEncode(ar.Ability_Name), ar.Ability_Name, ar.Value)
					});
				rankAbilityRequirementsListView.DataBind();
			}
		}
	}
}