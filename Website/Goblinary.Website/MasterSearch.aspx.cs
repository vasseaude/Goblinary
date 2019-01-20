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

	using Goblinary.Common;
	using Goblinary.WikiData.Model;
	using Goblinary.WikiData.SqlServer;

	public partial class MasterSearch : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            Page.Form.DefaultFocus = Page.Form.FindControl("searchbox").ClientID;

            GridView gridControl = (GridView)Page.LoadControl("~/Controls/MasterSearchGrid.ascx").FindControl("MasterSearchGridView");
            string searchString = HttpUtility.UrlDecode(Request.QueryString["q"].ToString()).ToLower();
			if (string.IsNullOrEmpty(searchString))
			{
				searchLabel.Text = "You searched for nothing?";
			}
			else
			{
				searchLabel.Text = string.Format("You searched for '{0}', here are your results:", searchString);
			}
            using (WikiDataContext context = new WikiDataContext())
            {
				var achievementResults = (
						from ag in context.Set<AchievementGroup>()
						where ag.Name.ToLower().Contains(searchString)
						select ag
					).ToList();
				var featResults = (
						from f in context.Set<Feat>()
						where f.Name.ToLower().Contains(searchString)
						select f
					).ToList();
				var effectResults = (
						from ef in context.Set<Effect>()
						where ef.Name.ToLower().Contains(searchString)
						select ef
					).ToList();
				var categoryResults = (
						from c in context.Set<AchievementRankCategoryBonus>()
						where c.Category_Name != null && c.Category_Name.ToLower().Contains(searchString)
						select c.Category_Name
					).Distinct().ToList();
				var abilityResults = (
						from ab in context.Set<Ability>()
						where ab.Name.ToLower().Contains(searchString)
						select ab
					).ToList();
				var keywordResults = (
						from k in context.Set<Keyword>()
						where k.Name.ToLower().Contains(searchString)
						select k
					).ToList();
				var itemResults = (
						from i in context.Set<Item>()
						where i.Name.ToLower().Contains(searchString)
						select i
					).ToList();
				var recipeResults = (
						from r in context.Set<Recipe>()
						where r.Name.ToLower().Contains(searchString)
						select r
					).ToList();
                var stockResults = (
                        from s in context.Set<Stock>()
                        where s.Name.ToLower().Contains(searchString)
                        select s
                    ).ToList();
                var structureResults = (
                        from s in context.Set<Structure>()
                        where s.Name.ToLower().Contains(searchString)
                        select s
                    ).ToList();
				var searchResults = (
						from a in achievementResults
						select new
						{
							Name = a.Name,
							ResultSubtype = String.Format("<a href=\"/AchievementList?type={0}\">{1}</a>", HttpUtility.UrlEncode(a.AchievementType.Name), a.AchievementType.DisplayName),
							ResultType = "Achievement",
							ResultURL = string.Format("~/AchievementDetails?achievement={0}", HttpUtility.UrlEncode(a.Name))
						}
					).Union(
						from f in featResults
						select new
						{
							Name = f.Name,
							ResultSubtype = String.Format("<a href=\"/FeatList?type={0}\">{1}</a>", HttpUtility.UrlEncode(f.FeatType.Name), f.FeatType.DisplayName),
							ResultType = "Feat",
							ResultURL = string.Format("~/FeatDetails?feat={0}", HttpUtility.UrlEncode(f.Name))
						}
					).Union(
						from ef in effectResults
						select new
						{
							Name = ef.Name,
							ResultSubtype = "",
							ResultType = "Effect",
							ResultURL = string.Format("~/EffectDetails?effect={0}", HttpUtility.UrlEncode(ef.Name))
						}
					).Union(
						from c in categoryResults
						select new
						{
							Name = c,
							ResultSubtype = "",
							ResultType = "Category",
							ResultURL = string.Format("~/CategoryDetails?category={0}", HttpUtility.UrlEncode(c))
						}
					).Union(
						from ab in abilityResults
						select new
						{
							Name = ab.Name,
							ResultSubtype = "",
							ResultType = "Ability",
							ResultURL = string.Format("~/AbilityDetails?ability={0}", HttpUtility.UrlEncode(ab.Name))
						}
					).Union(
						from k in keywordResults
						select new
						{
							Name = k.Name,
							ResultSubtype = k.KeywordType_Name,
							ResultType = "Keyword",
							ResultURL = string.Format("~/KeywordDetails?keyword={0}&type={1}", HttpUtility.UrlEncode(k.Name), HttpUtility.UrlEncode(k.KeywordType_Name))
						}
					).Union(
						from i in itemResults
						select new
						{
							Name = i.Name,
							ResultSubtype = String.Format("<a href=\"/ItemList?type={0}\">{1}</a>", HttpUtility.UrlEncode(i.ItemType_Name), i.ItemType_Name),
							ResultType = "Item",
							ResultURL = string.Format("~/ItemDetails?item={0}", HttpUtility.UrlEncode(i.Name))
						}
					).Union(
						from r in recipeResults
						select new
						{
							Name = r.Name,
							ResultSubtype = String.Format("<a href=\"/RecipeList?type={0}\">{1}</a>", HttpUtility.UrlEncode(r.RecipeType_Name), r.RecipeType_Name),
							ResultType = "Recipe",
							ResultURL = string.Format("~/RecipeDetails?recipe={0}", HttpUtility.UrlEncode(r.Name))
						}
                    ).Union(
                        from s in stockResults
                        select new
                        {
                            Name = s.Name,
                            ResultSubtype = "",
                            ResultType = "Stock",
                            ResultURL = string.Format("~/StockDetails?stock={0}", HttpUtility.UrlEncode(s.Name))
                        }
                    ).Union(
                        from se in structureResults
                        select new
                        {
                            Name = se.Name,
                            ResultSubtype = String.Format("<a href=\"/StructureList?type={0}\">{1}</a>", HttpUtility.UrlEncode(se.StructureType_Name), se.StructureType_Name), //needs to change to structure.StructureType.DisplayName
                            ResultType = "Structure",
                            ResultURL = string.Format("~/StructureDetails?structure={0}", HttpUtility.UrlEncode(se.Name))
                        }
					).OrderBy(x => x.Name).ToList();
				if (searchResults.Count == 1) // if only return 1 result, just redirect straight to that result
                {
                    Response.Redirect(searchResults[0].ResultURL, true);
                }
                if (searchResults.Count > 0)
                {
                    gridControl.DataSource = searchResults;
                    gridControl.DataBind();
                    gridControl.HeaderRow.TableSection = TableRowSection.TableHeader;
                    gridControl.Attributes.Add("tableName", "a");
                    MasterSearchGridDiv.Controls.Add(gridControl);
                }
            }
		}
	}
}