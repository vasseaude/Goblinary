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
    public partial class StructureDetails : System.Web.UI.Page
    {
		public class CustomHoldingUpgrade
		{
			public string Structure_Name { get; set; }
			public int? Upgrade { get; set; }
			public int? InfluenceCost { get; set; }
			public string FacilityFeat { get; set; }
			public List<HoldingUpgradeTrainerLevel> TrainerLevels { get; set; }
			public List<HoldingUpgradeBulkResourceBonus> BulkResourceBonuses { get; set; }
			public List<HoldingUpgradeBulkResourceRequirement> BulkResourceRequirements { get; set; }
		}

		public class CustomOutpost
		{
			public string KitName { get; set; }
			public List<OutpostBulkResource> BulkResources { get; set; }
			public List<OutpostWorkerFeat> WorkerFeats { get; set; }
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            string QS_Structure_Name = HttpUtility.UrlDecode(Request.QueryString["structure"]);
            using (WikiDataContext context = new WikiDataContext())
            {
                var structures = (
                    from s in context.Set<Structure>()
						.Include(x => x.StructureType)
                    where s.Name == QS_Structure_Name
                    select s).ToList();
                var structure = structures[0];
                Page.MetaDescription += String.Format(" Structure details and ranks for {0} - a {1}.", structure.Name, structure.StructureType_Name); //needs to change to structure.StructureType.DisplayName
                Page.Title = structure.Name + " | Structure";
                structureTitle.InnerHtml = structure.Name; // set the main header

				Label dd = new Label();
				// temporary until we can get a real description from the database
				dd.Text = String.Format("Goblinworks has not provided a description for type: <b><a href=\"/StructureList?type={0}\">{1}</a></b><br>", HttpUtility.UrlEncode(structure.StructureType_Name), structure.StructureType.DisplayName);
				StructureBlock.Controls.Add(dd);

				GridView gridControl = (GridView)Page.LoadControl("~/Controls/Structure/" + structure.StructureType.Name + ".ascx").FindControl("StructureUpgradeGridView");
				ListView structureDetailsListView = (ListView)Page.LoadControl("~/Controls/Structure/" + structure.StructureType.Name + ".ascx").FindControl("StructureDetailsListView");
				if (structure is Camp)
				{
					gridControl.DataSource = ((Camp)structure).Upgrades;
					structureDetailsListView.DataSource = (
						from s in new List<Camp>() { structure as Camp }
						select new
						{
							Description = s.Description,
							KitName = s.Kit != null ? String.Format("<a href=\"/ItemDetails?item={0}\">{1}</a>", HttpUtility.UrlEncode(s.Kit.Name), s.Kit.Name) : null,
							Cooldown = s.Cooldown
						}).ToList();
				}
				else if (structure is Holding)
				{
					gridControl.DataSource = (
						from u in ((Holding)structure).Upgrades
						select new CustomHoldingUpgrade
						{
							Structure_Name = u.Structure_Name,
							Upgrade = u.Upgrade,
							InfluenceCost = u.InfluenceCost,
							FacilityFeat = u.CraftingFacilityFeat_Name != null ? String.Format("<a href=\"/FeatDetails?feat={0}\">{1}</a> ({2})", HttpUtility.UrlEncode(u.CraftingFacilityFeat_Name), u.CraftingFacilityFeat_Name, u.CraftingFacilityQuality) : null,
							TrainerLevels = u.TrainerLevels,
							BulkResourceBonuses = u.BulkResourceBonuses,
							BulkResourceRequirements = u.BulkResourceRequirements
						}).ToList();
					structureDetailsListView.DataSource = (
						from s in new List<Holding>() { structure as Holding }
						select new
						{
							KitName = s.Kit != null ? String.Format("<a href=\"/ItemDetails?item={0}\">{1}</a>", HttpUtility.UrlEncode(s.Kit.Name), s.Kit.Name) : null
						}).ToList();
				}
				else if (structure is Outpost)
				{
					gridControl.DataSource = ((Outpost)structure).Upgrades;
					structureDetailsListView.DataSource = (
						from s in new List<Outpost>() { structure as Outpost }
						select new CustomOutpost
						{
							KitName = s.Kit != null ? String.Format("<a href=\"/ItemDetails?item={0}\">{1}</a>", HttpUtility.UrlEncode(s.Kit.Name), s.Kit.Name) : null,
							BulkResources = s.BulkResources,
							WorkerFeats = s.WorkerFeats
						}).ToList();
				}
				gridControl.RowDataBound += new GridViewRowEventHandler(gridControl_RowDataBound);
				gridControl.DataBind();
				if (gridControl.Rows.Count > 0) { gridControl.HeaderRow.TableSection = TableRowSection.TableHeader; }
				gridControl.Attributes.Add("tableName", "a");
				UpgradesBlock.Controls.Add(gridControl);

				structureDetailsListView.ItemDataBound += new EventHandler<ListViewItemEventArgs>(structureDetailsListView_ItemDataBound);
				structureDetailsListView.DataBind();
				this.StructureBlock.Controls.Add(structureDetailsListView);
            }
            // insert search notes
            Table notesTable = (Table)Page.LoadControl("~/Controls/TablesorterNotes.ascx").FindControl("TablesorterNotesTable");
            notes.Controls.Add(notesTable);
		}

		protected void gridControl_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				if (e.Row.DataItem is CustomHoldingUpgrade)
				{
					var currentUpgrade = (CustomHoldingUpgrade)e.Row.DataItem;

					ListView trainerLevelsListView = (ListView)e.Row.FindControl("TrainerLevelsListView");
					trainerLevelsListView.DataSource = currentUpgrade.TrainerLevels;
					trainerLevelsListView.DataBind();

					ListView bulkResourceBonusesListView = (ListView)e.Row.FindControl("BulkResourceBonusesListView");
					bulkResourceBonusesListView.DataSource = currentUpgrade.BulkResourceBonuses;
					bulkResourceBonusesListView.DataBind();

					ListView bulkResourceRequirementsListView = (ListView)e.Row.FindControl("BulkResourceRequirementsListView");
					bulkResourceRequirementsListView.DataSource = currentUpgrade.BulkResourceRequirements;
					bulkResourceRequirementsListView.DataBind();
				}
			}
		}

		protected void structureDetailsListView_ItemDataBound(object sender, ListViewItemEventArgs e)
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				if (e.Item.DataItem is CustomOutpost)
				{
					ListView bulkResourcesListView = (ListView)e.Item.FindControl("BulkResourcesListView");
					bulkResourcesListView.DataSource = ((CustomOutpost)e.Item.DataItem).BulkResources;
					bulkResourcesListView.DataBind();

					ListView workerFeatsListView = (ListView)e.Item.FindControl("WorkerFeatsListView");
					workerFeatsListView.DataSource = (
						from w in ((CustomOutpost)e.Item.DataItem).WorkerFeats
						select new
						{
							WorkerFeat = w.WorkerFeat_Name != null ? String.Format("<a href=\"/FeatDetails?feat={0}\">{1}</a>", HttpUtility.UrlEncode(w.WorkerFeat_Name), w.WorkerFeat_Name) : null
						}).ToList();
					workerFeatsListView.DataBind();
				}
			}
		}
    }
}